using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using System.Collections.ObjectModel;

namespace Memora.ViewModels;

public class HomeViewModel : ViewModel
{

    public ObservableCollection<FlashcardSet> FlashcardSetsView { get; }

    private INavigationService _navigation;
    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    public HomeViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        FlashcardSetsView = new ObservableCollection<FlashcardSet>{
            new FlashcardSet { Name = "English" },
            new FlashcardSet { Name = "Polish" },
            new FlashcardSet { Name = "Comp SCI" },
            new FlashcardSet { Name = "Maths" },
            new FlashcardSet { Name = "Reverse engineering" },
            new FlashcardSet { Name = "Algebra" },
            new FlashcardSet { Name = "Embedded systems" },
            new FlashcardSet { Name = "Databases" },
            new FlashcardSet { Name = "Programming" } };
    }
}
