using UnityEngine;
using Zenject;


namespace CGames
{
    public class ResourcesInstaller : MonoInstaller
    {
        [SerializeField] private MonoProxy monoProxy;

        public override void InstallBindings()
        {
            Container.Bind<MonoProxy>().FromInstance(monoProxy).AsSingle().NonLazy();
        
            Container.BindInterfacesAndSelfTo<ResourceSystem>().AsSingle().NonLazy();
            Container.Bind<ShapesFactory>().AsSingle().NonLazy();
        }
    }
}