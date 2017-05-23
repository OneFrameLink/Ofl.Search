using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Linq;

namespace Ofl.Search
{
    public class IndexManager : IIndexManager
    {
        #region Constructor

        public IndexManager(IEnumerable<IIndex> indexes)
        {
            // Validate parameters.
            _indexes = indexes?.ToReadOnlyDictionary(i => i.Name) ?? throw new ArgumentNullException(nameof(indexes));
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
            IReadOnlyCollection<IIndex> indicies = _indexes.Values.ToReadOnlyCollection();

            // Return.
            return Task.FromResult(indicies);
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
            // Dispose of unamanged resources.

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
