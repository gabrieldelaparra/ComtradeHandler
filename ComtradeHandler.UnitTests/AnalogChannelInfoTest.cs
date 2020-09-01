using ComtradeHandler.Core;

using NUnit.Framework;

namespace ComtradeHandler.UnitTests
{
    [TestFixture]
    public class AnalogChannelInfoTest
    {
        [Test]
        public void ParserTest()
        {
            const string str = @"  8,F8-VN               ,N,,V     ,     0.012207,1,2,-32767,32767, 330000.0,100.0,S";
            var channelInfo = new AnalogChannelInformation(str);

            Assert.That(channelInfo.Index, Is.EqualTo(8));
            Assert.That(channelInfo.Name, Is.EqualTo("F8-VN"));
            Assert.That(channelInfo.Phase, Is.EqualTo("N"));
            Assert.That(channelInfo.CircuitComponent, Is.EqualTo(""));
            Assert.That(channelInfo.Units, Is.EqualTo("V"));
            Assert.That(channelInfo.a, Is.EqualTo(0.012207).Within(0.001));
            Assert.That(channelInfo.b, Is.EqualTo(1).Within(0.001));
            Assert.That(channelInfo.Skew, Is.EqualTo(2).Within(0.001));
            Assert.That(channelInfo.Min, Is.EqualTo(-32767).Within(0.001));
            Assert.That(channelInfo.Max, Is.EqualTo(32767).Within(0.001));
            Assert.That(channelInfo.Primary, Is.EqualTo(330000.0).Within(0.001));
            Assert.That(channelInfo.Secondary, Is.EqualTo(100.0).Within(0.001));
            Assert.That(channelInfo.IsPrimary, Is.EqualTo(false));
        }
    }
}