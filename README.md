<img src="https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/appiconfg.svg" width="100"/>

# Z2X-Programmer
[![Z2X-Programmer Documentation](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X_Programmer-Badge-Documentation.svg)](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/Documentation.md)
[![Download Z2X-Programmer](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X_Programmer-Badge-Download.svg)](https://github.com/PeterK78/Z2X-Programmer/releases)
[![Download Z2X-Programmer](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X_Programmer-Badge-Download-Dev.svg)](https://github.com/PeterK78/Z2X-Programmer/actions)
[![Installation instructions](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X_Programmer-Badge-InstallationInstruction.svg)](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/InstallationInstructions_en.md)
[![Getting started](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X_Programmer-Badge-GettingStarted.svg)](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/GettingStarted_en.md)
[![FAQ](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X_Programmer-Badge-FAQ.svg)](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/FAQ_en.md)

Z2X-Programmer is an application to configure locomotive and function decoders by using a Roco / Fleischmann Z21 compatible digital command station. 
The application is aimed at beginner level users who want to configure the most essential functions of their decoders in a simple and intuitive way. 

>[!NOTE]
>The application is currently in a beta state.

## Z2X-Programmer in the press
* [Digitale Modellbahn 2-2025](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/DiMo_2025_02_014-015_HH_Z2X-Programmer.pdf)

## Overview

The application provides the following features:

*  Configuration of various addresses
*  Configuration of drive properties
*  Configuration of motor properties
*  Configuration of the function keys
*  Configuration of selected sound functions
*  Configuration of selected lighting functions
*  Display of decoder information such as decoder name, software version, etc.
*  Configuration of the protocols
*  Configuration of security functions
*  Maintenance functions
*  etc.

> A detailed list of all supported functions can be found on the following page: [List of supported decoder features](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/SupportedDecoderFeatures_en.md).

User-friendliness is supported with the following functions:

* The user interface can switch to dark mode
* The user interface language is available in German and English
* A search function helps you to find the functions you need

## Supported decoder manufacturers
Z2X-Programmer is universal and not limited to a specific decoder manufacturer. The application offers a wide range of configuration options that are available for all decoders. Which configuration options are used by a decoder is defined in a decoder specification file. The following decoder specification files are currently available:

* RCN225 compatible decoders
* ZIMO MS sound decoder
* ZIMO MX sound decoder
* ZIMO MX function decoder
* ZIMO MN decoder
* Doehler & Haass PD series
* etc.

You can create your own decoder specification file at any time.

>[!NOTE]
> At the moment the focus is on Z21 digital command stations and ZIMO decoders - this is the hardware I currently own.

## Screenshots
![Changing the motor characteristics](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X-Programmer-MotorCharacteristics.png "Changing the motor characteristics")

![Changing the drive characteristics](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X-Programmer-DriveCharacteristics.png "Changing the drive characteristics")

![Configuring function keys](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X-Programmer-FunctionKeys.png "Configuring function keys")

![Search](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X-Programmer-Search.png "Search")

![Android](https://github.com/PeterK78/Z2X-Programmer/blob/master/Assets/Z2X-Programmer-Android.png "Android")

## Documentation

* [Installation instructions](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/InstallationInstructions_en.md)
* [Getting started](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/GettingStarted_en.md)
* [List of supported decoder features](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/SupportedDecoderFeatures_en.md)
* [Recommended Z21 settings](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/RecommendedZ21Settings_en.md)
* [Locomotive list](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/LocomotiveList_en.md)
* [FAQ](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/FAQ_en.md)

## Notes

* Z2X-Programmer is not intended for experts. Experts are recommended to use the manufacturer-specific programming tools.

## Supported operating systems and languages
* Z2X-Programmer is currently available for Microsoft Windows 11 64Bit and Android.

>[!NOTE]
>A 32-bit version for older Windows versions is also available, but no support is offered for this version.

* Further ports are possible (iOS, MacOS etc.), but not currently planned.
* The software offers multilingual support, but so far only German and English are available.

## Technical stuff

* Z2X-Programmer is build with Microsoft [.NET Multi-platform App UI (.NET MAUI)](https://dotnet.microsoft.com/en-us/apps/maui)

## License

* The original source code in this repository is licensed under the GNU GPLv3 license.
* Microsoft, Z21, Fleischmann, ZIMO and Roco are registered trademarks of their respective companies.


