using ComtradeHandler.Core;

using NUnit.Framework;

namespace ComtradeHandler.UnitTests
{
    [TestFixture]
    public class DigitalChannelInfoTest
    {
        [Test]
        public void ParserTest()
        {
            const string str = @"  4,W8a_KQC C    Off    ,,,0";
            var channelInfo = new DigitalChannelInformation(str);

            Assert.That(channelInfo.Index, Is.EqualTo(4));
            Assert.That(channelInfo.Name, Is.EqualTo("W8a_KQC C    Off"));
            Assert.That(channelInfo.Phase, Is.EqualTo(""));
            Assert.That(channelInfo.CircuitComponent, Is.EqualTo(""));
            Assert.That(channelInfo.NormalState, Is.EqualTo(false));
        }
    }
}