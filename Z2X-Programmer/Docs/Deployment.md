# Deployment

## Introduction
This document documents all the steps required to create a release.

## Step-by-step guide

| Work step number  | Description |
| ------------- | ------------- |
| 1.  | Preparation of the CreateSetup.bat file. Enter the new version number in the SETUP_VERSION variable. |
| 2.  | Preparation of the Readme.txt file. Update the content of the Readme.txt |



## Installation problems
### The Android APK file cannot be installed - Error type 3

This problem occurs when the APK signed by Visual Studio is to be replaced by the APK signed by Z2X-Progammer.
The following error message appears:

`Error type 3
Error: Activity class {com.peterk78.z2xprogrammer/crc64f7e7738b6d0cb357.MainActivity} does not exist.
Failed to launch android application`

To solve the problem, execute the following commands:

* Open the ADB command prompt: `Tools > Android > Android Adb Command Prompt`
* Enter the following command: `adb uninstall com.peterk78.z2xprogrammer`










