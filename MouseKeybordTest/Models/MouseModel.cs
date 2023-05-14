using System;

namespace MouseKeybordTest.Models
{
    public class MouseModel
    {
        public string EventName { get; set; }
        public MouseMessages MouseMessages { get; set; }
        public Point Position { get; set; }
        public DateTime EventTime { get; set; }
        public bool IsActive { get; set; }
    }

    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
