# Serial

VB.NET WinForms host that opens a serial port, parses `acx`/`acy`/`acz` accelerometer tokens (and other tokens such as `li0`/`tc0`), subtracts XYZ offsets from My.Settings, and drives analog gauges plus PerfChart traces. Visual Studio 2010 solution `Serial.sln` builds the WinExe with the VB.NET Gauge control (`AVBGuage`, folder name misspelled) and the PerfChart library. The tree also keeps A.J.Bauer's C# AGauge analog gauge and a second VB Gauge project (`Gauge/GaugeApp.vbproj`) that are not listed in the sln.

**Source last updated:** 2013-01-01  
**Language:** VB.NET + C#  
**Target:** Visual Studio 2010 / .NET 3.5  
**Output:** WinExe + libraries

## Solution structure

| Project | Language | Path | In `Serial.sln` | Type |
|---------|----------|------|-----------------|------|
| `Serial` | VB.NET | `Serial/Serial.vbproj` | yes | WinExe |
| `Gauge` | VB.NET | `AVBGuage/Gauge.vbproj` | yes | Library (folder name misspelled AVBGuage) |
| `PerfChart` | VB.NET | `PerfChart/PerfChart.vbproj` | yes | Library |
| `AGauge` | C# | `AGauge/AGauge.csproj` | no | Library (A.J.Bauer analog gauge) |
| `GaugeApp` | VB.NET | `Gauge/GaugeApp.vbproj` | no | Library |

## How to open

Open `Serial.sln` in Visual Studio 2010 (or later with VB.NET). That solution builds Serial, the AVBGuage Gauge control, and PerfChart. AGauge and `Gauge/GaugeApp.vbproj` are in the tree but are not solution members.

## Attribution and provenance

Dave Robinson / VaderConsulting authored the Serial host. AGauge is A.J.Bauer's C# analog gauge (Copyright (C) 2007 A.J.Bauer); keep the zlib-style notice in `AGauge/AGauge.cs`. AssemblyInfo Company/Copyright "Microsoft 2010" on the VS 2010 projects is a project-template leftover, not a Microsoft product. See `THIRD_PARTY_NOTICES.md`.

## License

MIT License for Dave's Serial code. Copyright (c) 2026 VaderConsulting. See `LICENSE`. AGauge remains under A.J.Bauer's zlib-style terms in `AGauge/AGauge.cs`. See `THIRD_PARTY_NOTICES.md`.
