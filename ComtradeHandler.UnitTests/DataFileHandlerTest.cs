using System;
using NUnit.Framework;

namespace Wisp.Comtrade
{
	[TestFixture]
    public class DataFileHandlerTest
	{
		[Test]
		public void TestDigitalByteCount()
		{
			Assert.That(DataFileHandler.GetDigitalByteCount(7),Is.EqualTo(2));
			Assert.That(DataFileHandler.GetDigitalByteCount(16),Is.EqualTo(2));
			Assert.That(DataFileHandler.GetDigitalByteCount(17),Is.EqualTo(4));
			Assert.That(DataFileHandler.GetDigitalByteCount(32),Is.EqualTo(4));			
		}
		
		[Test]
		public void TestByteCount()
		{
			Assert.That(DataFileHandler.GetByteCount(1,1,DataFileType.Binary),Is.EqualTo(12));
			Assert.That(DataFileHandler.GetByteCount(5,17,DataFileType.Binary),Is.EqualTo(22));
			Assert.That(DataFileHandler.GetByteCount(5,17,DataFileType.Float32),Is.EqualTo(32));
			Assert.That(DataFileHandler.GetByteCount(5,17,DataFileType.Binary32),Is.EqualTo(32));			
		}
	}
}
