using PlayneraTest.Code.Scripts.Blushers;
using PlayneraTest.Code.Scripts.Hand;
using Zenject;

namespace PlayneraTest.Code.Scripts.DI
{
    public class ZenjectContext: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BlushersViewModel>()
                .AsCached();
            
            Container.BindInterfacesAndSelfTo<HandService>().AsCached();
        }
    }
}