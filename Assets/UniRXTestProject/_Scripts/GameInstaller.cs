using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public AttackButtonPresenter _attackButtonPresenter;
    public VfxManager _vfxManager;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AttackListener>().AsSingle();
        Container.Bind<AttackButtonPresenter>().FromInstance(_attackButtonPresenter);
        Container.Bind<VfxManager>().FromInstance(_vfxManager);
        
    }
}