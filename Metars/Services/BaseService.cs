using System;
using System.Threading;
using Metars.Services.Interfaces;

namespace Metars.Services
{
    public class BaseService : IBaseService
    {
        private static CancellationTokenSource _cts = new CancellationTokenSource();

        protected static CancellationToken Token => _cts.Token;

        public void CancelAllTasks()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}
