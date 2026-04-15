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
    /// Interaction logic for ImportFromCodePopUp.xaml
    /// </summary>
    public partial class ImportFromCodePopUp : UserControl
    {
        public ImportFromCodePopUp()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CodeTextProperty =
            DependencyProperty.Register(
                name: "CodeText",
                propertyType: typeof(string),
                ownerType: typeof(ImportFromCodePopUp),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: string.Empty));

        public string CodeText
        {
            get => (string)GetValue(CodeTextProperty);
            set => SetValue(CodeTextProperty, value);
        }

        /// <summary>
        /// For a bindable command that is executed once the "Import" button is clicked
        /// </summary>
        public ICommand ImportCommand
        {
            get => (ICommand)GetValue(ImportCommandProperty);
            set => SetValue(ImportCommandProperty, value);
        }

        public static readonly DependencyProperty ImportCommandProperty =
            DependencyProperty.Register(nameof(ImportCommand), typeof(ICommand), typeof(ImportFromCodePopUp), new PropertyMetadata(null));

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
