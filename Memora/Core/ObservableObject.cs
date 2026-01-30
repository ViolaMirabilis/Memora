using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Memora.Core
{
    /// <summary>
    /// Each view model and navigation service inherits from ObservableObject, so their content can be updated in real time without additional refreshers.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)        // can be overridden
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
