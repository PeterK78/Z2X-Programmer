------------------------------------------------------------------
File:         Readme.txt
Version:      0.1.2.0-beta.6
Date:         22.12.2024
Description:  Z2X-Programmer
------------------------------------------------------------------

Below you will find a brief description and version
information on the software supplied.

------------------------------------------------------------------
Version 0.1.2.0-beta.6 / 22.12.2024
------------------------------------------------------------------
Note:

  A detailed list of changes can be found in the Z2X programmer
  repository on GitHub.

* Sixth beta version (Beta Version 6, V0.1.2.0)

New features:

* Modified CVs are now displayed in detail before a download.

* The current values of the CVs can now be displayed for each
  setting. Note: Not all settings have been expanded yet, the
  work is still being carried out.Ongoing process.

* More detailed error messages if the digital command station
  cannot be reached.

Bug fixes:

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
Copyright (C) 2024
PeterK78
https://github.com/PeterK78/Z2X-Programmer
------------------------------------------------------------------