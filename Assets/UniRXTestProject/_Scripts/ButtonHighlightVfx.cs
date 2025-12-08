using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using Zenject;


[RequireComponent(typeof(AttackButtonPresenter))]
public class ButtonHighlightVfx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject highlightVfx;
    public UIParticleSystem  highlightVfxPs;
    
    
    private AttackButtonPresenter _attackButtonPresenter;
    private IDisposable _disposable;
    private GameSettingsSO _settings;
    
    private int _HighlightOnInterval;
    private int _HighlightOffInterval;
    private float _HighlightOffPressedMul;
    private float _HighlightDeltaPerInterval;
    
    
    private static readonly int _tintColor = Shader.PropertyToID("_TintColor");
    
    private Color _highlightVfxColor;
    private bool _isPointerOverButton;



    [Inject]
    private void Init(GameSettingsSO gameSettingsSo)
    {
        _settings = gameSettingsSo;
        InitSettings();
    }

    private void InitSettings()
    {
        _HighlightOnInterval = _settings.HighlightOnInterval;
        _HighlightOffInterval = _settings.HighlightOffInterval;
        _HighlightOffPressedMul = _settings.HighlightOffPressedMul;
        _HighlightDeltaPerInterval = _settings.HighlightDeltaPerInterval;
    }
    

    private void Awake()
    {
        _highlightVfxColor =  highlightVfxPs.material.GetColor(_tintColor);

        _attackButtonPresenter = GetComponent<AttackButtonPresenter>();
        _attackButtonPresenter.OnAttackDelayFinished.Subscribe(x =>
        {
            if (_isPointerOverButton) HighlightOn();
        });

        _attackButtonPresenter.OnAttackPerform.Subscribe(x =>
        {
            HighlightOff(_HighlightOffPressedMul);
        });
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
#if UNITY_EDITOR
        InitSettings(); 
#endif
        HighlightOn();
        _isPointerOverButton = true;
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
#if UNITY_EDITOR
        InitSettings();
#endif
        HighlightOff();
        _isPointerOverButton = false;
    }

    private void HighlightOn()
    {
        _disposable?.Dispose();
        highlightVfx.SetActive(true);
        
        _disposable = Observable.Interval(TimeSpan.FromMilliseconds(_HighlightOnInterval))
            .TakeWhile(x => _highlightVfxColor.a < 0.99f)
            .Subscribe(x =>
            {
                _highlightVfxColor.a = Mathf.MoveTowards
                    (_highlightVfxColor.a, 1f, _HighlightDeltaPerInterval);
                
                highlightVfxPs.material.SetColor(_tintColor, _highlightVfxColor);
            });
    }

    private void HighlightOff(float timeMul = 1f)
    {
        _disposable?.Dispose();
        
        _disposable = Observable.Interval(TimeSpan.FromMilliseconds(_HighlightOffInterval * timeMul))
            .TakeWhile(x => _highlightVfxColor.a > 0.01f)
            .Subscribe(x =>
            {
                _highlightVfxColor.a = Mathf.MoveTowards
                    (_highlightVfxColor.a, 0, _HighlightDeltaPerInterval);

                if (_highlightVfxColor.a < 0.02f)
                {
                    highlightVfx.SetActive(false);
                }

                highlightVfxPs.material.SetColor(_tintColor, _highlightVfxColor);
            });
    }

    private void OnDestroy()
    {
        _disposable?.Dispose();
        _highlightVfxColor.a = 1f;
        highlightVfxPs.material.SetColor(_tintColor, _highlightVfxColor);
    }
}