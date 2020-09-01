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
            Assert.That(channelInfo.name, Is.EqualTo("W8a_KQC C    Off"));
            Assert.That(channelInfo.phase, Is.EqualTo(""));
            Assert.That(channelInfo.circuitComponent, Is.EqualTo(""));
            Assert.That(channelInfo.normalState, Is.EqualTo(false));
        }
    }
}