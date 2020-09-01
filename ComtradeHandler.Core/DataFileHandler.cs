using System.Linq;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// Description of DataFileHandler.
    /// </summary>
    public class DataFileHandler
    {
        internal DataFileSample[] Samples;

        internal const int SampleNumberLength = 4;
        internal const int TimeStampLength = 4;
        internal const int Digital16ChannelLength = 2;

        public static int GetDigitalByteCount(int digitalChannelsCount)
        {
            return
                (digitalChannelsCount / 16 + ((digitalChannelsCount % 16) == 0 ? 0 : 1)) * DataFileHandler.Digital16ChannelLength;
        }

        public static int GetByteCount(int analogsChannelsCount, int digitalChannelsCount, DataFileType dataFileType)
        {
            int analogOneChannelLength = dataFileType == DataFileType.Binary ? 2 : 4;
            return DataFileHandler.SampleNumberLength +
                    DataFileHandler.TimeStampLength +
                    analogsChannelsCount * analogOneChannelLength +
                    DataFileHandler.GetDigitalByteCount(digitalChannelsCount);
        }

        internal DataFileHandler(string fullPathToFileDAT, ConfigurationHandler configuration)
        {
            int samplesCount = configuration.SampleRates[configuration.SampleRates.Count - 1].LastSampleNumber;
            this.Samples = new DataFileSample[samplesCount];

            if (configuration.DataFileType == DataFileType.Binary ||
               configuration.DataFileType == DataFileType.Binary32 ||
               configuration.DataFileType == DataFileType.Float32)
            {
                var fileContent = System.IO.File.ReadAllBytes(fullPathToFileDAT);

                int oneSampleLength = DataFileHandler.GetByteCount(configuration.AnalogChannelsCount,
                                                                 configuration.DigitalChannelsCount,
                                                                 configuration.DataFileType);

                for (int i = 0; i < samplesCount; i++)
                {
                    var bytes = new byte[oneSampleLength];
                    for (int j = 0; j < oneSampleLength; j++)
                    {
                        bytes[j] = fileContent[i * oneSampleLength + j];
                    }
                    this.Samples[i] = new DataFileSample(bytes, configuration.DataFileType,
                                                       configuration.AnalogChannelsCount, configuration.DigitalChannelsCount);
                }

            }
            else if (configuration.DataFileType == DataFileType.ASCII)
            {
                var strings = System.IO.File.ReadAllLines(fullPathToFileDAT);
                //removing empty strings (when *.dat file not following Standard)
                strings = strings.Where(x => x != string.Empty).ToArray();
                for (int i = 0; i < samplesCount; i++)
                {
                    this.Samples[i] = new DataFileSample(strings[i],
                                                       configuration.AnalogChannelsCount, configuration.DigitalChannelsCount);
                }
            }
        }
    }
}
