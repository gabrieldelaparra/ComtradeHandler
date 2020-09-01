using ComtradeHandler.Core;

using NUnit.Framework;

namespace ComtradeHandler.UnitTests
{
    [TestFixture]
    public class SampleRateTest
    {
        [Test]
        public void ParseTest()
        {
            const string str = @" 0 ,  1360";
            var sampleRate = new SampleRate(str);

            Assert.That(sampleRate.samplingFrequency, Is.EqualTo(0).Within(0.1));
            Assert.That(sampleRate.lastSampleNumber, Is.EqualTo(1360));
        }
    }
}