using System;
using UniRx;
using UnityEngine;
using Zenject;

public class AttackListener : IInitializable, IDisposable
{
    private readonly AttackButtonPresenter _attackButtonPresenter;
    private readonly VfxManager _vfxManager;
    private readonly Player _player;
    
    private CompositeDisposable _disposable;
    private GameSettingsSO _settings;
    
    private static readonly int IsLightAttack = Animator.StringToHash("IsLightAttack");
    private static readonly int IsHeavyAttack = Animator.StringToHash("IsHeavyAttack");

    private int _LightAttackVfxDelay;
    private int _HeavyAttackVfxDelay;
    
    [Inject]
    public AttackListener(GameSettingsSO gameSettingsSo, AttackButtonPresenter attackButtonPresenter,
        VfxManager vfxManager, Player player)
    {
        _settings = gameSettingsSo;
        _attackButtonPresenter = attackButtonPresenter;
        _vfxManager = vfxManager;
        _player = player;
    }
    
    public void Initialize()
    {
        _disposable?.Dispose();
        _disposable = new CompositeDisposable();
        _LightAttackVfxDelay = _settings.LightAttackVfxDelay;
        _HeavyAttackVfxDelay = _settings.HeavyAttackVfxDelay;

#if UNITY_EDITOR
        ReinitSettings();
#endif
        
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.light)
            .Subscribe(x =>
            {
                _player.Animator.SetBool(IsLightAttack, true);
            })
            .AddTo(_disposable);
        
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.light)
            .Throttle(TimeSpan.FromMilliseconds(_LightAttackVfxDelay))
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
            .Throttle(TimeSpan.FromMilliseconds(_HeavyAttackVfxDelay))
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
    
    private void ReinitSettings()
    {
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.light)
            .Throttle(TimeSpan.FromMilliseconds(_LightAttackVfxDelay + 1))
            .Subscribe(x =>
            {
                Initialize();
            })
            .AddTo(_disposable);
        
        _attackButtonPresenter.OnAttackPerform
            .Where(x => x == AttackTypes.heavy)
            .Throttle(TimeSpan.FromMilliseconds(_HeavyAttackVfxDelay + 1))
            .Subscribe(x =>
            {
                Initialize();
            })
            .AddTo(_disposable);
        
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
