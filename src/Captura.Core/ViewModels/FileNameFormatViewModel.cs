using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Captura.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Captura.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FileNameFormatViewModel : ViewModelBase
    {
        public FileNameFormatViewModel(Settings Settings, LanguageManager LanguageManager)
            : base(Settings, LanguageManager)
        {
            FilenameFormat = new ReactiveProperty<string>(Settings.ObserveProperty(M => M.FilenameFormat));

            FilenamePreview = FilenameFormat.Select(M =>
                {
                    M = Path.GetInvalidFileNameChars()
                        .Aggregate(M, (Current, InvalidChar) => Current.Replace(InvalidChar.ToString(), ""));

                    Settings.FilenameFormat = M;

                    return Settings.GetFileName(".mp4");
                })
                .ToReadOnlyReactiveProperty();
        }

        public FileNameFormatGroup[] FormatGroups { get; } =
        {
            new FileNameFormatGroup("Year", new []
            {
                new FileNameFormatItem("%yyyy%", "Year (2018)"),
                new FileNameFormatItem("%yy%", "Year (18)")
            }),
            new FileNameFormatGroup("Month", new []
            {
                new FileNameFormatItem("%MMMM%", "Month (September)"),
                new FileNameFormatItem("%MMM%", "Month (Sep)"),
                new FileNameFormatItem("%MM%", "Month (09)")
            }),
            new FileNameFormatGroup("Date", new []
            {
                new FileNameFormatItem("%dd%", "Date (22)"),
                new FileNameFormatItem("%ddd%", "Day (Tue)"),
                new FileNameFormatItem("%dddd%", "Day (Tuesday)")
            }),
            new FileNameFormatGroup("Time", new []
            {
                new FileNameFormatItem("%HH%", "Hours (24 hr) (20)"),
                new FileNameFormatItem("%hh%", "Hours (12 hr) (08)"),
                new FileNameFormatItem("%mm%", "Minutes (58)"),
                new FileNameFormatItem("%ss%", "Seconds (54)"),
                new FileNameFormatItem("%tt%", "AM / PM"),
                new FileNameFormatItem("%zzz%", "Time Zone (+05:30)")
            })
        };

        public ReactiveProperty<string> FilenameFormat { get; }

        public ReadOnlyReactiveProperty<string> FilenamePreview { get; }
    }
}