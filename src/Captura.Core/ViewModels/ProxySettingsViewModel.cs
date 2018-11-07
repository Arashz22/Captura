using System.Collections.Generic;
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Captura.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProxySettingsViewModel
    {
        public ProxySettingsViewModel(Settings Settings)
        {
            ProxySettings = Settings.Proxy;

            var proxyTypeObservable = ProxySettings.ObserveProperty(M => M.Type);

            CanAuth = proxyTypeObservable
                .Select(M => M != ProxyType.None)
                .ToReadOnlyReactiveProperty();

            CanAuthCred = new[]
            {
                CanAuth,
                ProxySettings.ObserveProperty(M => M.Authenticate)
            }.Merge().ToReadOnlyReactiveProperty();

            CanHost = proxyTypeObservable
                .Select(M => M == ProxyType.Manual)
                .ToReadOnlyReactiveProperty();
        }

        public ProxySettings ProxySettings { get; }

        public IReadOnlyReactiveProperty<bool> CanAuth { get; }

        public IReadOnlyReactiveProperty<bool> CanAuthCred { get; }

        public IReadOnlyReactiveProperty<bool> CanHost { get; }

        public IEnumerable<ProxyType> ProxyTypes { get; } = new[] { ProxyType.None, ProxyType.System, ProxyType.Manual };
    }
}