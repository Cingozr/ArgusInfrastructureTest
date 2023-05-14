using MouseKeybordTest.Models;
using System;

namespace MouseKeybordTest.Patterns.Observers.MouseObservers
{
    public class MouseObserver : IObserver<MouseModel, ForegroundAppModel>
    {
        private IDisposable _unsubscriber;

        public virtual void Subscribe(IObservable<MouseModel, ForegroundAppModel> provider)
        {
            if (provider != null)
                _unsubscriber = provider.Subscribe(this);
        }

        public virtual void OnCompleted()
        {
            this.Unsubscribe();
        }

        public virtual void OnError(Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }

        public virtual void OnNext(MouseModel mouseModel, ForegroundAppModel appModel)
        {
            Console.WriteLine($"Mouse activity: {mouseModel.MouseMessages} at position {mouseModel.Position}. Active: {appModel.AppName}");
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
    }

}
