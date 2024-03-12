# Comtrade Handler

![NuGet Version](https://img.shields.io/nuget/v/ComtradeHandler)

A C# library that allows to read and write Comtrade (IEEE C37.111 / IEC 60255-24) files.

The base for the code was taken from [Wisp.Comtrade](https://github.com/Esticonv/Wisp.Comtrade) (MIT). All credits to [Esticonv](https://github.com/Esticonv).

The base repository was (I thought) inactive and was .NetFramework4 compatible only (2020). I required .NetCore3 so I migrated it for personal use. 
The source repository has added some changes since then, and is currently on version v0.9.5 (2022).
These (v0.9.5) changes have been migrated to this project as well.
The main difference with the original repo, is that this repository uses `xUnit` and the code has been reformatted with my own `.editorConfig` rules.
I also added some tests with some sample Comtrade files from [this repo](https://github.com/dparrini/python-comtrade).

This repository has the following features:
- Supports [1991](https://standards.ieee.org/ieee/C37.111/2644/), [1999](https://standards.ieee.org/ieee/C37.111/2645/) and [2013](https://standards.ieee.org/ieee/C37.111/3795/) revisions.
- Supports ASCII and Binary files.
- Supports `.cfg`, `.dat` and `.cff` files. Does not support `.hdr` files.

## Installation
You can get the library from Nuget via
``` nuget
> dotnet add package ComtradeHandler
```

## Usage

There are some tests that show how to read and write the COMTRADE files. \
Both `ComtradeHandler.UnitTests\RecordReaderTest.cs` and `ComtradeHandler.UnitTests\RecordWriterTest.cs` are good starting points to see how the code works. \
Drop a message or issue if you need help. (:

## Contribution

Project is MIT, so all contributions are welcome !
