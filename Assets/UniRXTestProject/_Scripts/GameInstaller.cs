using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public AttackButtonPresenter _attackButtonPresenter;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AttackListener>().AsSingle();
        Container.Bind<AttackButtonPresenter>().FromInstance(_attackButtonPresenter);
    }
}