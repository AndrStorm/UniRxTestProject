using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;


public class ButtonHighlightVfx : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public GameObject highlightVfx;
    public UIParticleSystem  highlightVfxPs;
    
    private static readonly int _tintColor = Shader.PropertyToID("_TintColor");
    
    private IDisposable _disposable;
    private Color _highlightVfxColor;


    private void Awake()
    {
        _highlightVfxColor =  highlightVfxPs.material.GetColor(_tintColor);
    }
    

    public void OnPointerEnter(PointerEventData eventData)
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

    
    public void OnPointerExit(PointerEventData eventData)
    {
        _disposable?.Dispose();
        
        _disposable = Observable.Interval(TimeSpan.FromMilliseconds(30))
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
