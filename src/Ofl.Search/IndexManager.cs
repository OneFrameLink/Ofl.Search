using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Linq;

namespace Ofl.Search
{
    public class IndexManager : IIndexManager
    {
        #region Constructor

        public IndexManager(IIndexFactory indexFactory)
        {
            // Validate parameters.
            if (indexFactory == null) throw new ArgumentNullException(nameof(indexFactory));

            // Set indexes.
            _indexes = indexFactory
                .CreateIndices()
                .ToReadOnlyDictionary(i => i.Name);
        }

        #endregion

        #region Instance, read-only state.

        private readonly IReadOnlyDictionary<string, IIndex> _indexes;

        #endregion

        #region Implementation of IIndexManager

        public virtual Task<IIndex> GetIndexAsync(string name, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            // Lookup.
            IIndex index = _indexes[name];

            // Return the value.
            return Task.FromResult(index);
        }

        public virtual Task<IReadOnlyCollection<IIndex>> GetIndexesAsync(CancellationToken cancellationToken)
        {
            // Just return the values.
            IReadOnlyCollection<IIndex> indices = _indexes.Values.ToReadOnlyCollection();

            // Return.
            return Task.FromResult(indices);
        }

        public virtual async Task<MultiIndexGetResponse> MultiIndexGetAsync(MultiIndexGetRequest request,
            CancellationToken cancellationToken)
        {
            // Validate parameters
            if (request == null) throw new ArgumentNullException(nameof(request));

            // The tasks.
            var tasks = new List<Task<GetResponse<object>>>(request.Ids.Count);

            // Cycle through the indexes (which are the keys), get the index.
            foreach (IGrouping<string, object> group in request.Ids)
            {
                // Get the index.
                IIndex index = _indexes[group.Key];

                // Create the get operation.
                var operation = (IMultiIndexGetOperation) Activator.CreateInstance(
                    MultiIndexGetOperationExtensions.OpenMultiIndexGetOperationType.MakeGenericType(index.Type));

                // Call the operation.
                tasks.Add(operation.GetAsync(group, cancellationToken));
            }

            // Wait on all the tasks.
            GetResponse<object>[] results = await Task.WhenAll(tasks)
                .ConfigureAwait(false);

            // Combine everything.  Create the root.
            MultiIndexGetResponse response = results.Aggregate(new MultiIndexGetResponse { Request = request }, (mr, r) => {
                // Add the total hits.
                mr.TotalHits += r.TotalHits;

                // Concat the results.
                mr.Hits = mr.Hits.Concat(r.Hits).ToReadOnlyCollection();

                // Return the item.
                return mr;
            });

            // Return the response.
            return response;
        }

        #endregion

        #region IDisposable implementation.

        public void Dispose()
        {
            // Call the overload, suppress finalize.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Dispose of unmanaged resources.

            // If not disposing, get out.
            if (!disposing) return;

            // Dispose of resources.
            foreach (IIndex disposable in _indexes.Values)
                using (disposable) { }
        }

        ~IndexManager() => Dispose(false);

        #endregion
    }
}
