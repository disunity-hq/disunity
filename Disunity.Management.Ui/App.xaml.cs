using Avalonia;
using Avalonia.Markup.Xaml;

namespace Disunity.Management.Ui
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}