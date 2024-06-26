using Zenject;

namespace CGames
{
    public class SystemsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SavingSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AudioSystem>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<BonusSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameStatusHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LocalizationSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LeaderboardSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerPreferences>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<AdvertisementHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CollectionsInventory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<Wallet>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GridSnapshot>().AsSingle().NonLazy();
        }
    }
}