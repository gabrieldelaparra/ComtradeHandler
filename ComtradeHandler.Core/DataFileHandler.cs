using System;
using System.Linq;

namespace Comtrade.Core;

/// <summary>
///     Description of DataFileHandler.
/// </summary>
public class DataFileHandler
{
    internal const int SampleNumberLength = 4;
    internal const int TimeStampLength = 4;
    internal const int Digital16ChannelLength = 2;
    internal DataFileSample[] Samples;

    //internal DataFileHandler(string fullPathToFileDAT, ConfigurationHandler configuration)
    //{
    //    var samplesCount = configuration.SampleRates[configuration.SampleRates.Count - 1].LastSampleNumber;
    //    Samples = new DataFileSample[samplesCount];

    //    if (configuration.DataFileType == DataFileType.Binary ||
    //        configuration.DataFileType == DataFileType.Binary32 ||
    //        configuration.DataFileType == DataFileType.Float32) {
    //        var fileContent = File.ReadAllBytes(fullPathToFileDAT);

    //        var oneSampleLength = GetByteCountInOneSample(configuration.AnalogChannelsCount,
    //                                                      configuration.DigitalChannelsCount,
    //                                                      configuration.DataFileType);

    //        for (var i = 0; i < samplesCount; i++) {
    //            var bytes = new byte[oneSampleLength];

    //            for (var j = 0; j < oneSampleLength; j++) {
    //                bytes[j] = fileContent[i * oneSampleLength + j];
    //            }

    //            Samples[i] = new DataFileSample(bytes, configuration.DataFileType,
    //                                            configuration.AnalogChannelsCount, configuration.DigitalChannelsCount);
    //        }
    //    }
    //    else if (configuration.DataFileType == DataFileType.ASCII) {
    //        var strings = File.ReadAllLines(fullPathToFileDAT);
    //        //removing empty strings (when *.dat file not following Standard)
    //        strings = strings.Where(x => x != string.Empty).ToArray();

    //        for (var i = 0; i < samplesCount; i++) {
    //            Samples[i] = new DataFileSample(strings[i],
    //                                            configuration.AnalogChannelsCount, configuration.DigitalChannelsCount);
    //        }
    //    }
    //}
    internal DataFileHandler(string[] strings, ConfigurationHandler configuration)
    {
        var samplesCount = configuration.SampleRates[configuration.SampleRates.Count - 1].LastSampleNumber;
        Samples = new DataFileSample[samplesCount];

        if (configuration.DataFileType == DataFileType.ASCII) {
            strings = strings.Where(x => x != string.Empty).ToArray(); //removing empty strings (when *.dat file not following Standart)

            for (var i = 0; i < samplesCount; i++) {
                Samples[i] = new DataFileSample(strings[i],
                                                configuration.AnalogChannelsCount, configuration.DigitalChannelsCount);
            }
        }
        else {
            throw new InvalidOperationException($"Configuration dataFileType must be ASCII, but was {configuration.DataFileType}");
        }
    }

    internal DataFileHandler(byte[] bytes, ConfigurationHandler configuration)
    {
        var samplesCount = configuration.SampleRates[^1].LastSampleNumber;
        Samples = new DataFileSample[samplesCount];

        if (configuration.DataFileType == DataFileType.Binary ||
            configuration.DataFileType == DataFileType.Binary32 ||
            configuration.DataFileType == DataFileType.Float32) {
            //var fileContent = System.IO.File.ReadAllBytes(fullPathToFileDAT);

            var oneSampleLength = GetByteCountInOneSample(configuration.AnalogChannelsCount,
                                                          configuration.DigitalChannelsCount,
                                                          configuration.DataFileType);

            for (var i = 0; i < samplesCount; i++) {
                var bytesOneSample = new byte[oneSampleLength];

                for (var j = 0; j < oneSampleLength; j++) {
                    bytesOneSample[j] = bytes[i * oneSampleLength + j];
                }

                Samples[i] = new DataFileSample(bytesOneSample, configuration.DataFileType,
                                                configuration.AnalogChannelsCount, configuration.DigitalChannelsCount);
            }
        }
        else {
            throw new InvalidOperationException($"Configuration dataFileType must be Binary, Binary32 or Float , but was {configuration.DataFileType}");
        }
    }

    public static int GetDigitalByteCount(int digitalChannelsCount)
    {
        return
            (digitalChannelsCount / 16 + (digitalChannelsCount % 16 == 0 ? 0 : 1)) * Digital16ChannelLength;
    }

    public static int GetByteCountInOneSample(int analogsChannelsCount, int digitalChannelsCount, DataFileType dataFileType)
    {
        var analogOneChannelLength = dataFileType == DataFileType.Binary ? 2 : 4;

        return SampleNumberLength +
               TimeStampLength +
               analogsChannelsCount * analogOneChannelLength +
               GetDigitalByteCount(digitalChannelsCount);
    }
}
