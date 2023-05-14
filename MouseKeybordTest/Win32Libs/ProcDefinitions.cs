using System;

namespace MouseKeybordTest.Win32Libs
{
    public static class ProcDefinitions
    {
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}
