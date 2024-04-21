
using Zenject;

namespace App.Core.Common.Services
{
    public class DIInstaller : MonoInstaller
    {
        public static DiContainer GlobalContainer { get; private set; }

        public override void InstallBindings()
        {
            GlobalContainer = Container;
        }
    }
}
