using ComtradeHandler.Core;
using ComtradeHandler.Core.Models;
using Xunit;

namespace ComtradeHandler.UnitTests;

public class DataFileHandlerTest
{
    [Fact]
    public void TestByteCount()
    {
        Assert.Equal(12, ComtradeData.GetByteCountInOneSample(1, 1, DataFileType.Binary));
        Assert.Equal(22, ComtradeData.GetByteCountInOneSample(5, 17, DataFileType.Binary));
        Assert.Equal(32, ComtradeData.GetByteCountInOneSample(5, 17, DataFileType.Float32));
        Assert.Equal(32, ComtradeData.GetByteCountInOneSample(5, 17, DataFileType.Binary32));
    }

    [Fact]
    public void TestDigitalByteCount()
    {
        Assert.Equal(2, ComtradeData.GetDigitalByteCount(7));
        Assert.Equal(2, ComtradeData.GetDigitalByteCount(16));
        Assert.Equal(4, ComtradeData.GetDigitalByteCount(17));
        Assert.Equal(4, ComtradeData.GetDigitalByteCount(32));
    }
}
