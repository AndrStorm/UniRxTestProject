using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AttackButtonPresenter : MonoBehaviour
{
    
    public IObservable<AttackTypes> OnAttackPerform => _onAttackPerform;
    private readonly Subject<AttackTypes> _onAttackPerform = new();

    public IObservable<int> OnAttackPressed => _onAttackPressed;
    private readonly Subject<int> _onAttackPressed = new();
    
    public IObservable<int> OnAttackDelayFinished => _onAttackDelayFinished;
    private readonly Subject<int> _onAttackDelayFinished = new();
    
    private readonly ReactiveProperty<int> _clickCounter = new();
    private readonly CompositeDisposable _disposable = new();

    
    private Button _button;

    public float attackDelayLight = 500f;
    public float attackDelayHeavy = 1000f;
    public float doubleClickDelay = 250f;
    
    
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        
        _onAttackPerform
            .Where(x => x == AttackTypes.light)
            .Throttle(TimeSpan.FromMilliseconds(attackDelayLight))
            .Subscribe(x =>
            {
                _onAttackDelayFinished.OnNext(1);
                _button.interactable = true;
            })
            .AddTo(_disposable);
        
        _onAttackPerform
            .Where(x => x == AttackTypes.heavy)
            .Throttle(TimeSpan.FromMilliseconds(attackDelayHeavy))
            .Subscribe(x =>
            {
                _onAttackDelayFinished.OnNext(1);
                _button.interactable = true;
            })
            .AddTo(_disposable);
        
        
        
        _clickCounter
            .Where(x => x == 2)
            .ThrottleFirst(TimeSpan.FromMilliseconds(attackDelayHeavy))
            .Subscribe(x =>
            {
                _clickCounter.Value = 0;
                _onAttackPerform.OnNext(AttackTypes.heavy);
                _button.interactable = false;
            })
            .AddTo(_disposable);
        
        
        _clickCounter
            .Where(x => x > 0)
            .Buffer(_clickCounter.Where(x => x > 0).Throttle(TimeSpan.FromMilliseconds(doubleClickDelay)))
            .Where(x => x.Count < 2)
            .ThrottleFirst(TimeSpan.FromMilliseconds(attackDelayLight))
            .Subscribe(x =>
            {
                _clickCounter.Value = 0;
                _onAttackPerform.OnNext(AttackTypes.light);
                _button.interactable = false;
            })
            .AddTo(_disposable);
    }
    

    public void OnClick()
    {
        _clickCounter.Value++;
        _onAttackPressed.OnNext(1);
    }

    private void OnDestroy()
    {
        _disposable.Dispose();
    }
}

public enum AttackTypes
{
    light,
    heavy
}

