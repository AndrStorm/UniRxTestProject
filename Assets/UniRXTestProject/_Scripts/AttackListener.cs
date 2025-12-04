using System;
using UniRx;
using UnityEngine;
using Zenject;

public class AttackListener : IInitializable, IDisposable
{
    private readonly AttackButtonPresenter _attackButtonPresenter;
    private readonly VfxManager _vfxManager;
    private readonly Player _player;
    
    private readonly CompositeDisposable _disposable = new ();
    
    private static readonly int IsLightAttack = Animator.StringToHash("IsLightAttack");
    private static readonly int IsHeavyAttack = Animator.StringToHash("IsHeavyAttack");
    
    
    [Inject]
    public AttackListener(AttackButtonPresenter attackButtonPresenter, VfxManager vfxManager, Player player)
    {
        _attackButtonPresenter = attackButtonPresenter;
        _vfxManager = vfxManager;
        _player = player;
    }
    
    public void Initialize()
    {
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.light)
            .Subscribe(x =>
            {
                _player.Animator.SetBool(IsLightAttack, true);
            })
            .AddTo(_disposable);
        
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.light)
            .Throttle(TimeSpan.FromMilliseconds(200))
            .Subscribe(x =>
            {
                _vfxManager.PlayLightAttackVfx();
                _player.Animator.SetBool(IsLightAttack, false);
            })
            .AddTo(_disposable);
        
        
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.heavy)
            .Subscribe(x =>
            {
                _player.Animator.SetBool(IsHeavyAttack, true);
            })
            .AddTo(_disposable);
        
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.heavy)
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Subscribe(x =>
            {
                _vfxManager.PlayHeavyAttackVfx();
                _player.Animator.SetBool(IsHeavyAttack, false);
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
