------------------------------------------------------------------
File:         Readme.txt
Version:      0.1.2.0-beta.10
Date:         16.04.2025
Description:  Z2X-Programmer
------------------------------------------------------------------

Below you will find a brief description and version
information on the software supplied.

------------------------------------------------------------------
Version 0.1.2.0-beta.10 / 16.04.2025
------------------------------------------------------------------

Note:

* Tenth beta version (Beta Version 10, V0.1.2.0)

New features:

1. A controller has been added.
2. A note is shown, if CVs are disabled during up- and download.
3. A new button "Enable all CVs" has been added to the
   "Expert functions" page.
4. The feature RCN225_DIRECTION_CV29_0 for ZIMO FX decoder has
   been added.
5. The layout of the function keys page has been updated.
6. The feature ZIMO_FUNCKEY_SHUNTINGKEY_CV155 has been added.
7. The feature ZIMO_MXBRIGHTENINGUPANDIMMINGTIMES_CV190X has
   been added.
8. The feature ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X
   has been added.

Bug fixes:

1. The configuration of the mute function key no longer triggers
   the Undo Manager.

------------------------------------------------------------------
Version 0.1.2.0-beta.9 / 15.03.2025
------------------------------------------------------------------

Note: 

* Ninth beta version (Beta Version 9, V0.1.2.0)

New features:

1. A decoder specification for  Doehler & Haass PD series decoder
   has been added.

2. Z2X projects are automatically saved when the application
   is closed.

Bug fixes:

1. Sporadically, the sliders for CV3 and CV4 were set to the
   value 1 after reading out a decoder.

------------------------------------------------------------------
Version 0.1.2.0-beta.8 / 25.02.2025
------------------------------------------------------------------

Note:

  A detailed list of changes can be found in the Z2X programmer
  repository on GitHub.

* Eighth beta version (Beta Version 8, V0.1.2.0)

New features:

1. The extended toolbar with the commands “Save as” and
   “Report a bug” has been added.

2. The feature ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X has been
   added.

3. The feature ZIMO_MXFXFUNCTIONKEYMAPPING_CV3346 has been added.

4. The feature ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X has
   been added.

5. The search database has been updated.

Bug fixes:

1. Fixed a bug with the feature ZIMO_LIGHT_EFFECTS_CV125X and
   the effect "Double puls strobe" which was not read correctly.

2. Sound function keys were incorrectly displayed for ZIMO MN
   decoders. The problem has been fixed.

------------------------------------------------------------------
Version 0.1.2.0-beta.7 / 05.01.2025
------------------------------------------------------------------
Note:

  A detailed list of changes can be found in the Z2X programmer
  repository on GitHub.

* Seventh beta version (Beta Version 7, V0.1.2.0)

New features:

* Undo and Redo has been added.

* Added a decoder specification for ZIMO MN decoder.

* User specific directory for decoder specifications.

* The feature ZIMO_LIGHT_EFFECTS_CV125X has been updated.

* The locomotive list now shows whether a Z2X file is available.

* The track voltage can now also be switched on and off
  using the “Connection State” button.

Bug fixes:

* Sporadically, a non-valid vehicle address was selected after
  changing the DCC address mode.

* The direction in consist mode was sporadically not displayed
  correctly.

* In some cases, the values for CV3 and CV4 were not displayed
  correctly in the GUI.

* The layout of the info page has been updated.

------------------------------------------------------------------
Version 0.1.2.0-beta.6 / 05.01.2025
------------------------------------------------------------------
Note:

  A detailed list of changes can be found in the Z2X programmer
  repository on GitHub.

* Sixth beta version (Beta Version 6, V0.1.2.0)

New features:

* Modified CVs are now displayed in detail before a download.

* The current values of the CVs can now be displayed for each
  setting.

* More detailed error messages if the digital command station
  cannot be reached.

* A “Getting started” dialog has been added.

Bug fixes:

* Older Z2X projects without names for function outputs lead
  to a crash.

* The ZIMO-specific extended assignment of the function outputs
  - without left shift (CV61) was sporadically activated
  incorrectly.

* The status of the ZIMO light effects was not read out correctly,
  also the GUI did not react correctly to the
  feature ZIMO_LIGHT_EFFECTS_CV125X.

* The values for feature ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57
  were sporadically overwritten by the values of the feature
  ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57.

* The extended speed curve values are now written to the
  decoder.

* The default maximum speed switch did not work properly.

* The automatic motor reference value switch for ZIMO MS
  decoder did not work properly.

* Receiving a Rocrail locomotive list can sometimes take a
  long time.

* The names of some function outputs could not be set
  correctly.

* GUI elements for RCN225_DIRECTION_CV29_0 were visible
  although this feature was not activated.

Technical improvements:

* Upgraded to MAUI Community Toolikt V10.0.

------------------------------------------------------------------
Version 0.1.2.0-beta.5 / 08.12.2024
------------------------------------------------------------------
Note:

  A detailed list of changes can be found in the Z2X programmer
  repository on GitHub.

* Fifth beta version (Beta Version 5, V0.1.2.0)

New features:

1. Only those variables that are supported by the selected decoder
   specification are now displayed in the "Configuration
   variables" "table on the "Expert functions" page. This allows
   you to quickly see which configuration variables Z2X-Programmer
   is processing.

2. Configuration variables can now be activated and deactivated on
   the “Expert functions” page. This new function can be used if a
   certain configuration variable cannot be read out on a recurring
   basis.

2. Zimo FX function decoders now also support ZIMO_LIGHT_DIM_CV60
   and ZIMO_LIGHT_EFFECTS_CV125X light features. Function outputs
   can now be dimmed and assigned effects.

4. The icons for upload and download have been updated. The new
   icons should show the underlying functions more clearly.

5. A default directory for Z2X files is created during the
   installation. This means that the directory no longer has
   to be created manually after the start.

6. A setting for deactivating the cancelation of an upload
   if a configuration variable cannot be read has been added.
   For experts only! Only use this function if you have
   reading problems with your decoders.

7. The list of known decoder manufacturers has been extended.

8. The configuration variables read or written are displayed
   during the upload and download.

9. The feature ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X has
   been added.

Bug fixes:

1. The description of the decoder specification for Zimo FX
   function decoders has been corrected.
2. Minor updates for the input field of second addresses
   for function decoders.
3. The Zimo MX671 function decoder has been removed from
   the decoder specification for Zimo MX sound decoder.
4. The name of the Zimo MX671 decoder has been added
   to the Zimo FX function decoder specification.
5. The UDP client is no longer bound to the local UDP
   port 21105.
6. The feature “RCN225_DECODERRESET_CV8” was noted
   as “Safe to write to”. This could lead to unintentional
   decoder resets.
7. The feature "ZIMO_SUBVERSIONNR_CV65" was noted
   as "Safe to write to". This could lead to aborts
   when downloading the settings
   

Technical improvements:

* Switched to Microsoft .NET 9.

------------------------------------------------------------------
Version 0.1.2.0-beta.4 / 10.11.2024
------------------------------------------------------------------

* Fouth beta release (Beta Version 4, V0.1.2.0)

------------------------------------------------------------------
Version 0.1.2.0-beta.3 / 07.11.2024
------------------------------------------------------------------

* Third beta release (Beta Version 3, V0.1.2.0)

------------------------------------------------------------------
Version 0.1.2.0-beta.3 / 24.10.2024 
------------------------------------------------------------------

* Second beta release (Beta Version 2, V0.1.2.0)

------------------------------------------------------------------
Version 0.1.2.0-beta.1 / 30.09.2024 
------------------------------------------------------------------

* First beta release (Beta Version 1, V0.1.2.0)

------------------------------------------------------------------
Z2X-Programmer
Copyright (C) 2025
PeterK78
https://github.com/PeterK78/Z2X-Programmer
------------------------------------------------------------------