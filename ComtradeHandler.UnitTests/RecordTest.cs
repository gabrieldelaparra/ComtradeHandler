﻿using System;
using ComtradeHandler.Core;
using NUnit.Framework;

namespace ComtradeHandler.UnitTests
{
    [TestFixture]
    public class RecordTest
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
        }
    }
}