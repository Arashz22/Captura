using System.IO;
using System.Linq;
using System.Reflection;
using Captura.Models;
using Reactive.Bindings;

namespace Captura.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LicensesViewModel
    {
        public LicensesViewModel()
        {
            var selfPath = Assembly.GetEntryAssembly().Location;

            var folder = Path.Combine(Path.GetDirectoryName(selfPath), "licenses");

            if (Directory.Exists(folder))
            {
                Licenses = Directory.EnumerateFiles(folder).Select(FileName => new FileContentItem(FileName)).ToArray();

                if (Licenses.Length > 0)
                {
                    SelectedLicense.Value = Licenses[0];
                }
            }
        }

        public FileContentItem[] Licenses { get; }

        public IReactiveProperty<FileContentItem> SelectedLicense { get; } = new ReactiveProperty<FileContentItem>();
    }
}