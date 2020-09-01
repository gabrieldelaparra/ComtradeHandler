using ComtradeHandler.Core;

using NUnit.Framework;

using System;

namespace ComtradeHandler.UnitTests
{
    [TestFixture]
    public class RecordReaderTest
    {
        [Test]
        public void TestNotSupportedExtentions()
        {
            Assert.Throws<InvalidOperationException>(() => new RecordReader("notComtradeExtentions.trr"));
        }

        /// <summary>
        /// Success only on maintainer machine 
        /// </summary>
        [Test]
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
}