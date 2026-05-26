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

namespace MemoraWPF.Controls
{
    /// <summary>
    /// Interaction logic for BindablePasswordbox.xaml
    /// </summary>
    public partial class BindablePasswordbox : UserControl
    {

        // propdp tab tab works here, lol.
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(BindablePasswordbox), new PropertyMetadata(string.Empty));


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // whenever the password changes, an event gets fired and the property is set to the desired one
            Password = passwordBox.Password;
        }


        public BindablePasswordbox()
        {
            InitializeComponent();
        }
    }
}
