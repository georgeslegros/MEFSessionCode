using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;

namespace Demo05Module01
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

    class RedModule : BaseModule
    {
        public RedModule() : base(Colors.Red) { }
    }

    class GreenModule : BaseModule
    {
        public GreenModule() : base(Colors.Green) { }
    }

    class BlueModule : BaseModule
    {
        public BlueModule() : base(Colors.Blue) { }
    }

    class YellowModule : BaseModule
    {
        public YellowModule() : base(Colors.Yellow) { }
    }

    [Export("DNH", typeof(FrameworkElement))]
    public class Test : FrameworkElement
    {
        
    }
}
