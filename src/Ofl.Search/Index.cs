﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Search
{
    public abstract class Index : IIndex
    {
        #region Constructor

        protected Index(string name, Type type)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));

            // Assign values.
            Name = name;
        }

        #endregion

        #region Implementation of IIndex

        public string Name { get; }

        public Type Type { get; }

        public abstract Task<bool> ExistsAsync(CancellationToken cancellationToken);

        public abstract Task CreateAsync(CancellationToken cancellationToken);

        public abstract Task DestroyAsync(CancellationToken cancellationToken);

        public abstract Task<IndexStats> GetStatsAsync(CancellationToken cancellationToken);

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            // Call the overload, suppress finalization.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Dispose of unmanaged resources.

            // If not disposing, get out.
            if (!disposing) return;

            // Dispose IDisposable implementations.
        }

        ~Index()
        {
            // Call Dispose.
            Dispose(false);
        }

        #endregion
    }
}
