using System;
using Comtrade.Core;
using Xunit;

namespace Comtrade.UnitTests;

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

        //record=new RecordReader(@"D:\YandexDisk\Oscillogram\Sepam_StopingGenerator_B\1.DAT");
        //record.GetTimeLine();
        //record.GetAnalogPrimaryChannel(0);
        //record.GetDigitalChannel(0);

        //record=new RecordReader(@"D:\YandexDisk\Oscillogram\Undefined_2013_B32\000.DAT");
        //record.GetTimeLine();
        //record.GetAnalogPrimaryChannel(0);
        //record.GetDigitalChannel(0);
    }
}