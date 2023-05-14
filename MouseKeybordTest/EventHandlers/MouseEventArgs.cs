using MouseKeybordTest.Models;
using System;

namespace MouseKeybordTest.EventHandlers
{
    public class MouseEventArgs : EventArgs
    {
        private MouseMessages arg1;
        private POINT arg2;

        public MouseEventArgs(MouseMessages arg1, POINT arg2)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        public MouseMessages Message { get; set; }
        public POINT Point { get; set; }
    }
}
