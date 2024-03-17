using System;
using ComtradeHandler.Core.Handlers;
using Xunit;

namespace ComtradeHandler.UnitTests;

public class RecordReaderTest
{
    [Fact]
    public void TestNotSupportedExtensions()
    {
        Assert.Throws<InvalidOperationException>(() => new RecordReader("notComtradeExtension.trr"));
    }

    /// <summary>
    ///     Success only on maintainer machine
    /// </summary>
    [Fact]
    public void TestOpenFile()
    {
        var record = new RecordReader(@"Resources\sample_ascii.dat");
        record.GetTimeLine();
        record.GetAnalogPrimaryChannel(0);
        record.GetDigitalChannel(0);

        record = new RecordReader(@"Resources\sample_bin.DAT");
        record.GetTimeLine();
        record.GetAnalogPrimaryChannel(0);
        record.GetDigitalChannel(0);

        record = new RecordReader(@"Resources\sample_ascii.cFg");
        record.GetTimeLine();
        record.GetAnalogPrimaryChannel(0);
        record.GetDigitalChannel(0);

        record = new RecordReader(@"Resources\sample_bin.cfg");
        record.GetTimeLine();
        record.GetAnalogPrimaryChannel(0);
        record.GetDigitalChannel(0);
    }
}
