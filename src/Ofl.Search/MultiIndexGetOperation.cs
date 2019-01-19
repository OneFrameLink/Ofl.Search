using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Linq;

namespace Ofl.Search
{
    internal class MultiIndexGetOperation<T> : IMultiIndexGetOperation
        where T : class
    {
        #region Constructor

        public MultiIndexGetOperation(IIndex index)
        {
            // Validate parameters.
            if (index == null) throw new ArgumentNullException(nameof(index));
            if (!(index is IIndex<T> typedIndex)) throw new ArgumentException($"The index { index.Name } does not implement the {nameof(IIndex<T>)} interface, where {nameof(T)} is {typeof(T).FullName}.");

            // Assign values.
            _index = typedIndex;
        }

        #endregion

        #region Instance, read-only state

        private readonly IIndex<T> _index;

        #endregion

        #region Helpers

        public async Task<GetResponse<object>> GetAsync(IEnumerable<object> ids, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (ids == null) throw new ArgumentNullException(nameof(ids));

            // Get the read only operations.
            IIndexReadOperations<T> readOperations = await _index.GetReadOperationsAsync(cancellationToken)
                .ConfigureAwait(false);

            // Create the get request.
            var request = new GetRequest { Ids = ids.ToReadOnlyCollection() };

            // Get.
            GetResponse<T> response = await readOperations.GetAsync(request, cancellationToken)
                .ConfigureAwait(false);

            // Yield the results.
            return new GetResponse<object> {
                Request = response.Request,
                TotalHits = response.TotalHits,
                Hits = response
                    .Hits
                    .Select(h => new Hit<object> {
                        Highlights = h.Highlights,
                        Id = h.Id,
                        Index = h.Index,
                        Item = h.Item,
                        Score = h.Score
                    })
                    .ToReadOnlyCollection()
            };
        }

        #endregion
    }
}
