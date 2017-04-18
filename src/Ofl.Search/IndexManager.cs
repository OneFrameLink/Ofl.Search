using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Core;
using Ofl.Core.Linq;
using Ofl.Core.Collections.Generic;

namespace Ofl.Search
{
    public class IndexManager : Disposable, IIndexManager
    {
        #region Constructor

        public IndexManager(IEnumerable<IIndex> indexes)
        {
            // Validate parameters.
            if (indexes == null) throw new ArgumentNullException(nameof(indexes));

            // Create a dictionary.
            var ro = new Dictionary<string, IIndex>();

            // Cycle.
            foreach (IIndex index in indexes)
            {
                // Add to the disposables, add to the dictionary.
                ro.Add(index.Name, index);
                Disposables.Add(index);
            }

            // Set the ro dictionary.
            _indexes = ro.WrapInReadOnlyDictionary();
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
    }
}
