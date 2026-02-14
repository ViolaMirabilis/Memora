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

namespace Memora.View
{
    /// <summary>
    /// Interaction logic for RevisionModeView.xaml
    /// </summary>
    public partial class RevisionModeView : UserControl
    {
        public RevisionModeView()
        {
            InitializeComponent();

            // Focuses the keyboard on the flashcard control so it is targetable by the "spacebar" right away
            Loaded += (s, e) =>
            {
                Keyboard.Focus(FlashcardControl);
            };
        }
    }
}
