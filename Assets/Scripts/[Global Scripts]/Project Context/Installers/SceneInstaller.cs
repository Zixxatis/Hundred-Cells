using UnityEngine;
using Zenject;

namespace CGames
{
    public class SceneInstaller : MonoInstaller
    {
        [Header("In-Game Objects")]
        [SerializeField] private ScoreCounter scoreCounter;
        [SerializeField] private GameplayGrid gameplayGrid;
        [SerializeField] private GridMediator gridMediator;
        [SerializeField] private GridModifier gridModifier;
        [SerializeField] private PlayerHand playerHand;
        [Space]
        [SerializeField] private MainMenu mainMenu;

        [Header("Canvases")]
        [SerializeField] private AdvertisementCanvas advertisementCanvas;
        [SerializeField] private DragCanvas dragCanvas;

        [Header("Various")]
        [SerializeField] private AudioPlayer audioPlayer;
        [SerializeField] private StoreElementsFactory storeElementsFactory;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScoreCounter>().FromInstance(scoreCounter).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameplayGrid>().FromInstance(gameplayGrid).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GridMediator>().FromInstance(gridMediator).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GridModifier>().FromInstance(gridModifier).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerHand>().FromInstance(playerHand).AsSingle().NonLazy(); 
            Container.BindInterfacesTo<MainMenu>().FromInstance(mainMenu).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<AdvertisementCanvas>().FromInstance(advertisementCanvas).AsSingle().NonLazy();
            Container.BindInstance(dragCanvas).AsSingle().NonLazy();

            Container.BindInstance(audioPlayer).AsSingle().NonLazy();
            Container.BindInstance(storeElementsFactory).AsSingle().NonLazy();
        }
    }
}