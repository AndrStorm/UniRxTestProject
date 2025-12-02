using System;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class AttackListener : IInitializable, IDisposable
{
    
    private readonly AttackButtonPresenter _attackButtonPresenter;
    private readonly VfxManager _vfxManager;
    
    private readonly CompositeDisposable _disposable = new ();
    
    
    [Inject]
    public AttackListener(AttackButtonPresenter attackButtonPresenter, VfxManager vfxManager)
    {
        _attackButtonPresenter = attackButtonPresenter;
        _vfxManager = vfxManager;
    }
    
    public void Initialize()
    {
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.light)
            .Subscribe(x =>
            {
                _vfxManager.PlayLightAttackVfx(Vector3.zero, Quaternion.identity);
            })
            .AddTo(_disposable);
        
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.heavy)
            .Subscribe(x =>
            {
                _vfxManager.PlayHeavyAttackVfx(Vector3.zero, Quaternion.identity);
            })
            .AddTo(_disposable);
        
        _attackButtonPresenter.OnAttackPressed
            .Subscribe(x =>
            {
                _vfxManager.PlayAttackPressedVfx();
            })
            .AddTo(_disposable);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
