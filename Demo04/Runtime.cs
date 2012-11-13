using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace Demo04
{
    public class Runtime : IPartImportsSatisfiedNotification
    {
        #region Setup

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

            // Register the actual class in MEF engine
            // Required to resolve the Imports
            batch.AddPart(this);

            //Start the MEF engine
            container.Compose(batch);
        }

        #endregion

        [ImportMany]
        public IEnumerable<Lazy<ISayHello, IHelloMetadata>> SayHellos { get; set; }

        public void OnImportsSatisfied()
        {
            foreach (var sayHello in SayHellos.OrderBy(s=>s.Metadata.Order))
            {
                sayHello.Value.SayHello();
            }
        }
    }

    public interface IHelloMetadata
    {
        int Order { get; }
    }

    public interface ISayHello
    {
        void SayHello();
    }

    [ExportMetadata("Order", 10)]
    [Export(typeof(ISayHello))]
    public class HablaEspanol : ISayHello
    {
        [Import]
        public IMouth Mouth { get; set; }

        public void SayHello()
        {
            Mouth.Say("Ola");
        }
    }

    [ExportMetadata("Order", 2)]
    [Export(typeof(ISayHello))]
    public class SpeakEnglish : ISayHello
    {
        [Import]
        public IMouth Mouth { get; set; }

        public void SayHello()
        {
            Mouth.Say("Hello");
        }
    }

    [ExportMetadata("Order", 3)]
    [Export(typeof(ISayHello))]
    public class SpreekNederlands : ISayHello
    {
        [Import]
        public IMouth Mouth { get; set; }

        public void SayHello()
        {
            Mouth.Say("Goeiedag");
        }
    }

    public interface IMouth
    {
        void Say(string text);
    }

    [Export(typeof(IMouth))]
    public class Mouth : IMouth
    {
        public void Say(string text)
        {
            Console.WriteLine(text);
        }
    }
}