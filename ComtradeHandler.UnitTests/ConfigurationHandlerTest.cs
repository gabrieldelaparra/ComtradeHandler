using ComtradeHandler.Core;

using NUnit.Framework;

using System;

namespace ComtradeHandler.UnitTests
{
    [TestFixture]
    public class ConfigurationHandlerTest
    {
        [Test]
        public void ParserTest()
        {
            const string str = @"MASHUK-W2D-C60-1    ,520                 ,1999
 5,2A, 3D
  1,F1-IA               ,A,,A     ,     0.001953,0,0,-32767,32767,   1000.0,  1.0,S
  2,F2-IB               ,B,,A     ,     0.001953,0,0,-32767,32767,   1000.0,  1.0,S
    1,OSC TRIG     On     ,,,0
  2,W7a_KQC A    Off    ,,,0
  3,W7c_KQC B    Off    ,,,0
50
0
0,  1360
06/04/2012,06:42:59.391076
06/04/2012,06:42:59.895690
BINARY
1.00
";
            var strings = str.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var configHandler = new ConfigurationHandler();
            configHandler.Parse(strings);

            Assert.That(configHandler.StationName, Is.EqualTo("MASHUK-W2D-C60-1"));
            Assert.That(configHandler.DeviceId, Is.EqualTo("520"));
            Assert.That(configHandler.Version, Is.EqualTo(ComtradeVersion.V1999));
            Assert.That(configHandler.AnalogChannelsCount, Is.EqualTo(2));
            Assert.That(configHandler.AnalogChannelInformationList.Count, Is.EqualTo(2));
            Assert.That(configHandler.DigitalChannelsCount, Is.EqualTo(3));
            Assert.That(configHandler.DigitalChannelInformationList.Count, Is.EqualTo(3));
            Assert.That(configHandler.Frequency, Is.EqualTo(50).Within(0.1));
            Assert.That(configHandler.SamplingRateCount, Is.EqualTo(0));
            Assert.That(configHandler.SampleRates.Count, Is.EqualTo(1));
            //время1
            //время2
            Assert.That(configHandler.DataFileType, Is.EqualTo(DataFileType.Binary));
            //остальное дописать
        }
    }
}