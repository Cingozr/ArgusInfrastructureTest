using MouseKeybordTest.Win32Libs;
using System;
using System.Windows.Forms;

namespace MouseKeybordTest.Abstracts
{
    public abstract class GlobalHook : IDisposable
    {
        protected User32.HookProc _proc;
        protected IntPtr _hookID = IntPtr.Zero;

        public GlobalHook()
        {
            _proc = HookProcedure;
        }

        protected abstract IntPtr SetHook(User32.HookProc proc);

        protected abstract IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam);

        public void Start()
        {
            _hookID = SetHook(_proc);
            Application.Run();
            User32.UnhookWindowsHookEx(_hookID);
        }

        public void Dispose()
        {
            User32.UnhookWindowsHookEx(_hookID);
        }
    }

}
