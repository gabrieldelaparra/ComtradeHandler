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

            Assert.That(sampleRate.SamplingFrequency, Is.EqualTo(0).Within(0.1));
            Assert.That(sampleRate.LastSampleNumber, Is.EqualTo(1360));
        }
    }
}