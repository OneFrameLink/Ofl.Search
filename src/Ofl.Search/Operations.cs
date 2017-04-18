using System;
using Ofl.Core;

namespace Ofl.Search
{
    public class Operations<TIndex> : Disposable
        where TIndex : IIndex
    {
        #region Constructor

        protected Operations(TIndex index)
        {
            // Validate parameters.
            if (index == null) throw new ArgumentNullException(nameof(index));

            // Assign values.
            Index = index;
        }

        #endregion

        #region Instance, read-only state.

        protected TIndex Index { get; }

        #endregion
    }
}
