﻿using ComtradeHandler.Core;

using NUnit.Framework;

using System.Collections.Generic;

namespace ComtradeHandler.UnitTests
{
    [TestFixture]
    public class DataFileSampleTest
    {
        [Test]
        public void ASCIIReadingTest()
        {
            const string str = "5 ,667 , -760, 1274,72,, 3.4028235e38,-3.4028235e38,0 ,0,0 ,0,1,1";
            var sample = new DataFileSample(str, 6, 6);

            Assert.That(sample.Number, Is.EqualTo(5));
            Assert.That(sample.Timestamp, Is.EqualTo(667));
            Assert.That(sample.AnalogValues.Length, Is.EqualTo(6));
            Assert.That(sample.AnalogValues[0], Is.EqualTo(-760));
            Assert.That(sample.AnalogValues[1], Is.EqualTo(1274));
            Assert.That(sample.AnalogValues[2], Is.EqualTo(72));
            Assert.That(sample.AnalogValues[3], Is.EqualTo(0));
            Assert.That(sample.AnalogValues[4], Is.EqualTo(3.4028235e38));
            Assert.That(sample.AnalogValues[5], Is.EqualTo(-3.4028235e38));
            Assert.That(sample.DigitalValues.Length, Is.EqualTo(6));
            Assert.That(sample.DigitalValues[0], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[1], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[2], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[3], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[4], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[5], Is.EqualTo(true));
        }

        [Test]
        public void CommonBinaryReadingTest()
        {
            byte[] bytes = {
                0x05, 0x00, 0x50, 0x00,
                0x9B, 0x02, 0x00, 0x00,
                0x08, 0xFD,
                0xFA, 0x04,
                0x48, 0x00,
                0x3D, 0x00,
                0x74, 0xFF,
                0x0A, 0xFE,
                0x30, 0x00
            };

            var sample = new DataFileSample(bytes, DataFileType.Binary, 6, 6);

            Assert.That(sample.Number, Is.EqualTo(5242885));
            Assert.That(sample.Timestamp, Is.EqualTo(667));
            Assert.That(sample.AnalogValues.Length, Is.EqualTo(6));
            Assert.That(sample.AnalogValues[0], Is.EqualTo(-760));
            Assert.That(sample.AnalogValues[1], Is.EqualTo(1274));
            Assert.That(sample.AnalogValues[2], Is.EqualTo(72));
            Assert.That(sample.AnalogValues[3], Is.EqualTo(61));
            Assert.That(sample.AnalogValues[4], Is.EqualTo(-140));
            Assert.That(sample.AnalogValues[5], Is.EqualTo(-502));
            Assert.That(sample.DigitalValues.Length, Is.EqualTo(6));
            Assert.That(sample.DigitalValues[0], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[1], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[2], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[3], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[4], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[5], Is.EqualTo(true));
        }

        [Test]
        public void CommonBinaryWritingTest()
        {
            byte[] bytes = {
                0x05, 0x00, 0x50, 0x00,
                0x9B, 0x02, 0x00, 0x00,
                0x08, 0xFD,
                0xFA, 0x04,
                0x48, 0x00,
                0x3D, 0x00,
                0x74, 0xFF,
                0x0A, 0xFE,
                0x30, 0x00
            };

            var sample = new DataFileSample("5242885,667,-760,1274,72,61,-140,-502,0,0,0,0,1,1", 6, 6);

            var analogInformations = new List<AnalogChannelInformation>();
            analogInformations.Add(new AnalogChannelInformation(string.Empty, string.Empty));
            analogInformations.Add(new AnalogChannelInformation(string.Empty, string.Empty));
            analogInformations.Add(new AnalogChannelInformation(string.Empty, string.Empty));
            analogInformations.Add(new AnalogChannelInformation(string.Empty, string.Empty));
            analogInformations.Add(new AnalogChannelInformation(string.Empty, string.Empty));
            analogInformations.Add(new AnalogChannelInformation(string.Empty, string.Empty));

            var result = sample.ToByteDAT(DataFileType.Binary, analogInformations);

            for (var i = 0; i < bytes.Length; i++)
            {
                Assert.That(result[i], Is.EqualTo(bytes[i]));
            }
        }

        [Test]
        public void DigitalOnlyBinaryReadingTest()
        {
            byte[] bytes = {
                0x05, 0x00, 0x00, 0x00,
                0x9B, 0x02, 0x00, 0x00,
                0x0F, 0x0F,
                0x5A, 0x5A,
                0x01, 0x00
            };

            var sample = new DataFileSample(bytes, DataFileType.Binary, 0, 33);

            Assert.That(sample.Number, Is.EqualTo(5));
            Assert.That(sample.Timestamp, Is.EqualTo(667));
            Assert.That(sample.AnalogValues.Length, Is.EqualTo(0));
            Assert.That(sample.DigitalValues.Length, Is.EqualTo(33));

            Assert.That(sample.DigitalValues[0], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[1], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[2], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[3], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[4], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[5], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[6], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[7], Is.EqualTo(false));

            Assert.That(sample.DigitalValues[8], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[9], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[10], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[11], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[12], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[13], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[14], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[15], Is.EqualTo(false));

            Assert.That(sample.DigitalValues[16], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[17], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[18], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[19], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[20], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[21], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[22], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[23], Is.EqualTo(false));

            Assert.That(sample.DigitalValues[24], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[25], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[26], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[27], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[28], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[29], Is.EqualTo(false));
            Assert.That(sample.DigitalValues[30], Is.EqualTo(true));
            Assert.That(sample.DigitalValues[31], Is.EqualTo(false));

            Assert.That(sample.DigitalValues[32], Is.EqualTo(true));
        }
    }
}