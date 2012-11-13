using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Demo05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IPartImportsSatisfiedNotification
    {
        [ImportMany(AllowRecomposition = true)]
        public ObservableCollection<FrameworkElement> Modules { get; set; }
        
        [Import("DNH", AllowDefault = true, AllowRecomposition = true)]
        public FrameworkElement Module { get; set; }

        private readonly Runtime runtime = new Runtime();


        public MainWindow()
        {
            InitializeComponent();
            runtime.Run(this);
            items.ItemsSource = Modules;
        }

        private void Method1Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.InitialDirectory = @"C:\";
            dialog.Filter = "Assemblies (.dll)|*.dll";
            if ((bool)dialog.ShowDialog())
            {
                runtime.LoadAssembly(dialog.FileName);
            }
        }

        private void Method2Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;

            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                runtime.LoadFromDirectory(dialog.SelectedPath);
            }
        }

        #region HIDE THIS FOR NOW
        private void Method3Click(object sender, RoutedEventArgs e)
        {
            runtime.CleanCatalogs();
        }
        #endregion

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
            container = new CompositionContainer(Catalog);

            batch = new CompositionBatch();

            // Register the actual class in MEF engine
            // Required to resolve the Imports
            batch.AddPart(this);
            batch.AddPart(owner);

            //Start the MEF engine
            container.Compose(batch);
        }

        #endregion
        public void LoadAssembly(string assemblyPath)
        {
            var catalog = new AssemblyCatalog(Assembly.LoadFrom(assemblyPath));
            Catalog.Catalogs.Add(catalog);

            #region HIDE THIS FOR NOW
            addedCatalogs.Add(catalog);
            #endregion
        }

        public void LoadFromDirectory(string directoryPath)
        {
            var catalog = new DirectoryCatalog(directoryPath);
            Catalog.Catalogs.Add(catalog);
        }

        #region HIDE THIS FOR NOW

        List<AssemblyCatalog> addedCatalogs = new List<AssemblyCatalog>();
        public void CleanCatalogs()
        {
            foreach (var catalog in addedCatalogs)
            {
                Catalog.Catalogs.Remove(catalog);
            }
        }
        #endregion
    }
}
