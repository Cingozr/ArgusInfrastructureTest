using System;
using System.Windows.Automation;

class Program
{
    static void Main(string[] args)
    {
        // Chrome uygulamasının ana penceresini buluyoruz
        AutomationElement chromeWindow = AutomationElement.RootElement.FindFirst(
            TreeScope.Children,
            new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));

        if (chromeWindow != null)
        {
            // TabControl elemanını buluyoruz (TabItem'lar bu kontrol içinde yer alır)
            AutomationElement tabControl = chromeWindow.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tab));

            if (tabControl != null)
            {
                // TabItem'ları buluyoruz
                AutomationElementCollection tabItems = tabControl.FindAll(
                    TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem));

                foreach (AutomationElement tabItem in tabItems)
                {
                    // TabItem'ın adını alıyoruz ve ekrana yazdırıyoruz
                    string tabName = tabItem.Current.Name;
                    Console.WriteLine(tabName);
                }
            }
            else
            {
                Console.WriteLine("TabControl elementi bulunamadı.");
            }
        }
        else
        {
            Console.WriteLine("Chrome penceresi bulunamadı.");
        }

        Console.ReadLine();
    }
}
