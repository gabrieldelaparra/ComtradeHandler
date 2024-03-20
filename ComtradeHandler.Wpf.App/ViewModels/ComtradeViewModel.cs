﻿using ComtradeHandler.Core.Models;
using ComtradeHandler.Wpf.App.Core;

using Microsoft.Extensions.Logging;

using ScottPlot;
using ScottPlot.WPF;

using System.Collections.ObjectModel;

namespace ComtradeHandler.Wpf.App.ViewModels;
public class ComtradeViewModel : ViewModelBase
{
    private readonly ILogger<ComtradeViewModel> _logger;

    public ComtradeViewModel(ILogger<ComtradeViewModel> logger)
    {
        _logger = logger;

        var cfgFile = "Resources\\Comtrade1.cfg";
        var datFile = "Resources\\Comtrade1.dat";

        WpfPlots = new ObservableCollection<WpfPlot>();

        var reader = new ComtradeHandler.Core.Handlers.RecordReader(datFile);
        ComtradeConfiguration = reader.Configuration;
        ComtradeData = reader.Data;

        for (int i = 0; i < reader.Configuration.AnalogChannelsCount; i++) {
            var wpfPlot = new WpfPlot();
            var signal = reader.GetAnalogPrimaryChannel(i).ToArray();
            wpfPlot.Plot.Add.Signal(signal);
            wpfPlot.Plot.Layout.Fixed(new PixelPadding(60, 10, 30, 10));

            var min = signal.Min();
            var max = signal.Max();
            var hGap = 20;
            var d = 0.25;

            var minX = 0 - hGap;
            var maxX = signal.Length + hGap;
            var gMin = Math.Max(Math.Abs(min * d), hGap);
            var gMax = Math.Max(Math.Abs(max * d), hGap);
            var minY = min - gMin;
            var maxY = max + gMax;

            ScottPlot.AxisRules.MaximumBoundary maximumBoundary = new(
                xAxis: wpfPlot.Plot.Axes.Bottom,
                yAxis: wpfPlot.Plot.Axes.Left,
                limits: new AxisLimits(minX, maxX, minY, maxY));

            ScottPlot.AxisRules.LockedVertical lockedVertical = new(wpfPlot.Plot.Axes.Left);

            wpfPlot.Plot.Axes.Rules.Clear();
            wpfPlot.Plot.Axes.Rules.Add(maximumBoundary);
            wpfPlot.Plot.Axes.Rules.Add(lockedVertical);

            wpfPlot.Plot.RenderManager.AxisLimitsChanged += ApplyLayoutToOtherPlot;

            wpfPlot.Refresh();
            WpfPlots.Add(wpfPlot);
        }

    }

    private void ApplyLayoutToOtherPlot(object? sender, RenderDetails render)
    {
        if (sender is null)
            return;

        var source = (Plot)sender;

        foreach (var dest in WpfPlots) {
            if (dest == sender)
                continue;


            AxisLimits axesBefore = dest.Plot.Axes.GetLimits();
            dest.Plot.Axes.SetLimitsX(source.Axes.GetLimits());
            AxisLimits axesAfter = dest.Plot.Axes.GetLimits();
            if (axesBefore != axesAfter) {
                dest.Refresh();
            }
        }
    }

    private ComtradeConfiguration? ComtradeConfiguration { get; set; }
    private ComtradeData? ComtradeData { get; set; }

    // This is not a good practice (Wpf Control in the VM),
    // But it is the only way to have the code in the ViewModel (and not in the code behind in the View).
    // ¯\_(ツ)_/¯
    public ObservableCollection<WpfPlot> WpfPlots { get; }
}
