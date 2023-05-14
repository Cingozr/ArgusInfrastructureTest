using System;
using System.Windows.Forms;

namespace MouseKeybordTest.Services
{
    public class KeyboardListener : IKeyboardListener
    {
        public void OnKeyPress(Keys key)
        {
            // Tuş vuruşu işleme kodu burada
            Console.WriteLine($"Pressed key: {key}");
        }
    }
}
