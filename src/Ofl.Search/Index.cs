using System;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Core;

namespace Ofl.Search
{
    public abstract class Index : Disposable, IIndex
    {
        #region Constructor

        protected Index(string name)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            // Assign values.
            Name = name;
        }

        #endregion

        #region Implementation of IIndex

        public string Name { get; }

        public virtual Task RegenerateAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task CreateAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        public virtual Task DestroyAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IIndexWriteOperations<T>> GetWriteOperationsAsync<T>(CancellationToken cancellationToken)
            where T : class
        {
            throw new NotImplementedException();
        }

        public virtual Task<IIndexReadOperations<T>> GetReadOperationsAsync<T>(CancellationToken cancellationToken)
            where T : class
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
