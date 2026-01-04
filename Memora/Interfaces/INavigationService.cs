using Memora.Core;

namespace Memora.Interfaces;

public interface INavigationService
{
    ViewModel CurrentView { get; }
    void Navigate<T>() where T : ViewModel;
}
