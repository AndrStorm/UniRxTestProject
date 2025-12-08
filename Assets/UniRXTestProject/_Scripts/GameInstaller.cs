using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public GameSettingsSO _gameSettingsSO;

    public AttackButtonPresenter _attackButtonPresenter;
    public VfxManager _vfxManager;
    public Player _player;

    public override void InstallBindings()
    {
        Container.Bind<GameSettingsSO>().FromInstance(_gameSettingsSO);
        
        Container.BindInterfacesAndSelfTo<AttackListener>().AsSingle();
        
        Container.Bind<AttackButtonPresenter>().FromInstance(_attackButtonPresenter);
        Container.Bind<VfxManager>().FromInstance(_vfxManager);
        Container.Bind<Player>().FromInstance(_player);
    }
}