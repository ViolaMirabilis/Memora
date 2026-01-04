using Memora.Core;
using Memora.Interfaces;
namespace Memora.Services;

public class NavigationService : ObservableObject, INavigationService
{
    public Core.ViewModel CurrentView => throw new NotImplementedException();

    public void Navigate<T>() where T : ViewModel
    {
        throw new NotImplementedException();
    }
}
