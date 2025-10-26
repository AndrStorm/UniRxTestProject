using System;
using UniRx;
using UnityEngine;

public class UniRxTestPresenter : MonoBehaviour
{
    
    public ReadOnlyReactiveProperty<int> Counter => _counter.ToReadOnlyReactiveProperty();
    private readonly ReactiveProperty<int> _counter = new();
    private readonly CompositeDisposable _disposable = new();
    
    private readonly Subject<int> _subject = new ();
    

    private void Start()
    {
        _counter.Value = 0;

        Counter
            .Where(x => x > 0)
            .Subscribe(x => Debug.Log($"Counter {x}"))
            .AddTo(_disposable);
        
        Counter
            .Where(x => x == 5)
            .Subscribe(x => _disposable.Dispose())
            .AddTo(_disposable);


        Counter
            .Where(x => x == 10)
            .Subscribe(x => _subject.OnCompleted())
            .AddTo(_disposable);
    }
    

    public void PressButton()
    {
        _counter.Value++;
    }

    private void OnDestroy()
    {
        _disposable.Dispose();
    }
}
