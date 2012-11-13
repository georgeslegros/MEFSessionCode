using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;

namespace Demo06Module03
{
    [InheritedExport(typeof(FrameworkElement))]
    public abstract partial class BaseModule
    {
        protected BaseModule(Color color)
        {
            InitializeComponent();
            DataContext = new SolidColorBrush(color);
        }
    }

    public class RedModule : BaseModule
    {
        public RedModule() : base(Colors.Red) { }
    }

    public class GreenModule : BaseModule
    {
        public GreenModule() : base(Colors.Green) { }
    }

    public class BlueModule : BaseModule
    {
        public BlueModule() : base(Colors.Blue) { }
    }

    public class YellowModule : BaseModule
    {
        public YellowModule() : base(Colors.Yellow) { }
    }
}
