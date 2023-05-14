using System;
using System.Runtime.InteropServices;

namespace MouseKeybordTest.Models
{
    public class MouseEventModel
    {
        public bool IsActive { get; set; }
        public MouseMessages MouseMessage { get; set; }
        public Point Position { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    public enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
    }

}
