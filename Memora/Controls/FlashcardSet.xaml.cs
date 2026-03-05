using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Memora.Controls
{
    /// <summary>
    /// Interaction logic for FlashcardSet.xaml
    /// </summary>
    public partial class FlashcardSet : UserControl
    {
        public FlashcardSet()
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
                ownerType: typeof(FlashcardSet),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: "12345"));

        public string SetName
        {
            get => (string)GetValue(SetNameProperty);
            set => SetValue(SetNameProperty, value);
        }

        /// <summary>
        ///  This is just a copy of command parameter, but for my custom control.
        ///  @See: https://stackoverflow.com/questions/12486660/wpf-command-and-commandparameter-for-usercontrol
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = 
            DependencyProperty.Register(
                name: "CommandParameter",
                propertyType: typeof(object),
                ownerType: typeof(FlashcardSet),
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
                ownerType: typeof(FlashcardSet),
                typeMetadata: new PropertyMetadata(null));

        public object FlashcardSetClickCommand
        {
            get => GetValue(FlashcardSetClickCommandProperty);
            set => SetValue(FlashcardSetClickCommandProperty, value);
        }

        /// <summary>
        /// Bindable three dots menu command
        /// </summary>
        public static readonly DependencyProperty ThreeDotsCommandProperty =
            DependencyProperty.Register(
                name: "ThreeDotsCommand",
                propertyType: typeof(ICommand),
                ownerType: typeof(FlashcardSet),
                typeMetadata: new PropertyMetadata(null));

        public ICommand ThreeDotsCommand
        {
            get => (ICommand)GetValue(ThreeDotsCommandProperty);
            set => SetValue(ThreeDotsCommandProperty, value);
        }


        /// <summary>
        /// Bindable rename command property.
        /// This command is ran after pressing "ENTER" on the flashcard's set name textbox.
        /// </summary>
        public static readonly DependencyProperty RenameCommandProperty =
            DependencyProperty.Register(
                name: "RenameCommand",
                propertyType: typeof(ICommand),
                ownerType: typeof(FlashcardSet),
                typeMetadata: new PropertyMetadata(null));

        public ICommand RenameCommand
        {
            get => (ICommand)GetValue(RenameCommandProperty);
            set => SetValue(RenameCommandProperty, value);
        }

        /// <summary>
        /// Bindable delete command property
        /// </summary>
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(
                name: "DeleteCommand",
                propertyType: typeof(ICommand),
                ownerType: typeof(FlashcardSet),
                typeMetadata: new PropertyMetadata(null));

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            set => SetValue(DeleteCommandProperty, value);
        }

        // focuses the flashcard set's name
        // selects the entire name
        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            // Focuses the text and selects it
            FlashcardSetName.IsReadOnly = false;
            FlashcardSetName.Focusable = true;
            FlashcardSetName.Focus();
            FlashcardSetName.SelectAll();
        }

        // on ENTER key press, the control's focus is disabled
        // RenameCommand is ran (it has to be bound by the user via XAML)
        private void FlashcardSetName_KeyDown(object sender, KeyEventArgs e)
        {
            // If user pressed enter and the flashcard set name is not empty
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(FlashcardSetName.Text))
            {
                // disable the focus on the textbox
                FlashcardSetName.IsReadOnly = true;
                FlashcardSetName.Focusable = false;
                e.Handled = true;


                // fire the "RenameCommand"
                // it returns the FlashcardSetName.Text
                RenameCommand.Execute(FlashcardSetName.Text);
            }
        }
    }
}
