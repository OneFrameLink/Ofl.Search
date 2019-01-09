using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Search
{
    public abstract class Index<T> : Index, IIndex<T>
        where T : class
    {
        #region Constructor

        protected Index(string name) : base(name)
        { }

        #endregion

        #region Implementation of IIndex

        public virtual Task<IIndexWriteOperations<T>> GetWriteOperationsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IIndexReadOperations<T>> GetReadOperationsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
