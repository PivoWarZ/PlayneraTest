using PlayneraTest.Code.Scripts.Blushers;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Pomades;
using Zenject;

namespace PlayneraTest.Code.Scripts.DI
{
    public class ZenjectContext: MonoInstaller
    {
        public override void InstallBindings()
        {
            HandService handService = new HandService();
            Container.Bind<IHandService>().FromInstance(handService);
            
            Container.BindInterfacesAndSelfTo<BlushersViewModel>()
                .AsCached();
            
            Container.BindInterfacesAndSelfTo<BlushersModel>().AsCached();
            Container.BindInterfacesAndSelfTo<HandSpeedHandler>().AsCached().NonLazy();
            Container.BindInterfacesAndSelfTo<PomadesViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<PomadesModel>().AsCached();
        }
    }
}