using System;
using UniRx;
using UnityEngine;
using Zenject;

public class AttackListener : IInitializable, IDisposable
{
    
    private readonly AttackButtonPresenter _attackButtonPresenter;
    private readonly CompositeDisposable _disposable = new ();
    
    
    [Inject]
    public AttackListener(AttackButtonPresenter attackButtonPresenter)
    {
        _attackButtonPresenter = attackButtonPresenter;
    }
    
    public void Initialize()
    {
        //срабатывание эффектов
        _attackButtonPresenter.OnAttackPerform
            .Subscribe(x => Debug.Log($"attack type {x}"))
            .AddTo(_disposable);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
