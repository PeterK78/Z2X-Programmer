# Getting started 
Documentation on the first steps with Z2X-Programmer.

This guide is divided into three sections:

1. [Connecting the Z21 digital command station](#1-connecting-the-z21-digital-command-station)
2. [Auslesen eines Decoders](#2-auslesen-eines-decoders)
3. [Suchen und Finden der benötigten Einstellung](#3-suchen-und-finden-der-benötigten-einstellung)
4. [Übertragen einer neuen Konfiguration](#4-übertragen-einer-neuen-konfiguration)

## 1. Connecting the Z21 digital command station

* Make sure that your Z21 digital command station can be reached via the network.
* Open the tab `Settings`:

![Open the “Settings” tab"](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedSettings.png)

* Enter the IP address of your Z21 digital command station in the IP address field:

![Enter the IP address of your Z21 digital command station](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedEnterIPAddress.png "Enter the IP address of your Z21 digital command station")

* Test the connection by clicking on the `Test connection` button:

![Test the connection](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedTestConnection.png "Test the connection")

* If no connection can be established, you will receive the following message:

![No connection with Z21 command station](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Z2X-Programmer-GettingStartedConnectionFailed.png "No connection with Z21 command station")

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

* Z2X-Programmer tries to recognize the decoder during readout. :

![The type of decoder](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-GettingStartedTypeOfDecoder.png "The type of decoder")

## 3. Suchen und Finden der benötigten Einstellung

* Z2X-Programmer organisiert alle Einstellungen in logische Gruppen. Diese Gruppen können über das Flyout-Menü aufgerufen werden:
![Das Flyout-Menü](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/De/Assets/Z2X-Programmer-GettingStartedFlyoutMenu.png "Das Flyout-Menü")

* Trotzdem ist es manchmal schwierig, die gewünschte Funktion auf den unterschiedlichen Registerkarten zu finden. In diesem Fall verwenden Sie die Funktion `Suche`.
* Öffnen Sie dazu die Registerkarte `Suche` über das Flyout-Menü und suchen Sie den Begriff "Fahrrichtung":
![Suche nach Fahrtrichtung](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/De/Assets/Z2X-Programmer-GettingStartedSearchDriveDirection.png "Suche nach Fahrtrichtung")

* Wählen Sie das Ergebnis "Fahrtrichung invertiert (CV29.0)" aus.
* Z2X-Programmer wechselt nun direkt zur gewünschten Funktion.

## 4. Übertragen einer neuen Konfiguration

* Ändern Sie eine beliebige Einstellung. z. B. kehren Sie Fahrtrichtung um:

![Fahrtrichtung umkehren]( https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/De/Assets/Z2X-Programmer-GettingStartedInvertDriveDirection.png "Fahrtrichtung umkehren")

* Anschließend können Sie die neuen Einstellungen mit der Funktion `Decoder beschreiben` auf den Decoder übertragen:

![Decoder beschreiben](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/De/Assets/Z2X-Programmer-GettingStartedDownloadData.png "Decoder beschreiben")

* Vor dem Download informiert Sie Z2X-Programmer welche Konfigurationsvariablen geändert werden. Klicken Sie auf `Ja` um das Übertragen der Daten zu starten.

![Decoder beschreiben](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/De/Assets/Z2X-Programmer-GettingStartedSummary.png "Decoder beschreiben")

* Wurde der Download erfolgreich durchgeführt, so erscheint folgende Meldung:

![Decoder erfolgreich](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/De/Assets/Z2X-Programmer-GettingStartedDownloadSuccess.png "Decoder erfolgreich")

  

  








  











  
