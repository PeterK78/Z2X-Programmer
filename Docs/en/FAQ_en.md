# FAQ
The following questions are addressed in this document.

1. [Which CVs are read from the decoder?](#which-cvs-are-read-from-the-decoder)
2. [ Which CVs are written to the Z2X file?](#which-cvs-are-written-to-the-z2x-file)

## Which CVs are read from the decoder?
Z2X-Programmer only reads those CVs that are known to Z2X-Programmer. For this reason, Z2X-Programmer creates a list of known CVs before each readout. To do this, Z2X-Programmer opens the selected decoder specification file. All supported features are now collected and the CVs required for them are determined.

The CVs required for a function are described on the following page:

https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/SupportedDecoderFeatures_en.md

Once all the required CVs have been recognized, the decoder is read out.

>[!NOTE]
> All known CVs can be viewed on the `Expert functions` page:
>
> ![https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/InstallationInstructions_en.md](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-ExpertFunctions-AllCVs.png)

## Which CVs are written to the Z2X file?
Z2X-Programmer only writes those CVs to a Z2X file that are known to Z2X-Programmer. For this reason, Z2X-Programmer creates a list of known CVs. To do this, Z2X-Programmer opens the selected decoder specification file. All supported features are now collected and the CVs required for them are determined.

The CVs required for a function are described on the following page:

https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/SupportedDecoderFeatures_en.md

Once all the required CVs have been recognized, the Z2X file is written.

>[!NOTE]
> You can view the content of a Z2X file at any time with a simple text editor (e.g. Notepad). Z2X files are simply XML files:
> ![Z2X-Programmer-Z2X-Textfile.png](https://github.com/PeterK78/Z2X-Programmer/blob/master/Docs/en/Assets/Z2X-Programmer-Z2X-Textfile.png)


