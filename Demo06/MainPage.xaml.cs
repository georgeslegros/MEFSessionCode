using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;
using System.ComponentModel.Composition;

namespace Demo06
{
    public partial class MainPage : IPartImportsSatisfiedNotification
    {
        [ImportMany(AllowRecomposition = true)]
        public ObservableCollection<FrameworkElement> Modules { get; set; }

        private readonly Runtime runtime = new Runtime();
        public MainPage()
        {
            InitializeComponent();
            runtime.Run(this);
            items.ItemsSource = Modules;
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            runtime.LoadDeploymentCatalog("Demo06Module01.xap");
        }
        private void Button2Click(object sender, RoutedEventArgs e)
        {
            runtime.LoadDeploymentCatalog("Demo06Module02.xap");
        }
        private void Button3Click(object sender, RoutedEventArgs e)
        {
            runtime.LoadDeploymentCatalog("Demo06Module03.xap");
        }

        public void OnImportsSatisfied()
        {
            
        }
    }

    public class Runtime
    {
        #region Setup

        public AggregateCatalog Catalog { get; set; }
        private CompositionContainer container;
        private CompositionBatch batch;

        public void Run(object owner)
        {
            // Create the Aggregate Catalog (Master catalog)
            Catalog = new AggregateCatalog();

            // Create a Catalog for the current assembly
            Catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            // Create the CompositionContainer
            container = new CompositionContainer(Catalog, new ApplicationSettingExportProvider(Application.Current.Host.InitParams));

            batch = new CompositionBatch();

            // Register the actual class in MEF engine
            // Required to resolve the Imports
            batch.AddPart(this);
            batch.AddPart(owner);

            //Start the MEF engine
            container.Compose(batch);
        }

        #endregion

        public void LoadDeploymentCatalog(string path)
        {
            DeploymentCatalog catalog = new DeploymentCatalog(path);
            catalog.DownloadCompleted += CatalogDownloadCompleted;
            catalog.DownloadProgressChanged += CatalogDownloadProgressChanged;
            catalog.DownloadAsync();
            Catalog.Catalogs.Add(catalog);
        }

        void CatalogDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
        }

        void CatalogDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
        }

    }
}
