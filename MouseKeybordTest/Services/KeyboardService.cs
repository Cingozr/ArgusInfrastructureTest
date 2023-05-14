using MouseKeybordTest.Models;
using MouseKeybordTest.Patterns.Observers;
using MouseKeybordTest.Win32Libs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace MouseKeybordTest.Services
{
    public class KeyboardService : IObservable<KeyboardModel, ForegroundAppModel>
    {
        private static User32.HookProc _proc;
        private static IntPtr _hookID = IntPtr.Zero;

        private static KeyboardModel currentApp = new KeyboardModel();
        private static StringBuilder keysBuffer = new StringBuilder();
        private static readonly object lockObj = new object();
        private ForegroundAppWatcher _foregroundAppWatcher;

        private List<IObserver<KeyboardModel, ForegroundAppModel>> observers = new List<IObserver<KeyboardModel, ForegroundAppModel>>();

        public KeyboardService()
        {
            _proc = HookCallback;
            _foregroundAppWatcher = new ForegroundAppWatcher();
            _foregroundAppWatcher.ForegroundChanged += HandleForegroundChanged;

            currentApp.AppName = _foregroundAppWatcher.CurrentApp.AppName;
        }

        public void Start()
        {
            _hookID = SetHook(_proc);
            Application.Run();
            User32.UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(User32.HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return User32.SetWindowsHookEx(Win32HookConstants.WH_KEYBOARD_LL, proc,
                    Kernel32.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private void HandleForegroundChanged(ForegroundAppModel newApp, ForegroundAppModel oldApp)
        {
            lock (lockObj)
            {
                if (currentApp.AppName != newApp.AppName)
                {
                    currentApp.Keys = keysBuffer.ToString();
                    OnAppDataChanged(currentApp, oldApp);

                    keysBuffer.Clear();
                    currentApp.AppName = newApp.AppName;
                }

            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WindowsMessages.WM.KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                lock (lockObj)
                {
                    keysBuffer.Append((Keys)vkCode);
                }
            }

            return User32.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private void OnAppDataChanged(KeyboardModel data, ForegroundAppModel foregroundApp)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(data, foregroundApp);
            }
        }

        public IDisposable Subscribe(IObserver<KeyboardModel, ForegroundAppModel> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber<KeyboardModel, ForegroundAppModel>(observers, observer);
        }
    }
}
