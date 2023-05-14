using MouseKeybordTest.Abstracts;
using MouseKeybordTest.Models;
using MouseKeybordTest.Win32Libs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MouseKeybordTest.Hooks
{
    public class GlobalKeyboardHook : GlobalHook, IObservable<KeyboardModel, ForegroundAppModel>
    {
        private const int WM_KEYDOWN = 0x0100;
        private static StringBuilder keysBuffer = new StringBuilder();
        private static readonly object lockObj = new object();
        private GlobalForegroundAppHook _globalForegroundAppHook;
        private KeyboardModel currentApp = new KeyboardModel(); 

        private List<IObserver<KeyboardModel, ForegroundAppModel>> observers = new List<IObserver<KeyboardModel, ForegroundAppModel>>();

        public GlobalKeyboardHook()
        {
            _proc = HookProcedure;
            _globalForegroundAppHook = new GlobalForegroundAppHook();
            _globalForegroundAppHook.ForegroundChanged += HandleForegroundChanged;

            currentApp.AppName = _globalForegroundAppHook.CurrentApp.AppName;
        }

        protected override IntPtr SetHook(User32.HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return User32.SetWindowsHookEx(Win32HookConstants.WH_KEYBOARD_LL, _proc,
                    Kernel32.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        protected override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                lock (lockObj)
                {
                    keysBuffer.Append((Keys)vkCode);
                }
            }

            return User32.CallNextHookEx(_hookID, nCode, wParam, lParam);
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
