////////////////////////////////////////////////////////////////////////////////
/// ORIGINAL AUTHOR : http://code.dortikum.net/2011/01/05/extending-mef/
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Demo06
{
    public class ApplicationSettingExportProvider : ExportProvider
    {
        private readonly List<Export> exports;
        public ApplicationSettingExportProvider(IDictionary<string, string> settings)
        {
            exports = new List<Export>();
            foreach (var key in settings.Keys)
            {
                var metadata = new Dictionary<string, object>();
                metadata.Add(CompositionConstants.ExportTypeIdentityMetadataName, typeof(string).FullName);
                var value = settings[key];
                var exportDefinition = new ExportDefinition(key, metadata);
                exports.Add(new Export(exportDefinition, () => value));
            }
        }

        protected override IEnumerable<Export> GetExportsCore(
            ImportDefinition definition, AtomicComposition atomicComposition)
        {
            return exports.Where(x => definition.IsConstraintSatisfiedBy(x.Definition));
        }
    }
}