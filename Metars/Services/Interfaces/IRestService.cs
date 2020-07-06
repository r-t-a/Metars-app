using System;
using System.Threading;
using System.Threading.Tasks;
using Metars.Models;

namespace Metars.Services.Interfaces
{
    public interface IRestService
    {
        Task<MobileResponse<TResponseType>> Post<TResponseType>(string requestUri, CancellationToken cancellationToken);
    }
}
