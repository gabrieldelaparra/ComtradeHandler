using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ComtradeHandler.Core.Models;

namespace ComtradeHandler.Core.Handlers;

public class RecordReader
{
    /// <summary>
    ///     Read record from file
    /// </summary>
    public RecordReader(string fullPathToFile)
    {
        OpenFile(fullPathToFile);
    }

    /// <summary>
    ///     Read record from stream
    /// </summary>
    public RecordReader(Stream cfgStream, Stream datStream)
    {
        OpenFromStreamCfg(cfgStream);
        OpenFromStreamDat(datStream);
    }

    /// <summary>
    ///     Read record from stream (single file format, *.cff)
    /// </summary>
    public RecordReader(Stream cffStream)
    {
        OpenFromStreamCff(cffStream);
    }

    /// <summary>
    ///     Get configuration for loaded record
    /// </summary>
    public ComtradeConfiguration? Configuration { get; private set; }
    public ComtradeData? Data { get; private set; }

    /// <summary>
    ///     Units for GetTimeLine()
    /// </summary>
    /// <return>true = ns, false = ms</return>
    public bool TimeLineNanoSecondResolution
    {
        get {
            if (Configuration?.SamplingRateCount == 0 || Configuration?.SampleRates[0].SamplingFrequency == 0) {
                return Configuration.TimeLineNanoSecondResolution;
            }

            return true;
        }
    }

    private void OpenFile(string fullPathToFile)
    {
        var fileInfo = new FileInfo(fullPathToFile);
        var path = Path.GetDirectoryName(fileInfo.FullName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
        var extension = Path.GetExtension(fullPathToFile).ToLower();

        if (extension == GlobalSettings.ExtensionCFF) {
            using var cffFileStream = new FileStream(Path.Combine(path, fileNameWithoutExtension) + GlobalSettings.ExtensionCFF, FileMode.Open);
            OpenFromStreamCff(cffFileStream);
        }
        else if (extension == GlobalSettings.ExtensionCFG || extension == GlobalSettings.ExtensionDAT) {
            using var cfgFileStream = new FileStream(Path.Combine(path, fileNameWithoutExtension) + GlobalSettings.ExtensionCFG, FileMode.Open);
            OpenFromStreamCfg(cfgFileStream);

            using var datFileStream = new FileStream(Path.Combine(path, fileNameWithoutExtension) + GlobalSettings.ExtensionDAT, FileMode.Open);
            OpenFromStreamDat(datFileStream);
        }
        else {
            throw new InvalidOperationException("Unsupported file extentions. Must be *.cfg, *.dat, *.cff");
        }
    }

    private void OpenFromStreamCfg(Stream cfgStream)
    {
        var buffer = new byte[1024];
        var loadedAsListByteFile = new List<byte>();
        int readBytes;

        while ((readBytes = cfgStream.Read(buffer, 0, buffer.Length)) != 0) {
            loadedAsListByteFile.AddRange(buffer.SkipLast(buffer.Length - readBytes));
        }

        var cfgSection = Encoding.UTF8.GetString(loadedAsListByteFile.ToArray())
                                 .Split(new[] {GlobalSettings.NewLine, "\n"}, StringSplitOptions.None);

        Configuration = new ComtradeConfiguration(cfgSection.ToArray());
    }

    private void OpenFromStreamDat(Stream datStream)
    {
        var buffer = new byte[1024];
        var loadedAsListByteFile = new List<byte>();
        int readBytes;

        while ((readBytes = datStream.Read(buffer, 0, buffer.Length)) != 0) {
            loadedAsListByteFile.AddRange(buffer.SkipLast(buffer.Length - readBytes));
        }

        if (Configuration.DataFileType == DataFileType.ASCII) {
            var datSection = Encoding.UTF8.GetString(loadedAsListByteFile.ToArray())
                                     .Split(new[] {GlobalSettings.NewLine, "\n"}, StringSplitOptions.None);

            Data = new ComtradeData(datSection.ToArray(), Configuration);
        }
        else {
            Data = new ComtradeData(loadedAsListByteFile.ToArray(), Configuration);
        }
    }

    private void OpenFromStreamCff(Stream cffStream)
    {
        var cfgSection = new List<string>();

        var buffer = new byte[1024];
        var loadedAsListByteFile = new List<byte>();
        int readBytes;

        while ((readBytes = cffStream.Read(buffer, 0, buffer.Length)) != 0) {
            loadedAsListByteFile.AddRange(buffer.SkipLast(buffer.Length - readBytes));
        }

        var loadedAsArrayByte = loadedAsListByteFile.ToArray();

        var threeDashCounter = 0;
        var indexOfDataSection = -1;

        for (var i = 2; i < loadedAsListByteFile.Count; i++) {
            if (loadedAsListByteFile[i - 2] == '-' &&
                loadedAsListByteFile[i - 1] == '-' &&
                loadedAsListByteFile[i - 0] == '-') {
                threeDashCounter++;

                if (threeDashCounter == 8) {
                    //4 section header with "--- ......  ---" in each = 8
                    indexOfDataSection = i + 3; //skip CRLF(2) and move to next(1) = (2+1) = 3
                }
            }
        }

        if (indexOfDataSection == -1) {
            throw new InvalidOperationException("Not found DAT section, possible incorrect file");
        }

        var cffFileStrings = Encoding.UTF8.GetString(loadedAsArrayByte, 0, indexOfDataSection)
                                     .Split(new[] {GlobalSettings.NewLine, "\n"}, StringSplitOptions.None);

        var indexInCff = 0;

        if (!cffFileStrings[indexInCff].Contains("type: CFG")) {
            throw new InvalidOperationException("First line must contains \"file type: CFG\"");
        }

        indexInCff++;

        while (!cffFileStrings[indexInCff].Contains("type: INF")) {
            cfgSection.Add(cffFileStrings[indexInCff]);
            indexInCff++;
        }

        //ignore INF and HDR section
        while (!cffFileStrings[indexInCff].Contains("type: DAT")) {
            //forward Index to DAT section
            indexInCff++;
        }

        Configuration = new ComtradeConfiguration(cfgSection.ToArray());

        if (Configuration.DataFileType == DataFileType.ASCII) {
            var dataSectionStr = Encoding.UTF8.GetString(loadedAsArrayByte, indexOfDataSection, loadedAsArrayByte.Length - indexOfDataSection)
                                         .Split(new[] {GlobalSettings.NewLine, "\n"}, StringSplitOptions.None);

            Data = new ComtradeData(dataSectionStr.ToArray(), Configuration);
        }
        else {
            var dataSectionByte = loadedAsArrayByte[indexOfDataSection..];
            Data = new ComtradeData(dataSectionByte, Configuration);
        }
    }

    /// <summary>
    ///     Get common for all channels set of timestamps
    /// </summary>
    /// <returns>
    ///     If guided by samplingFrequency in nanoSecond
    ///     Else in microSecond or nanoSecond depending on cfg datetime stamp (6 or 9 Second digit)
    ///     Use TimeLineResolution property for get information about
    /// </returns>
    public IReadOnlyList<double> GetTimeLine()
    {
        var list = new double[Data.Samples.Length];

        if (Configuration.SamplingRateCount == 0 ||
            Math.Abs(Configuration.SampleRates[0].SamplingFrequency) < 0.01d) {
            //use timestamps in samples
            for (var i = 0; i < Data.Samples.Length; i++) {
                list[i] = Data.Samples[i].Timestamp * Configuration.TimeMultiplicationFactor;
            }
        }
        else {
            //use calculated by samplingFrequency
            double currentTime = 0;
            var sampleRateIndex = 0;
            const double secondToNanoSecond = 1000000000;

            for (var i = 0; i < Data.Samples.Length; i++) {
                list[i] = currentTime;

                if (i >= Configuration.SampleRates[sampleRateIndex].LastSampleNumber) {
                    sampleRateIndex++;
                }

                currentTime += secondToNanoSecond / Configuration.SampleRates[sampleRateIndex].SamplingFrequency;
            }
        }

        return list;
    }

    /// <summary>
    ///     Return sequence of values chosen analog channel
    /// </summary>
    public IReadOnlyList<double> GetAnalogPrimaryChannel(int channelNumber)
    {
        double Kt = 1;

        if (Configuration.AnalogChannelInformationList[channelNumber].IsPrimary == false) {
            Kt = Configuration.AnalogChannelInformationList[channelNumber].Primary /
                 Configuration.AnalogChannelInformationList[channelNumber].Secondary;
        }

        var list = new double[Data.Samples.Length];

        for (var i = 0; i < Data.Samples.Length; i++) {
            list[i] = (Data.Samples[i].AnalogValues[channelNumber] * Configuration.AnalogChannelInformationList[channelNumber].MultiplierA +
                       Configuration.AnalogChannelInformationList[channelNumber].MultiplierB) * Kt;
        }

        return list;
    }

    /// <summary>
    ///     Return sequence of values chosen digital channel
    /// </summary>
    public IReadOnlyList<bool> GetDigitalChannel(int channelNumber)
    {
        var list = new bool[Data.Samples.Length];

        for (var i = 0; i < Data.Samples.Length; i++) {
            list[i] = Data.Samples[i].DigitalValues[channelNumber];
        }

        return list;
    }
}
