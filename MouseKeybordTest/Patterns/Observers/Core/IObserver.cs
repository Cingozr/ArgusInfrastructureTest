using System;

public interface IObserver<T1>
{
    void OnNext(T1 value1);
    void OnError(Exception error);
    void OnCompleted();
}

public interface IObserver<T1, T2>
{
    void OnNext(T1 value1, T2 value2);
    void OnError(Exception error);
    void OnCompleted();
}



public interface IObservable<T1>
{
    IDisposable Subscribe(IObserver<T1> observer);
}

public interface IObservable<T1, T2>
{
    IDisposable Subscribe(IObserver<T1, T2> observer);
}