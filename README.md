# TID Station

<div align="center">

![TID Station Logo](tidlogo.png)

**A desktop radio programming & control application for the 1090-series (H3/H8) handheld radios.**

[![Release](https://img.shields.io/github/v/release/nicsure/1090Station-2?style=flat-square)](https://github.com/nicsure/1090Station-2/releases/latest)
[![Platform](https://img.shields.io/badge/platform-Windows-blue?style=flat-square&logo=windows)](https://github.com/nicsure/1090Station-2/releases/latest)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)](LICENSE)

</div>

---

## Overview

TID Station is a Windows desktop application that gives you full control over compatible 1090-series handheld radios (H3, H8) over a serial/USB connection. It lets you read and write the radio's EEPROM, manage channel memories, adjust radio settings, flash custom firmware, and monitor live signal strength — all from a sleek, dark-themed UI.

> **⚠️ Use at your own risk.** Writing incorrect data to your radio's EEPROM or flashing bad firmware can permanently damage it. Always back up your radio's configuration before making changes.

---

## Features

| Feature | Description |
|---|---|
| 📡 **Dual VFO Control** | Independently control VFO A and VFO B — frequency, tone, power, bandwidth, step size, and more |
| 📖 **Channel Memory Manager** | View, edit, and program up to 199 memory channels with full CTCSS/DCS tone support |
| 📶 **TX Power Calibration** | Fine-tune transmit power levels across all supported frequency bands |
| 📻 **Tuner / Settings Panel** | Configure squelch, backlight, battery saver, roger beep, dual watch, and many more radio settings |
| 📊 **Spectrum Analyser** | Real-time spectrum scope with configurable step count and frequency range |
| ⚡ **Firmware Flasher** | Flash custom or patched firmware binaries directly from the app |
| 💾 **Config Save / Load** | Export your radio's full configuration to a `.cfg` file and reload it any time |
| 🔄 **Live Mode** | Sync changes to the radio in real time over serial |
| 🎛️ **Modulation Override** | Switch between FM / AM / USB modulation on the fly |
| ⎇ **Shift Mode** | Enable frequency offset / repeater shift |
| 🔑 **On-screen Keypad** | Simulate radio keypad button presses from the desktop |

---

## Requirements

### To Run (Download the release)

- **Windows 10 or later** (64-bit)
- A compatible **1090-series radio** (H3, H8) with a USB programming cable
- The appropriate **USB-to-serial driver** for your cable (usually CH340 or CP2102)

No other software is required — the release download is a **self-contained single `.exe` file**.

### To Build from Source

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community edition is free) with the **".NET desktop development"** workload installed
- Windows (WPF is Windows-only)

---

## Getting Started

### Option A — Download the Pre-built Release (Recommended)

1. Go to the [**Releases**](https://github.com/nicsure/1090Station-2/releases/latest) page.
2. Download **`TIDStation.exe`** from the latest release.
3. Run `TIDStation.exe` — no installation needed.

> **SmartScreen warning:** Windows may warn that the file is from an unknown publisher. Click **"More info" → "Run anyway"** to proceed.

---

### Option B — Build from Source

#### Step 1 — Clone the repository

```bash
git clone https://github.com/nicsure/1090Station-2.git
cd 1090Station-2
```

#### Step 2 — Build using Visual Studio

1. Open `TIDStation.sln` in **Visual Studio 2022**.
2. Set the configuration to **Release** (dropdown at the top).
3. Click **Build → Build Solution** (or press `Ctrl+Shift+B`).
4. The output file will be at:
   ```
   TIDStation\bin\Release\net8.0-windows\TIDStation.exe
   ```

#### Step 3 — Build using the .NET CLI (command line)

```bash
# Standard build (requires .NET runtime on the target machine)
dotnet build TIDStation/TIDStation.csproj -c Release

# Self-contained single-file publish (no runtime needed on target machine)
dotnet publish TIDStation/TIDStation.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

The single-file executable will be in the `./publish` folder.

---

## Connecting Your Radio

1. Power **off** your radio.
2. Plug in the **USB programming cable** to the radio and your PC.
3. Install the **USB driver** for your cable if Windows doesn't do it automatically:
   - CH340 chip: [CH340 Driver](https://www.wch-ic.com/downloads/CH341SER_EXE.html)
   - CP2102 chip: [CP2102 Driver](https://www.silabs.com/developers/usb-to-uart-bridge-vcp-drivers)
4. Open **Device Manager** and note which COM port your cable appears on (e.g. `COM3`).
5. Power **on** your radio in **programming mode** (hold the appropriate button while powering on — refer to your radio's manual).
6. Launch **TIDStation**.
7. In the left panel, click the **COM port label** and select your port from the menu.

---

## Using the Application

### Radio Mode (📡)
The main view. Shows Dual VFO displays (A and B) and the S-meter / RSSI bar.

- **Left-click the frequency** to enter a new frequency directly using the keyboard or on-screen keypad.
- **Up / Down arrow keys** tune in steps.
- **Tab key** switches between VFO A and VFO B.
- **Space bar** activates PTT (push-to-talk) to transmit.
- **Right-click labels** (BW, PWR, TONE etc.) to get context menus with options.

### Channel Mode (📖)
Manage memory channels. Select channels in the grid and right-click for bulk-edit options including frequency, tone, power, and more.

### Power Mode (📶)
Adjust the transmit power calibration table across all frequency bands. Use **Reset**, **Revert**, and **Apply** buttons.

### Tuner / Settings Mode (📻)
Configure global radio settings: squelch, backlight, battery save, roger beep, dual watch, VOX, tail tone, and many more.

### Spectrum Analyser (📊)
Click the chart icon to activate the spectrum scope. Set the number of steps (10–200), then click **START** for a continuous sweep or **ONCE** for a single pass.  
Click any bar in the chart to jump VFO A to that frequency.

### Flash Mode (⚡)
Flash firmware to the radio. Browse for a `.bin` firmware file, then click **Flash**. The app automatically applies any enabled patches (see `Patches.cs`) before writing.  
Press **ESC** at any time to abort a flash in progress.

### Saving & Loading Config
- 💾 **Save** — exports the full EEPROM image to a `.cfg` file.
- 📂 **Load** — imports a previously saved `.cfg` file. If connected, the radio will be updated immediately.
- 🡇 **Download from radio** — reads the EEPROM from a connected radio.
- 🡅 **Upload to radio** — writes the current config to a connected radio.

---

## Firmware Patches

Custom firmware patches are stored as Intel HEX data. The app ships with a built-in patch:

| Patch | Description |
|---|---|
| `240606: TIDStation Patch 0.37.1b` | Main patch — enables S-meter, spectrum scope, modulation override, key simulation, and shift mode |

Additional `.hex` files placed in the root of the repository are automatically loaded at startup and can be toggled in the Flash mode patch list.

---

## Project Structure

```
1090Station-2/
├── TIDStation/              # Main WPF application
│   ├── Data/                # Application state & data bindings (Context.cs)
│   ├── Firmware/            # Firmware patch loader and iHEX parser (Patches.cs)
│   ├── General/             # Utilities and background task helpers
│   ├── Radio/               # Channel & power level data models
│   ├── Resources/           # Embedded resources (BLANK.BIN EEPROM template)
│   ├── Serial/              # Serial port communication (Comms.cs)
│   ├── UI/                  # Custom WPF controls (frequency display, bar graph, etc.)
│   ├── View/                # WPF view helpers
│   ├── MainWindow.xaml      # Main UI layout
│   └── TIDStation.csproj    # Project file (.NET 8, WPF)
├── TIDStation.sln           # Visual Studio solution file
├── *.BIN                    # Firmware binary files
├── *.hex / *.asm            # Assembly patches and hex files
└── README.md
```

---

## Troubleshooting

| Problem | Solution |
|---|---|
| Radio not detected | Make sure the USB driver is installed and the radio is in programming mode |
| Wrong COM port | Check Device Manager for the correct port number |
| Checksum error on read | Try a different cable; cheap cables can cause data corruption |
| App won't start | Ensure you're on Windows 10+ 64-bit; try running as Administrator |
| SmartScreen blocks the .exe | Click "More info" → "Run anyway" |
| Flash aborted / fails | Make sure the radio is in bootloader/flash mode (power on while holding the correct button) |

---

## Disclaimer

This software is provided **as-is**, without any warranty. The author is not responsible for any damage to your radio or equipment. Always make a backup of your radio's EEPROM before making any changes.

---

## Credits

Built by **nicsure** — 2024  
Version **v0.37.1b**
