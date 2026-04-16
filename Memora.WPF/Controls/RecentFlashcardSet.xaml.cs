using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Memora.Controls
{
    /// <summary>
    /// Interaction logic for RecentFlashcardSet.xaml
    /// </summary>
    public partial class RecentFlashcardSet : UserControl
    {
        public RecentFlashcardSet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Holds flashcard's set (folder) name
        /// </summary>
        public static readonly DependencyProperty SetNameProperty =
            DependencyProperty.Register(
                name: "SetName",
                propertyType: typeof(string),
                ownerType: typeof(RecentFlashcardSet),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: "default"));

        public string SetName
        {
            get => (string)GetValue(SetNameProperty);
            set => SetValue(SetNameProperty, value);
        }

        /// <summary>
        /// Holds flashcard's set count
        /// </summary>
        public static readonly DependencyProperty SetCountProperty =
            DependencyProperty.Register(
                name: "SetCount",
                propertyType: typeof(int),
                ownerType: typeof(RecentFlashcardSet),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: 0));

        public string SetCount
        {
            get => (string)GetValue(SetCountProperty);
            set => SetValue(SetCountProperty, value);
        }


        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                name: "CommandParameter",
                propertyType: typeof(object),
                ownerType: typeof(RecentFlashcardSet),
                typeMetadata: new PropertyMetadata(null));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Command that executes when the user presses on the litsview item (the entire control)
        /// </summary>
        public static readonly DependencyProperty FlashcardSetClickCommandProperty =
            DependencyProperty.Register(
                name: "FlashcardSetClickCommand",
                propertyType: typeof(ICommand),
                ownerType: typeof(RecentFlashcardSet),
                typeMetadata: new PropertyMetadata(null));

        public object FlashcardSetClickCommand
        {
            get => GetValue(FlashcardSetClickCommandProperty);
            set => SetValue(FlashcardSetClickCommandProperty, value);
        }
    }
}
