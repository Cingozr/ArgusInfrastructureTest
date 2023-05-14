using System.Collections.Generic;
using System;

public class Unsubscriber<T1> : IDisposable
{
    private List<IObserver<T1>> _observers;
    private IObserver<T1> _observer;

    public Unsubscriber(List<IObserver<T1>> observers, IObserver<T1> observer)
    {
        this._observers = observers;
        this._observer = observer;
    }

    public void Dispose()
    {
        if (_observer != null && _observers.Contains(_observer))
            _observers.Remove(_observer);
    }
}

public class Unsubscriber<T1, T2> : IDisposable
{
    private List<IObserver<T1, T2>> _observers;
    private IObserver<T1, T2> _observer;

    public Unsubscriber(List<IObserver<T1, T2>> observers, IObserver<T1, T2> observer)
    {
        this._observers = observers;
        this._observer = observer;
    }

    public void Dispose()
    {
        if (_observer != null && _observers.Contains(_observer))
            _observers.Remove(_observer);
    }
} 