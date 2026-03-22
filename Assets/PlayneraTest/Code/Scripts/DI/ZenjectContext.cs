using PlayneraTest.Code.Scripts.Blushers;
using Zenject;

namespace PlayneraTest.Code.Scripts.DI
{
    public class ZenjectContext: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BlushersViewModel>()
                .AsCached();
        }
    }
}