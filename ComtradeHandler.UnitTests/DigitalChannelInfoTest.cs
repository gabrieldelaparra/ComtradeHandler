﻿using Comtrade.Core;
using Xunit;

namespace Comtrade.UnitTests;

public class DigitalChannelInfoTest
{
    [Fact]
    public void ParserTest()
    {
        const string str = @"  4,W8a_KQC C    Off    ,,,0";
        var channelInfo = new DigitalChannelInformation(str);

        Assert.Equal(4, channelInfo.Index);
        Assert.Equal("W8a_KQC C    Off", channelInfo.Name);
        Assert.Equal("", channelInfo.Phase);
        Assert.Equal("", channelInfo.CircuitComponent);
        Assert.False(channelInfo.NormalState);
    }
}
