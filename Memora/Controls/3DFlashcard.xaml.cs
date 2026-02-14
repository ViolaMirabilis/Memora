using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Memora.Controls
{
    /// <summary>
    /// Properties for the custom 3DFlashcard control
    /// NEED TO ADD A TIMER LATER ON! So the control cannot be flipped back and forth too fast.
    /// </summary>
    public partial class _3DFlashcard : UserControl
    {
        public _3DFlashcard()
        {
            InitializeComponent();
            this.MouseLeftButtonUp += (s, e) =>
            {
                this.Focus();
                IsFront = !IsFront;
            };

            this.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Space)
                {
                    IsFront = !IsFront;
                    e.Handled = true;
                }
            };
        }

        public string FrontText
        {
            get { return (string)GetValue(FrontTextProperty); }
            set { SetValue(FrontTextProperty, value); }
        }

        public static readonly DependencyProperty FrontTextProperty =
            DependencyProperty.Register(nameof(FrontText), typeof(string), typeof(_3DFlashcard), new PropertyMetadata(string.Empty));

        public string BackText
        {
            get { return (string)GetValue(BackTextProperty); }
            set { SetValue(BackTextProperty, value); }
        }

        public static readonly DependencyProperty BackTextProperty =
            DependencyProperty.Register(nameof(BackText), typeof(string), typeof(_3DFlashcard), new PropertyMetadata(string.Empty));


        /// <summary>
        /// Animation time duration
        /// </summary>
        public double FlipDuration
        {
            get { return (double)GetValue(FlipDurationProperty); }
            set { SetValue(FlipDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FlipDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FlipDurationProperty =
            DependencyProperty.Register(nameof(FlipDuration), typeof(double), typeof(_3DFlashcard), new PropertyMetadata(0.25));



        /// <summary>
        /// Is showing front. Set to true on initialisation.
        /// </summary>
        public bool IsFront
        {
            get { return (bool)GetValue(IsFrontProperty); }
            set { SetValue(IsFrontProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFront.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFrontProperty =
            DependencyProperty.Register(nameof(IsFront), typeof(bool), typeof(_3DFlashcard), new PropertyMetadata(true, OnIsFrontChanged));


        // PropertyChangedCallback
        // Gets called once the IsFront value is changed in XAML/code behind
        // @see https://learn.microsoft.com/en-us/dotnet/api/system.windows.propertychangedcallback?view=windowsdesktop-10.0&redirectedfrom=MSDN
        // With this we avoid setting the animation from code behind or manually triggering it. We just take care of the IsFront bool and it dooes all the job.

        private static void OnIsFrontChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // casting the object to 3Dflashcard type
            var control = (_3DFlashcard)d;
            control.FlipFlashcard((bool)e.NewValue);     // casting to bool because IsFront is of type bool. NewValue is whatever we set it to later on (xaml/codebehind/bindable property)
        }

        /// <summary>
        /// It scales the X inwards (from 1 to 0), then scales the X outwards (from 1 to 0)
        /// RenderTransformOrigin (grid property in .xaml) makes it that the center point is in the middle (0 is on the left, 1 is on the right)
        /// </summary>
        /// <param name="isFront"></param>
        private void FlipFlashcard(bool isFront)
        {

            var firstHalf = new DoubleAnimation     // "0-90 degrees"
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(FlipDuration / 2),      // divided by half, so if FlipDur is 1 sec, it takes 0.5 for front and 0.5 for back
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut}
            };

            firstHalf.Completed += (s, e) =>
            {
                FrontSide.Visibility = isFront ? Visibility.Visible : Visibility.Collapsed;     // shows / collapses the border (with text blocks)
                BackSide.Visibility = isFront ? Visibility.Collapsed : Visibility.Visible;      // the same 

                var secondHalf = new DoubleAnimation        // 90 - 180 degrees
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(FlipDuration / 2),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
                };

                // RotateTransform object in the .xaml
                FlashcardRotate.BeginAnimation(ScaleTransform.ScaleXProperty, secondHalf);
            };

            FlashcardRotate.BeginAnimation(ScaleTransform.ScaleXProperty, firstHalf);

            
        }

    }
}
