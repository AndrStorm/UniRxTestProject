using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class UniRxTestPresenter : MonoBehaviour
{
    public ReadOnlyReactiveProperty<int> Counter => _counter.ToReadOnlyReactiveProperty();
    private readonly ReactiveProperty<int> _counter = new();

    public IObservable<int> SubjectObservable => _subject;
    private readonly Subject<int> _subject = new ();

    private readonly CompositeDisposable _disposable = new();

    
    private void Start()
    {
        _counter.Value = 0;
    

        Counter
            .Where(x => x > 0)
            .Subscribe(x => Debug.Log($"Counter {x}"))
            .AddTo(_disposable);
        
        Counter
            .Where(x => x > 15)
            .Subscribe(x => _disposable.Dispose())
            .AddTo(_disposable);


        Counter
            .Where(x => x == 21)
            .Subscribe(x => _subject.OnCompleted())
            .AddTo(_disposable);
        
        
        Counter.Subscribe(_subject);
        
        
        
        //_counter.Value += GenerateAndTakeSecond();
        //_counter.Value += GenerateAndTakeSecond2();
        

        GenerateNumbersAsObservable()
            .Skip(1)
            .First()   
            .Subscribe(onNext: value => _counter.Value += value,
                onCompleted: () => Debug.Log("Генерация завершена"))
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
    
    
    
    private int GenerateAndTakeSecond()
    {
        var numbers = GenerateNumbers();
        int count = 0;

        while (numbers.MoveNext())
        {
            if (++count == 2)
            {
                return (int)numbers.Current;
            }
        }
        return 0;
    }
    
    private int GenerateAndTakeSecond2()
    {
        return GenerateNumbers2().Skip(1).Take(1).FirstOrDefault();
    }
    
    
    
    private IObservable<int> GenerateNumbersAsObservable()
    {
        return Observable.FromCoroutine<int>(Generate);
    }

    private IEnumerator Generate(IObserver<int> observer)
    {
        Debug.Log("Генерируем первое число...");
        yield return new WaitForSeconds(1f);
        observer.OnNext(1);

        Debug.Log("Генерируем второе число...");
        yield return new WaitForSeconds(1f);
        observer.OnNext(8);

        Debug.Log("Генерируем третье число...");
        yield return new WaitForSeconds(1f);
        observer.OnNext(2);

        observer.OnCompleted();
    }


    private IEnumerator GenerateNumbers()
    {
        yield return 2;
        yield return 5;
        yield return 2;
        yield return 1;
        yield return 7;
    }
    
    private IEnumerable<int> GenerateNumbers2()
    {
        yield return 2;
        yield return 5;
        yield return 2;
        yield return 1;
        yield return 7;
    }
}
