using Memora.Core;

namespace Memora.Interfaces;

public interface INavigationService
{
    ViewModel CurrentView { get; }
    void NavigateTo<T>() where T : ViewModel;
    void NavigateTo<T>(Action<T> intialise) where T : ViewModel;
}
