using System.Collections.ObjectModel;
using System.Windows.Input;
using Reactive.Bindings;

namespace Captura.ViewModels
{
    public abstract class OverlayListViewModel<T> where T : class, new()
    {
        protected OverlayListViewModel(ObservableCollection<T> Collection)
        {
            _collection = Collection;

            this.Collection = new ReadOnlyObservableCollection<T>(_collection);

            AddCommand = new DelegateCommand(OnAddExecute);

            RemoveCommand = new DelegateCommand(OnRemoveExecute);

            if (Collection.Count > 0)
            {
                SelectedItem.Value = Collection[0];
            }
        }

        void OnAddExecute()
        {
            var item = new T();

            _collection.Add(item);

            SelectedItem.Value = item;
        }

        void OnRemoveExecute(object O)
        {
            if (O is T setting)
            {
                _collection.Remove(setting);
            }

            SelectedItem.Value = _collection.Count > 0 ? _collection[0] : null;
        }

        readonly ObservableCollection<T> _collection;

        public ReadOnlyObservableCollection<T> Collection { get; }

        public ICommand AddCommand { get; }

        public ICommand RemoveCommand { get; }

        public IReactiveProperty<T> SelectedItem { get; } = new ReactiveProperty<T>();
    }
}