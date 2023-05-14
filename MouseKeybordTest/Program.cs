using MouseKeybordTest.Hooks;
using MouseKeybordTest.Patterns.Observers.KeyboardObserver;
using MouseKeybordTest.Patterns.Observers.MouseObservers;
using System;
using System.Threading.Tasks;

public class Program
{

    public static async Task Main()
    {

        var globalMouseHook = new GlobalMouseHook();
        var globalKeyboardHook = new GlobalKeyboardHook();
        // Optional: You can subscribe to the services to get updates
        var mouseObserver = new MouseObserver();
        globalMouseHook.Subscribe(mouseObserver);

        var keyboardObserver = new KeyboardObserver();
        globalKeyboardHook.Subscribe(keyboardObserver);


        globalKeyboardHook.Start();
        globalMouseHook.Start();
    }
}
