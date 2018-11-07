using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Captura.Models;
using Reactive.Bindings;
using Screna;

namespace Captura.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CrashLogsViewModel
    {
        public CrashLogsViewModel()
        {
            var folder = Path.Combine(ServiceProvider.SettingsDir, "Crashes");

            var canExecute = SelectedCrashLog.Select(M => M != null);

            if (Directory.Exists(folder))
            {
                CrashLogs = new ObservableCollection<FileContentItem>(Directory
                    .EnumerateFiles(folder)
                    .Select(FileName => new FileContentItem(FileName))
                    .Reverse());

                if (CrashLogs.Count > 0)
                {
                    SelectedCrashLog.Value = CrashLogs[0];
                }
            }

            CopyToClipboardCommand = canExecute.ToReactiveCommand()
                .WithSubscribe(() => SelectedCrashLog.Value.Content.WriteToClipboard());

            RemoveCommand = canExecute.ToReactiveCommand()
                .WithSubscribe(OnRemoveExecute);
        }

        void OnRemoveExecute()
        {
            if (File.Exists(SelectedCrashLog.Value.FileName))
            {
                File.Delete(SelectedCrashLog.Value.FileName);
            }

            CrashLogs.Remove(SelectedCrashLog.Value);

            SelectedCrashLog.Value = CrashLogs.Count > 0 ? CrashLogs[0] : null;
        }

        public ObservableCollection<FileContentItem> CrashLogs { get; }

        public IReactiveProperty<FileContentItem> SelectedCrashLog { get; } = new ReactiveProperty<FileContentItem>();

        public ICommand CopyToClipboardCommand { get; }

        public ICommand RemoveCommand { get; }
    }
}