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
    /// Interaction logic for ResultLabel.xaml
    /// </summary>
    public partial class ResultLabel : UserControl
    {
        public ResultLabel()
        {
            InitializeComponent();
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Brush TextColor
        {
            get => (Brush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public Brush LeftShade
        {
            get => (Brush)GetValue(LeftShadeProperty);
            set => SetValue(LeftShadeProperty, value);
        }

        public Brush BorderBackground
        {
            get => (Brush)GetValue(BorderBackgroundProperty);
            set => SetValue(BorderBackgroundProperty, value);
        }

        //Label
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                name: "Label",
                propertyType: typeof(string),
                ownerType: typeof(ResultLabel),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: "Placeholder label: "));

        //Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                name: "Text",
                propertyType: typeof(string),
                ownerType: typeof(ResultLabel),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: "12345"));

        //Color
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(
                name: "TextColor",
                propertyType: typeof(Brush),
                ownerType: typeof(ResultLabel),
                typeMetadata: new FrameworkPropertyMetadata(Brushes.Black));

        //Left Shade
        public static readonly DependencyProperty LeftShadeProperty =
            DependencyProperty.Register(
                name: "LeftShade",
                propertyType: typeof(Brush),
                ownerType: typeof(ResultLabel),
                typeMetadata: new FrameworkPropertyMetadata(Brushes.Gray));

        //Border Background
        public static readonly DependencyProperty BorderBackgroundProperty =
            DependencyProperty.Register(
                name: "BorderBackground",
                propertyType: typeof(Brush),
                ownerType: typeof(ResultLabel),
                typeMetadata: new FrameworkPropertyMetadata(Brushes.Gray));
    }
}
