using System.Linq;
using ComtradeHandler.Core;
using Xunit;
using ScottPlot;
namespace ComtradeHandler.UnitTests;

public class VisualizationTest
{
    [Fact]
    public void PlotAsciiData()
    {
        //var record = new RecordReader(@"C:\Users\Home\Desktop\Engie\2024.03.11\Respaldo Engie\SLRP\Engie Energia Chile\PE WIND CALAMA\Trafo TR2\SP004G0216SE190G0216\Oscilografias\20210204,194356924,PA018G0216SE190G0216,SP004G0216SE190G0216.dat");
        var record = new RecordReader(@"C:\Users\Home\Desktop\Engie\2024.03.11\Respaldo Engie\SLRP\Engie Energia Chile\PE WIND CALAMA\Linea J3-J4\SP001G0216SE190G0216\Oscilografias\20230112,123659440,PA001G0216SE003T0025,SP001G0216SE190G0216.dat");
        record.GetTimeLine();
        //const string str = "5 ,667 , -760, 1274,72,, 3.4028235e38,-3.4028235e38,0 ,0,0 ,0,1,1";
        //var sample = new DataFileSample(str, 6, 6); // Assuming DataFileSample is your class that parses the ASCII data

        double[] channel0 = record.GetAnalogPrimaryChannel(3).ToArray();
        double[] channel1 = record.GetAnalogPrimaryChannel(4).ToArray();
        double[] channel2 = record.GetAnalogPrimaryChannel(5).ToArray();
        //double[] channel3 = record.GetAnalogPrimaryChannel(3).ToArray();

        double[] timeAxis = record.GetTimeLine().ToArray();
        Plot formsPlot = new ();

        // Plot analog data
        var sp0 = formsPlot.Add.Scatter(timeAxis, channel0);
        sp0.Label = "Channel 0";
        sp0.MarkerSize = 0;
        var sp1 = formsPlot.Add.Scatter(timeAxis, channel1);
        sp1.Label = "Channel 1";
        sp1.MarkerSize = 0;
        var sp2 = formsPlot.Add.Scatter(timeAxis, channel2);
        sp2.Label = "Channel 2";
        sp2.MarkerSize = 0;
        //var sp3 = formsPlot.Add.Scatter(timeAxis, channel3);
        //sp3.Label = "Channel 3";

        // Plot digital data (as a step plot for better visibility)
        //for (int i = 0; i < digitalValues.Length; i++) {
        //    double y = digitalValues[i] ? 1 : 0;
        //    formsPlot.Plot.PlotStep(new double[] { timeAxis[i], timeAxis[i + 1] }, new double[] { y, y }, lineWidth: 2);
        //}

        // Set axis labels and title
        formsPlot.XLabel("Time");
        formsPlot.YLabel("Value");
        formsPlot.Title("COMTRADE ASCII Data Visualization");

        // Add legend
        formsPlot.ShowLegend();

        // Show the plot
        formsPlot.SavePng("demo.png", 2400, 700);
        //formsPlot.Render(); // Render the plot (necessary for unit testing)
    }
}
