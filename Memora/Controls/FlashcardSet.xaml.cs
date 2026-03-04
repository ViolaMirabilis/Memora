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
