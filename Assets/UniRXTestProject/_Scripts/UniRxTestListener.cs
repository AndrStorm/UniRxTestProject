using System;
using UnityEngine;
using Zenject;
using UniRx;

public class UniRxTestListener: IInitializable, IDisposable
{
    //observable from update

    private readonly UniRxTestPresenter _presenter;
    private readonly CompositeDisposable _disposable = new();
    
    
    [Inject]
    public UniRxTestListener(UniRxTestPresenter presenter)
    {
        _presenter = presenter;
    }
    
    public void Initialize()
    {
        _presenter.Counter
            .Where(x => x > 0)
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(x => Debug.Log($"Listener {x}"))
            .AddTo(_disposable);

        Observable
            .EveryUpdate()
            .Where(x => x > 0)
            .Where(x => x % 300 == 0)
            .Select(x => x / 300)
            .Subscribe(Update)
            .AddTo(_disposable);
    }
    

    private void Update(long x)
    {
        Debug.Log($"Each 300 Frame Counter - {x}");
    }
    
    
    

    public void Dispose()
    {
        _disposable.Dispose();
    }

    
}
