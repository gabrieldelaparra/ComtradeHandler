using System;
using ComtradeHandler.Core;
using Xunit;

namespace ComtradeHandler.UnitTests;

public class RecordTest
{
    [Fact]
    public void TestNotSupportedExtensions()
    {
        Assert.Throws<InvalidOperationException>(() => new RecordReader("notComtradeExtensions.trr"));
    }

    [Fact]
    public void TestOpenFile()
    {
        RecordReader record;
        record = new RecordReader(@"Resources\sample_ascii.dat");
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
    }
}
