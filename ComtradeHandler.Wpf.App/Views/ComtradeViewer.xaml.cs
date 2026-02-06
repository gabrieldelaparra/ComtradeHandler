using System.Windows.Controls;
using System.Windows.Input;

namespace ComtradeHandler.Wpf.App.Views;

public partial class ComtradeViewer : UserControl
{
    public ComtradeViewer()
    {
        InitializeComponent();
    }

    private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        e.Handled = true;
    }
}
