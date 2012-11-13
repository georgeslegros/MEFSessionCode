using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace Demo01
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

            // Register the actual class in MEF engine
            // Required to resolve the Imports
            batch.AddPart(this);

            //Start the MEF engine
            container.Compose(batch);
        }

        #endregion

        [Import]
        public ISayHello SayHello { get; set; }

        public void OnImportsSatisfied()
        {
            //foreach (var sayHello in SayHello)
            //{
            //    Console.WriteLine(sayHello.SayHello());   
            //}
           
        }


        //[Import("DNH")]
        //public ISayHello SayHello2 { get; set; }
    }

    public interface ISayHello
    {
        string SayHello();
    }

    [Export(typeof(ISayHello))]
    public class HablaEspanol : ISayHello
    {
        public string SayHello()
        {
            return "Ola";
        }
    }

    //#region STEP 2
    //[Export("DNH")]
    //public class SpeakEnglish : ISayHello
    //{
    //    public string SayHello()
    //    {
    //        return "Hello";
    //    }
    //}

    //[Export(typeof(ISayHello))]
    //public class SpreekNederlands : ISayHello
    //{
    //    public string SayHello()
    //    {
    //        return "Goeiedag";
    //    }
    //}
    //#endregion
}