using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Search
{
    internal interface IMultiIndexGetOperation
    {
        Task<GetResponse<object>> GetAsync(IEnumerable<object> ids, CancellationToken cancellationToken);
    }
}
