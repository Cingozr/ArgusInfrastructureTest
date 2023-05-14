using MouseKeybordTest.Enums;
using MouseKeybordTest.Models;
using MouseKeybordTest.Win32Libs;
using System.Diagnostics;
using System;

internal class GlobalForegroundAppHook : IDisposable
{
    private IntPtr _winEventHook;
    private User32.WinEventDelegate _foreProc;
    public event Action<ForegroundAppModel, ForegroundAppModel> ForegroundChanged;
    private ForegroundAppModel tempCurrentApp;
    private ForegroundAppModel previousApp;

    public ForegroundAppModel CurrentApp => tempCurrentApp;

    public GlobalForegroundAppHook()
    {
        tempCurrentApp = new ForegroundAppModel
        {
            AppName = GetActiveAppName(User32.GetForegroundWindow(), HookType.NoneHooke),
            StartDate = DateTime.Now
        };

        _foreProc = ForegroundAppChanged;
        _winEventHook = User32.SetWinEventHook(User32.EVENT_SYSTEM_FOREGROUND, User32.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, _foreProc, 0, 0, User32.WINEVENT_OUTOFCONTEXT);
    }

    public void Dispose()
    {
        User32.UnhookWinEvent(_winEventHook);
    }

    private void ForegroundAppChanged(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
        string activeApp = GetActiveAppName(hwnd, HookType.Hook);

        if (tempCurrentApp != null && tempCurrentApp.AppName != activeApp)
        {
            // Set the end date for the previous application
            tempCurrentApp.EndDate = DateTime.Now;
            previousApp = tempCurrentApp;
        }

        // Create a new AppData instance for the current application
        tempCurrentApp = new ForegroundAppModel
        {
            AppName = activeApp,
            StartDate = DateTime.Now
        };

        ForegroundChanged?.Invoke(tempCurrentApp, previousApp);
    }

    private static string GetActiveAppName(IntPtr hwnd, HookType hookType)
    {
        uint processId;
        if (hookType == HookType.NoneHooke)
            hwnd = User32.GetForegroundWindow();

        User32.GetWindowThreadProcessId(hwnd, out processId);
        Process process = Process.GetProcessById((int)processId);
        return process.ProcessName;
    }
}
