using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class AttackButtonPresenter : MonoBehaviour
{
    public IObservable<AttackTypes> OnAttackPerform => _onAttackPerform;
    public IObservable<int> OnAttackPressed => _onAttackPressed;
    public IObservable<int> OnAttackDelayFinished => _onAttackDelayFinished;
    
    private readonly Subject<AttackTypes> _onAttackPerform = new();
    private readonly Subject<int> _onAttackPressed = new();
    private readonly Subject<int> _onAttackDelayFinished = new();
    
    private readonly ReactiveProperty<int> _clickCounter = new();
    private CompositeDisposable _disposable = new();

    
    private Button _button;
    private GameSettingsSO _settings;
    
    private float _AttackDelayLight;
    private float _AttackDelayHeavy;
    private float _DoubleClickDelay;
    
    
    
    [Inject]
    public void Init(GameSettingsSO gameSettings)
    {
        _settings = gameSettings;
    }
    
    private void OnDestroy()
    {
        _disposable.Dispose();
    }
    
    
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);

        Init();
    }

    private void Init()
    {
        _disposable?.Dispose();
        _disposable = new CompositeDisposable();
        
        _AttackDelayLight = _settings.AttackDelayLight;
        _AttackDelayHeavy = _settings.AttackDelayHeavy;
        _DoubleClickDelay = _settings.DoubleClickDelay;

#if UNITY_EDITOR
        ReinitSettings();
#endif
        
        _onAttackPerform
            .Where(x => x == AttackTypes.light)
            .Throttle(TimeSpan.FromMilliseconds(_AttackDelayLight))
            .Subscribe(x => { FinishAttackDelay(); })
            .AddTo(_disposable);
        
        _onAttackPerform
            .Where(x => x == AttackTypes.heavy)
            .Throttle(TimeSpan.FromMilliseconds(_AttackDelayHeavy))
            .Subscribe(x => { FinishAttackDelay(); })
            .AddTo(_disposable);
        
        
        _clickCounter
            .Where(x => x == 2)
            .ThrottleFirst(TimeSpan.FromMilliseconds(_AttackDelayHeavy))
            .Subscribe(x =>
            {
                _clickCounter.Value = 0;
                _onAttackPerform.OnNext(AttackTypes.heavy);
                _button.interactable = false;
            })
            .AddTo(_disposable);
        
        
        _clickCounter
            .Where(x => x > 0)
            .Buffer(_clickCounter.Where(x => x > 0).Throttle(TimeSpan.FromMilliseconds(_DoubleClickDelay)))
            .Where(x => x.Count < 2)
            .ThrottleFirst(TimeSpan.FromMilliseconds(_AttackDelayLight))
            .Subscribe(x =>
            {
                _clickCounter.Value = 0;
                _onAttackPerform.OnNext(AttackTypes.light);
                _button.interactable = false;
            })
            .AddTo(_disposable); 
    }

    private void FinishAttackDelay()
    {
        _onAttackDelayFinished.OnNext(1);
        _button.interactable = true;
    }

    private void ReinitSettings()
    {
        _onAttackPerform
            .Where(x => x == AttackTypes.light)
            .Throttle(TimeSpan.FromMilliseconds(_AttackDelayLight + 1))
            .Subscribe(x =>
            {
                Init();
            })
            .AddTo(_disposable);
        
        _onAttackPerform
            .Where(x => x == AttackTypes.heavy)
            .Throttle(TimeSpan.FromMilliseconds(_AttackDelayHeavy + 1))
            .Subscribe(x =>
            {
                Init();
            })
            .AddTo(_disposable);
    }
    

    public void OnClick()
    {
        _clickCounter.Value++;
        _onAttackPressed.OnNext(1);
    }

    
}

public enum AttackTypes
{
    light,
    heavy
}

