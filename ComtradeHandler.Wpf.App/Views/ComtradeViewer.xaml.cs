using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComtradeHandler.Wpf.App.Views;
/// <summary>
/// Interaction logic for ComtradeViewer.xaml
/// </summary>
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
