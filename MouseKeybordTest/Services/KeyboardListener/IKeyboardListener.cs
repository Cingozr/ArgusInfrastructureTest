using System.Windows.Forms;

namespace MouseKeybordTest.Services
{
    public interface IKeyboardListener
    {
        void OnKeyPress(Keys key);
    }
}
