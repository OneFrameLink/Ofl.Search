using System;

namespace Ofl.Search
{
    public class Operations<TIndex>
        where TIndex : IIndex
    {
        #region Constructor

        protected Operations(TIndex index)
        {
            // Validate parameters.
            if (index == null)
                throw new ArgumentNullException(nameof(index));

            // Assign values.
            Index = index;
        }

        #endregion

        #region Instance, read-only state.

        protected TIndex Index { get; }

        #endregion

        #region IDisposable implementation.

        public void Dispose()
        {
            // Call overload, suppress finalization.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Dispose of unmanaged resources.

            // If not disposing, get out.
            if (!disposing) return;

            // Dispose of the index.

        }

        ~Operations() => Dispose(false);

        #endregion
    }
}