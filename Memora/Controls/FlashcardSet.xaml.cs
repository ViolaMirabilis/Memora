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


        public static readonly DependencyProperty SetName =
            DependencyProperty.Register(
                name: "SetName",
                propertyType: typeof(string),
                ownerType: typeof(FlashcardSet),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: "12345"));


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
        /// Bindable rename command property
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

    }
}
