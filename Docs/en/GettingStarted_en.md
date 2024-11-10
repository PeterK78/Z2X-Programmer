# Getting started 
Documentation on the first steps with Z2X-Programmer.

This guide is divided into three sections:

1. [Connecting the Z21 digital command station](#1-connecting-the-z21-digital-command-station)
2. [Reading out a decoder](#2-reading-out-a-decoder)
3. [Search and find the required setting](#3-search-and-find-the-required-setting)
4. [Transfering a new configuration](#4-transferring-a-new-configuration)

## 1. Connecting the Z21 digital command station

* Make sure that your Z21 digital command station can be reached via the network.
* Open the tab `Settings`:

![Open the “Settings” tab"](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedSettings.png)

* Enter the IP address of your Z21 digital command station in the IP address field:

![Enter the IP address of your Z21 digital command station](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedEnterIPAddress.png "Enter the IP address of your Z21 digital command station")

* Test the connection by clicking on the `Test connection` button:

![Test the connection](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedTestConnection.png "Test the connection")

* If no connection can be established, you will receive the following message:

![No connection with Z21 command station](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedConnectionFailed.png "No connection with Z21 command station")

In this case, check whether you have entered the correct IP address and whether the Z21 command station can be reached.

* If a connection has been established, you will receive the following message:
  
![Connection successfully established](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedConnectionSuccess.png "Connection successfully established")

* If the connection has been successfully established, the current operating mode of the Z21 digital central unit is displayed at the top left of the window:

![Connection successfully established](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedOperatingMode.png "Connection successfully established")

## 2. Reading out a decoder

* Select the desired programming method: `Main track` or `Programming track`.

![Reading out a decoder](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedSelectProgramMethod.png "Reading out a decoder")

* If you have selected the `Main track` programming method, you must now enter the vehicle address of the decoder or locomotive. If you have selected the programming method `Programming track`, the vehicle address is not required for the readout.

![Enter the address](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedSetAddress.png "Enter the address")

* Now click on the `Read decoder` button:

![Read decoder](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedUploadData.png "Read decoder")

* Wait until all data has been read from the decoder:

![Wait until the decoder has been read out](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedWaitForUploadComplete.png "Wait until the decoder has been read out")

* The decoder has been successfully read out:

![The decoder was successfully read out](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedUploadComplete.png "The decoder was successfully read out")

* Z2X-Programmer tries to recognize the decoder during readout:

![The type of decoder](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedTypeOfDecoder.png "The type of decoder")

* The decoder has been successfully read out and can now be configured.

## 3. Search and find the required setting

* Z2X-Programmer organizes all settings into tabs. These tabs can be called up via the flyout menu:

![Das Flyout-Menü](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedFlyoutMenu.png "Das Flyout-Menü")

* Nevertheless, it is sometimes difficult to find the desired function on the various tabs. In this case, use the `Search` function.
* To do this, open the `Search` tab via the flyout menu and search for the term “Driving direction”.
* Select the result “Direction of travel inverted (CV29.0)”.

![Search by direction of travel](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedSearchDriveDirection.png "Search by direction of travel")

* Z2X-Programmer now switches directly to the desired function.

![The required function](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-DriveDirectionFound.png "The required function")
  
## 4. Transferring a new configuration

* Change any setting, e.g. reverse the direction of travel:

![Reverse direction of travel]( https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedInvertDriveDirection.png "Reverse direction of travel")

* You can then transfer the new settings to the decoder using the `Write modified values to decoder` function:

![Describe decoder](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedDownloadData.png "Describe decoder")

* Before the download, Z2X-Programmer informs you which configuration variables will be changed. Click on `Yes` to start transferring the settings.

![The decoder will be configured](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedSummary.png "The decoder will be configured")

* If the download was successful, the following message appears:

![The decoder has been successfully configured](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedDownloadSuccess.png "The decoder has been successfully configured")

  

  








  











  
