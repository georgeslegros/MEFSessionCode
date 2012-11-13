using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace Demo03
{
    public class Runtime : IPartImportsSatisfiedNotification
    {
        #region SETUP

        public AggregateCatalog Catalog { get; set; }
        private CompositionContainer container;
        private CompositionBatch batch;


        public void Run()
        {
            // Create the Aggregate Catalog (Master catalog)
            Catalog = new AggregateCatalog();

            // Create a Catalog for the current assembly
            Catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            // Create the CompositionContainer
            container = new CompositionContainer(Catalog);

            batch = new CompositionBatch();


#if SILVERLIGHT
            batch.AddPart(System.Windows.Application.Current.RootVisual);
#endif

            // Register the actual class in MEF engine
            // Required to resolve the Imports
            batch.AddPart(this);

            //Start the MEF engine
            container.Compose(batch);
        }

        #endregion

        [Import]
        public ExportFactory<ISayHello> SayHello { get; set; }


        [Import]
        public IMouth Mouth { get; set; }

        public void OnImportsSatisfied()
        {
            Mouth.Say("BEFORE LOOP");
            for (var i = 0; i < 5; i++)
                SayHello.CreateExport().Value.SayHello();
        }
    }

    public interface ISayHello
    {
        void SayHello();
    }

    [Export(typeof(ISayHello))]
    public class HablaEspanol : ISayHello, IPartImportsSatisfiedNotification
    {
        [Import]
        public IMouth Mouth { get; set; }

        private Guid id = Guid.NewGuid();

        public void SayHello()
        {
            Mouth.Say(id.ToString());
            Mouth.Say("Ola");
        }

        public void OnImportsSatisfied()
        {
            Mouth.Say("CTOR");
        }
    }

    public interface IMouth
    {
        void Say(string text);
    }

#if SILVERLIGHT

    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IMouth))]
    public class Mouth : IMouth
    {
        public Mouth()
        {
            
        }

        public event EventHandler<SaidArgs> SaidSomething;

        public void InvokeSaidSomething(string text)
        {
            EventHandler<SaidArgs> handler = SaidSomething;
            if (handler != null) handler(this, new SaidArgs { Text = text });
        }

        public void Say(string text)
        {
            InvokeSaidSomething(text);
        }
    }

    public class SaidArgs : EventArgs
    {
        public string Text { get; set; }
    }

#else
    [Export(typeof(IMouth))]
    public class Mouth : IMouth
    {
        public void Say(string text)
        {
            Console.WriteLine(text);
        }
    }
#endif


}