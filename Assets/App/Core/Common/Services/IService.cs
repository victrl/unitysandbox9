
using Cysharp.Threading.Tasks;

namespace App.Core.Common.Services
{
    public interface IService
    {
        void Inject();
        void OnRegister();
        void SetContext(AppContext.AppContext context);
        UniTaskVoid PrepareService();
        void OnUnregister();
        bool IsInitialized { get; }
    }
}