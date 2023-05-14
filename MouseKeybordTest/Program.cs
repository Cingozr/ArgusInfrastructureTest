using MouseKeybordTest.Patterns.Observers.KeyboardObserver;
using MouseKeybordTest.Services;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

public class Program
{

    public static void Main()
    {
        KeyboardObserver observer = new KeyboardObserver();
        KeyboardService keyboardService = new KeyboardService();

        observer.Subscribe(keyboardService);
        keyboardService.Start();
    }
}
