using System.Threading;
using System.Threading.Tasks;

namespace ApiDemo.Api.Common
{
    public interface IRequestHandler<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken token);
    }
}
