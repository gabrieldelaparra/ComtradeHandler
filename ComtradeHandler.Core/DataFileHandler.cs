using System.Linq;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// Description of DataFileHandler.
    /// </summary>
    public class DataFileHandler
    {
        internal DataFileSample[] samples;

        internal const int sampleNumberLength = 4;
        internal const int timeStampLength = 4;
        internal const int digital16ChannelLength = 2;

        public static int GetDigitalByteCount(int digitalChannelsCount)
        {
            return
                (digitalChannelsCount / 16 + ((digitalChannelsCount % 16) == 0 ? 0 : 1)) * DataFileHandler.digital16ChannelLength;
        }

        public static int GetByteCount(int analogsChannelsCount, int digitalChannelsCount, DataFileType dataFileType)
        {
            int analogOneChannelLength = dataFileType == DataFileType.Binary ? 2 : 4;
            return DataFileHandler.sampleNumberLength +
                    DataFileHandler.timeStampLength +
                    analogsChannelsCount * analogOneChannelLength +
                    DataFileHandler.GetDigitalByteCount(digitalChannelsCount);
        }

        internal DataFileHandler(string fullPathToFileDAT, ConfigurationHandler configuration)
        {
            int samplesCount = configuration.sampleRates[configuration.sampleRates.Count - 1].lastSampleNumber;
            this.samples = new DataFileSample[samplesCount];

            if (configuration.dataFileType == DataFileType.Binary ||
               configuration.dataFileType == DataFileType.Binary32 ||
               configuration.dataFileType == DataFileType.Float32)
            {
                var fileContent = System.IO.File.ReadAllBytes(fullPathToFileDAT);

                int oneSampleLength = DataFileHandler.GetByteCount(configuration.analogChannelsCount,
                                                                 configuration.digitalChannelsCount,
                                                                 configuration.dataFileType);

                for (int i = 0; i < samplesCount; i++)
                {
                    var bytes = new byte[oneSampleLength];
                    for (int j = 0; j < oneSampleLength; j++)
                    {
                        bytes[j] = fileContent[i * oneSampleLength + j];
                    }
                    this.samples[i] = new DataFileSample(bytes, configuration.dataFileType,
                                                       configuration.analogChannelsCount, configuration.digitalChannelsCount);
                }

            }
            else if (configuration.dataFileType == DataFileType.ASCII)
            {
                var strings = System.IO.File.ReadAllLines(fullPathToFileDAT);
                strings = strings.Where(x => x != string.Empty).ToArray();//removing empty strings (when *.dat file not following Standart)
                for (int i = 0; i < samplesCount; i++)
                {
                    this.samples[i] = new DataFileSample(strings[i],
                                                       configuration.analogChannelsCount, configuration.digitalChannelsCount);
                }
            }
        }
    }
}
