using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;


[RequireComponent(typeof(AttackButtonPresenter))]
public class ButtonHighlightVfx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject highlightVfx;
    public UIParticleSystem  highlightVfxPs;
    
    private static readonly int _tintColor = Shader.PropertyToID("_TintColor");

    private AttackButtonPresenter _attackButtonPresenter;
    private IDisposable _disposable;
    private Color _highlightVfxColor;

    private bool _isPointerOverButton;


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
            HighlightOff(0.5f);
        });
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightOn();
        _isPointerOverButton = true;
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        HighlightOff();
        _isPointerOverButton = false;
    }

    private void HighlightOn()
    {
        _disposable?.Dispose();
        highlightVfx.SetActive(true);
        
        _disposable = Observable.Interval(TimeSpan.FromMilliseconds(20))
            .TakeWhile(x => _highlightVfxColor.a < 0.99f)
            .Subscribe(x =>
            {
                _highlightVfxColor.a = Mathf.MoveTowards
                    (_highlightVfxColor.a, 1f, 0.05f);
                
                highlightVfxPs.material.SetColor(_tintColor, _highlightVfxColor);
            });
    }

    private void HighlightOff(float timeMul = 1f)
    {
        _disposable?.Dispose();
        
        _disposable = Observable.Interval(TimeSpan.FromMilliseconds(30 * timeMul))
            .TakeWhile(x => _highlightVfxColor.a > 0.01f)
            .Subscribe(x =>
            {
                _highlightVfxColor.a = Mathf.MoveTowards
                    (_highlightVfxColor.a, 0, 0.05f);

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