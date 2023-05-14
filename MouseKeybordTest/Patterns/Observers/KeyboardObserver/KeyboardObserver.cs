using MouseKeybordTest.Models;
using System;

namespace MouseKeybordTest.Patterns.Observers.KeyboardObserver
{
    public class KeyboardObserver : IObserver<KeyboardModel, ForegroundAppModel>
    {
        private IDisposable unsubscriber;

        public virtual void Subscribe(IObservable<KeyboardModel, ForegroundAppModel> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        public virtual void OnCompleted()
        {
            this.Unsubscribe();
        }

        public virtual void OnError(Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }

        public virtual void OnNext(KeyboardModel data, ForegroundAppModel foregroundApp)
        {
            Console.WriteLine($"KeyboardModel notification: App: {data.AppName} | Keys: {data.Keys}");
            Console.WriteLine($"ForegroundAppModel notification: App: {foregroundApp.AppName} StartDate: {foregroundApp.StartDate} EndDate: {foregroundApp.EndDate} TotalActiveTime: {foregroundApp.TotalActiveTime.TotalSeconds}");
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
    }
}
