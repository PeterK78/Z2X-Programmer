@ECHO OFF

SET SETUP_VERSION=V0.1.2.0beta3
SET SETUP_RELEASE_FOLDER=Release\

SET SETUP_FILENAME_WINDOWS=Z2X-Programmer-Win11-%SETUP_VERSION%.zip
SET SETUP_FILENAME_ANDROID=Z2X-Programmer-Android-%SETUP_VERSION%.zip

SET SETUP_PATH_WINDOWS=%SETUP_RELEASE_FOLDER%%SETUP_FILENAME_WINDOWS%
SET SETUP_PATH_ANDROID=%SETUP_RELEASE_FOLDER%%SETUP_FILENAME_ANDROID%

SET SOURCE_PATH_ANDROID=Release\SignedAPK\com.peterk78.z2xprogrammer.apk
SET SOURCE_PATH_WINDOWS=..\bin\Release\net8.0-windows10.0.19041.0\win10-x64\*

CLS

ECHO:
ECHO:
ECHO:
ECHO:
ECHO:
ECHO:
ECHO:
ECHO:
ECHO:
ECHO ***********************************************************
ECHO * Z2X-Programmer: Setup creator                           *
ECHO ***********************************************************
ECHO:
ECHO Version: %SETUP_VERSION%
ECHO:

if not exist %SOURCE_PATH_ANDROID% (

		ECHO  ERROR: Android APK file is missing.
		ECHO  Missing file: %SOURCE_PATH_ANDROID%
		ECHO  Please run a Release build to create the APK file.
		exit  /b;
) 

ECHO Step 1/5: Setup the release folder ...                       
IF NOT EXIST %SETUP_RELEASE_FOLDER% MKDIR %SETUP_RELEASE_FOLDER%

ECHO Step 2/5: Create the Windows ZIP archive ...
powershell -command Compress-Archive %SOURCE_PATH_WINDOWS% %SETUP_PATH_WINDOWS% -Force

ECHO Step 3/5: Add the Readme.txt ...
powershell -command Compress-Archive Readme.txt %SETUP_PATH_WINDOWS% -Update

ECHO Step 4/5: Create the Android ZIP archive...
powershell -command Compress-Archive %SOURCE_PATH_ANDROID% %SETUP_PATH_ANDROID% -Force

ECHO Step 5/5: Add the Readme.txt ...
powershell -command Compress-Archive Readme.txt %SETUP_PATH_ANDROID% -Update

ECHO Finish