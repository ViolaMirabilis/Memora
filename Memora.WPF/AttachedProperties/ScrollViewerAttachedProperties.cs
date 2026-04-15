using System.Windows;
using System.Windows.Controls;

namespace MemoraWPF.AttachedProperties;

public static class ScrollViewerAttachedProperties
{
    /// <summary>
    ///  Registering the attached property
    /// </summary>
    public static readonly DependencyProperty ScrollToBottomOnChangeProperty =
        DependencyProperty.RegisterAttached("ScrollToBottomOnChange",
            typeof(object),
            typeof(ScrollViewerAttachedProperties),
            new PropertyMetadata(default(ScrollViewer),
                OnScrollToBottomOnChangeChanged));


    /// <summary>
    /// This method is called once the attached property is changed.
    /// It scrolls the scroll viewer to the very bottom
    /// </summary>
    /// <param name="dependencyObject"></param>
    /// <param name="args"></param>
    private static void OnScrollToBottomOnChangeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        var scrollViewer = dependencyObject as ScrollViewer;
        scrollViewer?.ScrollToBottom();
    }

    public static void SetScrollToBottomOnChange(DependencyObject element, object value)
    {
        element.SetValue(ScrollToBottomOnChangeProperty, value);
    }

    public static object GetScrollToBottomOnChange(DependencyObject element)
    {
        return element.GetValue(ScrollToBottomOnChangeProperty);
    }


}
