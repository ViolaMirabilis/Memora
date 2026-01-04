using Memora.Core;

namespace Memora.Interfaces;

public interface INavigationService
{
    ViewModel CurrentView { get; }
    void NavigateTo<T>() where T : ViewModel;
}
