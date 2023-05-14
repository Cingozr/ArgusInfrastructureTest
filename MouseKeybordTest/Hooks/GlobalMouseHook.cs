using MouseKeybordTest.Abstracts;
using MouseKeybordTest.Models;
using MouseKeybordTest.Win32Libs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MouseKeybordTest.Hooks
{
    public class GlobalMouseHook : GlobalHook, IObservable<MouseModel, ForegroundAppModel>
    {
        public event Action<MouseMessages, POINT> MouseEvent;

        private List<IObserver<MouseModel, ForegroundAppModel>> observers = new List<IObserver<MouseModel, ForegroundAppModel>>();
        private GlobalForegroundAppHook _globalForegroundAppHook;
        private MouseModel currentMouseAction = new MouseModel();
        private ForegroundAppModel currentForegroundAction = new ForegroundAppModel();

        public GlobalMouseHook()
        {
            _globalForegroundAppHook = new GlobalForegroundAppHook();
            _globalForegroundAppHook.ForegroundChanged += HandleForegroundChanged;
            currentForegroundAction.AppName = _globalForegroundAppHook.CurrentApp.AppName;
        }

        protected override IntPtr SetHook(User32.HookProc proc)
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            using (ProcessModule currentModule = currentProcess.MainModule)
            {
                return User32.SetWindowsHookEx((int)Win32HookConstants.WH_MOUSE_LL, proc, Kernel32.GetModuleHandle(currentModule.ModuleName), 0);
            }
        }

        protected override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MouseMessages message = (MouseMessages)wParam;
                MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                POINT point = hookStruct.pt;
                MouseEvent?.Invoke(message, point);
            }

            return User32.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private void HandleForegroundChanged(ForegroundAppModel newApp, ForegroundAppModel oldApp)
        {
            if (currentForegroundAction.AppName != newApp.AppName)
            {
                OnMouseDataChanged(currentMouseAction, oldApp);
                currentForegroundAction.AppName = newApp.AppName;
            }
        }

        private void OnMouseDataChanged(MouseModel data, ForegroundAppModel foregroundApp)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(data, foregroundApp);
            }
        }

        public IDisposable Subscribe(IObserver<MouseModel, ForegroundAppModel> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber<MouseModel, ForegroundAppModel>(observers, observer);
        }
    }

}

