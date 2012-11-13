using System.ComponentModel.Composition;
using Demo03;

namespace Demo03_2
{
    public partial class MainPage : IPartImportsSatisfiedNotification
    {
        [Import]
        public IMouth Mouth { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        public void OnImportsSatisfied()
        {
            ((Mouth) Mouth).SaidSomething += MainPageSaidSomething;
        }

        private void MainPageSaidSomething(object sender, SaidArgs e)
        {
            listBox.Items.Add(e.Text);
        }
    }
}