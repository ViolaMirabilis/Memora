using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ImportFromTextPopUp.xaml
    /// </summary>
    public partial class ImportFromTextPopUp : UserControl
    {
        public ImportFromTextPopUp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bindable bool value (bool to visibility), which checks if the pop up is or isn't open.
        /// </summary>
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty =
             DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(ImportFromTextPopUp), new PropertyMetadata(false));

        /// <summary>
        /// Bindable char value to keep track of what the current "separator character" is.
        /// </summary>
        public string Separator
        {
            get => (string)GetValue(SeparatorProperty);
            set => SetValue(SeparatorProperty, value);
        }

        public static readonly DependencyProperty SeparatorProperty =
            DependencyProperty.Register(nameof(Separator), typeof(string), typeof(ImportFromTextPopUp), new PropertyMetadata(null));

        /// <summary>
        /// Bindable command to the "Import" button.
        /// </summary>
        public ICommand ImportCommand
        {
            get => (ICommand)GetValue(ImportCommandProperty);
            set => SetValue(ImportCommandProperty, value);
        }

        public static readonly DependencyProperty ImportCommandProperty =
            DependencyProperty.Register(nameof(ImportCommand), typeof(ICommand), typeof(ImportFromTextPopUp), new PropertyMetadata(null));

        /// <summary>
        /// Returns the content of a given control.
        /// In this case, the text from the textbox.
        /// </summary>
        public object ImportCommandParameter
        {
            get => GetValue(ImportCommandParameterProperty);
            set => SetValue(ImportCommandParameterProperty, value);
        }

        public static readonly DependencyProperty ImportCommandParameterProperty =
            DependencyProperty.Register(nameof(ImportCommandParameter), typeof(object), typeof(ImportFromTextPopUp), new PropertyMetadata(null));


        /// <summary>
        /// For a bindable command that is executed once the "X" button is clicked
        /// </summary>
        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(ImportFromCodePopUp), new PropertyMetadata(null));

    }
}
