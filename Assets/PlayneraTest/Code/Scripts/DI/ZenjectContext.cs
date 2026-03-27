using PlayneraTest.Code.Scripts.Blushers;
using PlayneraTest.Code.Scripts.Hand;
using UnityEngine;
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
        }
    }
}