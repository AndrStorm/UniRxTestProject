using UnityEngine;
using Zenject;

public class UniRxTestInstaller : MonoInstaller
{

    public UniRxTestPresenter _presenter;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UniRxTestListener>().AsSingle();
        Container.Bind<UniRxTestPresenter>().FromInstance(_presenter);
    }
}