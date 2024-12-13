/*

Z2X-Programmer
Copyright (C) 2024
PeterK78

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see:

https://github.com/PeterK78/Z2X-Programmer?tab=GPL-3.0-1-ov-file.

*/

using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Runtime.CompilerServices;
using Z21Lib.Events;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.Communication
{
    /// <summary>
    /// This class contains the implementation of all functions to communicate with a command station. Functions like ReadVC, WriteCV are provided.
    /// </summary>
    internal static class ReadWriteDecoder
    {

        static readonly string[,] RCNFeatures = new string[25, 29] {
                                  {DeqSpecReader.RCN225_BASEADDRESS_CV1,"1","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_MINIMALSPEED_CV2, "2","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_ACCELERATIONFACTOR_CV3, "3","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_DECELERATIONFACTOR_CV4, "4", "0", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_MAXIMALSPEED_CV5, "5", "0", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_MEDIUMSPEED_CV6, "6", "0", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_DECODERVERSION_CV7, "7", "0", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_MANUFACTUERID_CV8, "8", "0", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_DECODERLOCK_CV15X, "15","16","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_SPEEDSTEPS_CV29_1, "29", "0", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_SPEEDTABLE_CV29_4, "29", "0", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_LONGSHORTADDRESS_CV29_5, "29", "17", "18","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_RAILCOMENABLED_CV29_3, "29", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0, "28",  "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_RAILCOMCHANNEL2DATA_CV28_1, "28", "0", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_DIRECTION_CV29_0, "29", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X, "67", "68","69","70","71","72","73","74","75","76","77","78","79","80","81","82","83","84","85","86","87","88","89","90","91","92","93","94" },
                                  {DeqSpecReader.RCN225_ANALOGMODE_CV29_2, "29", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_DECODERRESET_CV8, "8", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_FUNCTIONKEYMAPPING_CV3346, "33", "34", "35","36","37", "38", "39", "40", "41","42", "43", "44", "45", "46", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" },
                                  {DeqSpecReader.RCN225_CONSISTADDRESS_CV19, "19", "20", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_AUTOMATICREGISTRATION_CV28_7, "28", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_HLU_CV27_2, "27", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_ABC_CV27_X, "27", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.RCN225_OPERATINGMODES_CV12, "12", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"}
                                };

        static readonly string[,] ZIMOFeatures = new string[31, 29] {
                                  {DeqSpecReader.ZIMO_SUBVERSIONNR_CV65, "65", "0", "0" ,"0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_DECODERTYPE_CV250, "250", "0", "0", "0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_DECODERID_CV25X, "250", "251", "252", "253","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156, "156", "0", "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_LIGHT_DIM_CV60,"60","114","152","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_BOOTLOADER_VERSION_24X,"248","249","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_MXMOTORCONTROLFREQUENCY_CV9,"9", "0", "0" ,"0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57, "57", "0", "0" ,"0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_MXUPDATELOCK_CV144, "144", "0", "0" ,"0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_MXFX_SECONDADDRESS_CV64, "64", "112","67","68" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_MXMOTORCONTROLPID_CV56, "56", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_SOUND_VOLUME_GENERIC_C266, "266", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_BRAKESQUEAL_CV287, "287", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61, "61", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_LIGHT_EFFECTS_CV125X, "125", "126","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396, "396", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397, "397", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_FUNCKEY_SOUNDALLOFF_CV310, "310", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_SELFTEST_CV30, "30", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_FUNCKEY_CURVESQUEAL_CV308, "308", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57, "57", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_SOUND_STARTUPDELAY_CV273, "273", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285, "285", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_SOUND_VOLUME_STEAM_CV27X, "275", "276","283","286" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_SOUND_VOLUME_DIESELELEC_CV29X, "296", "298","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_FUNCKEY_MUTE_CV313, "313", "298","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_MSOPERATINGMODES_CV12, "12", "298","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_SOUNDPROJECTNR_CV254, "254", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_SUSIPORT1CONFIG_CV201, "201", "0","0","0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_INPUTMAPPING_CV4XX, "400", "401","402","403" ,"404","405","406","407","408","409","410","411","412","413","414","415","416","417","418","419","420","0","0","0","0","0","0","0"},
                                  {DeqSpecReader.ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X, "69", "70","71","72" ,"73","74","75","76","77","78","79","80","81","82","0","0","0","0","0","0","0","0","0","0","0","0","0","0"}
                                };

        static readonly string[,] DOEHLERHAASFeatures = new string[6, 29] {
                                  {DeqSpecReader.DOEHLERHAAS_MOTORIMPULSWIDTH_CV49, "49", "0", "0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0" },
                                  {DeqSpecReader.DOEHLERANDHAAS_DECODERTYPE_CV261, "261", "0", "0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0" },
                                  {DeqSpecReader.DOEHLERANDHAAS_FIRMWAREVERSION_CV262x, "262", "264", "0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0" },
                                  {DeqSpecReader.DOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137, "137", "0", "0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0" },
                                  {DeqSpecReader.DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133, "133", "0", "0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0" },
                                  {DeqSpecReader.DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132, "132", "0", "0" ,"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0" }
                              };
        
        //  The name of the decoder specification
        private static string _decSpecName = "";

        //  How to do the programming (POM or Programming Track)
        private static NMRA.DCCProgrammingModes _mode = NMRA.DCCProgrammingModes.POMMainTrack;

        //  If TRUE the application is waiting for an answer of the command station
        private static bool _waitingForResultReceived = false;
        private static bool _commandSuccessFull = false;

        /// <summary>
        /// Constructor    
        /// </summary>
        static ReadWriteDecoder()
        {
            CommandStation.Z21.OnProgramResultReceived += OnProgramResultReceived;            
        }

        /// <summary>
        /// Downloads the configuration settings to the decoder.
        /// </summary>
        /// <param name="cancelToken">A cancel token to cancel the execution.</param>
        /// <param name="vehicleAddress">The vehicle address.</param>
        /// <param name="decSpecName">The decoder specification name.</param>
        /// <param name="mode">The programming mode (NMRA.DCCProgrammingModes).</param>
        /// <param name="progressPercentage">The current progress state in percent.</param>
        /// <param name="allConfigVariables">If TRUE all supported configuration variables will be transfered to the decoder. If FALSE only those for which the current value is different from the backup value are used.</param>
        /// <param name="progressCV">The currently processed CV.</param>
        /// <returns></returns>
        internal static Task<bool> DownloadDecoderData(CancellationToken cancelToken, ushort vehicleAddress, string decSpecName, NMRA.DCCProgrammingModes mode, IProgress<int> progressPercentage, bool allConfigVariables, IProgress<int> progressCV)
        {
            _mode = mode;
            _decSpecName = decSpecName;
            
            List<int> ConfigVariablesToWrite = new List<int>();

            //  We check if we can reach the command station
            if (CommandStation.Connect(cancelToken,5000) == false) { return Task.FromResult(false); }

            //  Turn the track power ON if we are in POM mode
            if (mode == NMRA.DCCProgrammingModes.POMMainTrack) SetTrackPowerON();

            //  If we currently do not have a valid decoder specification, or just the generic one - we try to detect it automatically
            if ((_decSpecName == "") || (_decSpecName == null) || (_decSpecName == DeqSpecReader.GetDefaultDecSpecName()))
            {
                _decSpecName = DetectDecoderDeqSpec(cancelToken, vehicleAddress);
                DecoderSpecification.DeqSpecName = _decSpecName;
            }

            // Create a list of variables which we have to download to the decoder. Depending on the allConfigVariables flag,
            // all variables are used or only those for which the current value is different from the backup value.
            //
            // Note: The functions GetAllConfigurationVariables and GetModifiedConfigurationVariables will provide only CVs which can be safelfy overwritten.
            //
            if (allConfigVariables == true)
            {
                ConfigVariablesToWrite = GetAllWritableConfigurationVariables(decSpecName, mode);
            }
            else
            {
                ConfigVariablesToWrite = GetModifiedConfigurationVariables(decSpecName, mode);
            }

            //  The required configuration variables have now been collected.
            //  Now we can write the configuration variables to the decoder.
            for (int i = 0; i <= ConfigVariablesToWrite.Count - 1; i++)
            {
                //  Before we write a planned configuration variable, we must check whether it is enabled in the current decoder configuration.
                if (DecoderConfiguration.ConfigurationVariables[ConfigVariablesToWrite[i]].Enabled == true)
                {
                    //  We report the configuration variable that will be written next.
                    progressCV.Report(ConfigVariablesToWrite[i]);

                    if (WriteCV((ushort)ConfigVariablesToWrite[i], vehicleAddress, DecoderConfiguration.ConfigurationVariables[ConfigVariablesToWrite[i]].Value, _mode, cancelToken) == false)
                    {
                        CommandStation.Z21.SetTrackPowerOn();
                        return Task.FromResult(false);
                    }
                    else
                    {
                        DecoderConfiguration.ConfigurationVariables[ConfigVariablesToWrite[i]].Enabled = true;
                        DecoderConfiguration.BackupCVs[ConfigVariablesToWrite[i]].Value = DecoderConfiguration.ConfigurationVariables[ConfigVariablesToWrite[i]].Value;
                        DecoderConfiguration.BackupCVs[ConfigVariablesToWrite[i]].Enabled = true;
                    }
                }
              
                //  Reporting the current percentage value.
                int percent = (int)(((double)100 / (double)ConfigVariablesToWrite.Count) * (double)i);
                Logger.PrintDevConsole("DownloadDecoderData: Percentage:" + percent);
                progressPercentage.Report(percent);

                if (cancelToken.IsCancellationRequested == true)
                {
                    CommandStation.Z21.SetTrackPowerOn();
                    return Task.FromResult(false);
                }

            }
            progressPercentage.Report(100);

            // We turn the power on to stop the programming mode
            CommandStation.Z21.SetTrackPowerOn();

            return Task.FromResult(true);


        }

        /// <summary>
        /// Returns a list with configuration variables which can be safely written by the given decoder specification.
        /// </summary>
        /// <returns>Returns a list of configuration variable numbers</returns>
        public static List<int> GetAllWritableConfigurationVariables(string decSpecName, NMRA.DCCProgrammingModes mode)
        {
            List<int> SupportConfigVariables = new List<int>();
            List<int> ModifiedConfigVariables = new List<int>();

            for (int i = 0; i <= RCNFeatures.GetLength(0) - 1; i++)
            {
                if (DeqSpecReader.FeatureSupported(decSpecName, RCNFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == true)
                {
                    //  We check whether we are allowed to describe the configuration variables belonging to the feature.
                    if (DeqSpecReader.IsWriteable(decSpecName, RCNFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == false)
                    {
                        continue;
                    }

                    for (int cvIndex = 1; cvIndex <= RCNFeatures.GetLength(1) - 1; cvIndex++)
                    {
                        ushort nextCV = ushort.Parse(RCNFeatures[i, cvIndex]);
                        if (nextCV != 0)
                        {                         
                            //  Check if we have already written this CV. If so, skip this CV
                            if (SupportConfigVariables.Contains(nextCV) == false)
                            {
                                SupportConfigVariables.Add(nextCV);
                            }
                        }
                    }
                }
            }

            if ((DecoderConfiguration.ConfigurationVariables[8].Value == NMRA.ManufacturerID_Trix) ||
            (DecoderConfiguration.ConfigurationVariables[8].Value == NMRA.ManufacturerID_DoehlerAndHaass))
            {

                for (int i = 0; i <= DOEHLERHAASFeatures.GetLength(0) - 1; i++)
                {
                    if (DeqSpecReader.FeatureSupported(decSpecName, DOEHLERHAASFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == true)
                    {
                        //  We check whether we are allowed to describe the configuration variables belonging to the feature.
                        if (DeqSpecReader.IsWriteable(decSpecName, DOEHLERHAASFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == false) continue;

                        for (int cvIndex = 1; cvIndex <= DOEHLERHAASFeatures.GetLength(1) - 1; cvIndex++)
                        {
                            ushort nextCV = ushort.Parse(DOEHLERHAASFeatures[i, cvIndex]);
                            if (nextCV != 0)
                            {
                                //  Check if we have already written this CV. If so, skip this CV
                                if (SupportConfigVariables.Contains(nextCV) == false)
                                {
                                    SupportConfigVariables.Add(nextCV);
                                }
                            }
                        }
                    }
                }

            }

            if (DecoderConfiguration.ConfigurationVariables[8].Value == NMRA.ManufacturerID_Zimo)
            {
                for (int i = 0; i <= ZIMOFeatures.GetLength(0) - 1; i++)
                {                    
                    if (DeqSpecReader.FeatureSupported(decSpecName, ZIMOFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == true)
                    {
                        //  We check whether we are allowed to describe the configuration variables belonging to the feature.
                        if (DeqSpecReader.IsWriteable(decSpecName, ZIMOFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == false) continue;

                        for (int cvIndex = 1; cvIndex <= ZIMOFeatures.GetLength(1) - 1; cvIndex++)
                        {
                            ushort nextCV = ushort.Parse(ZIMOFeatures[i, cvIndex]);
                            if (nextCV != 0)
                            {
                                //  Check if we have already written this CV. If so, skip this CV
                                if (SupportConfigVariables.Contains(nextCV) == false)
                                {
                                    SupportConfigVariables.Add(nextCV);
                                }
                            }
                        }
                    }
                }
            }

            return SupportConfigVariables;            
        }
  
        /// <summary>
        /// Returns a list with configuration variables which can be read by the given decoder specification.
        /// </summary>
        /// <returns>Returns a list of configuration variable numbers.</returns>
        public static List<int> GetAllReadableConfigurationVariables(string decSpecName)
        {
            List<int> ConfigVariablesToRead = new List<int>();

            //
            //  Read the RCN compatible features
            //
            Logger.PrintDevConsole("ReadWriteDecoder: Assembling RCN compatible features ...");
            for (int i = 0; i <= RCNFeatures.GetLength(0) - 1; i++)
            {
               
                if (DeqSpecReader.FeatureSupported(decSpecName, RCNFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == true)
                {
                    for (int cvIndex = 1; cvIndex <= RCNFeatures.GetLength(1) -1; cvIndex++)
                    {
                        ushort nextCV = ushort.Parse(RCNFeatures[i, cvIndex]);
                        if (nextCV != 0)
                        {
                            //  Check if we have already read this CV. If so, skip this CV
                            if (ConfigVariablesToRead.Contains(nextCV) == false)
                            {
                                ConfigVariablesToRead.Add(nextCV);
                            }
                        }
                    }                                    
                }
            }

            //
            //  Do we have a ZIMO decoder? If yes, read the ZIMO specific features
            //
            Logger.PrintDevConsole("ReadWriteDecoder: Assembling ZIMO compatible features ...");
            if ((DecoderConfiguration.ConfigurationVariables[8].Value == NMRA.ManufacturerID_Zimo) || (DecoderConfiguration.ConfigurationVariables[8].Value == 0))
            {
                for (int i = 0; i <= ZIMOFeatures.GetLength(0) - 1; i++)
                {
                    if (DeqSpecReader.FeatureSupported(decSpecName, ZIMOFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == true)
                    {
                        for (int cvIndex = 1; cvIndex <= ZIMOFeatures.GetLength(1) - 1; cvIndex++)
                        {
                            ushort nextCV = ushort.Parse(ZIMOFeatures[i, cvIndex]);
                            if (nextCV != 0)
                            {
                                //  Check if we have already read this CV. If so, skip this CV
                                if (ConfigVariablesToRead.Contains(nextCV) == false)
                                {
                                    ConfigVariablesToRead.Add(nextCV);
                                }
                            }
                        }
                    }
                }
            }
            
            //
            // Do we have a Doehler & Haas decoder? If yes, read the Doehler & Hass specific features
            //
            Logger.PrintDevConsole("ReadWriteDecoder: Assembling Trix and Döhler and Haass compatible features ...");
            if ((DecoderConfiguration.ConfigurationVariables[8].Value == NMRA.ManufacturerID_Trix) ||
            (DecoderConfiguration.ConfigurationVariables[8].Value == NMRA.ManufacturerID_DoehlerAndHaass) ||
            (DecoderConfiguration.ConfigurationVariables[8].Value == 0))
            {
                for (int i = 0; i <= DOEHLERHAASFeatures.GetLength(0) - 1; i++)
                {
                    if (DeqSpecReader.FeatureSupported(decSpecName, DOEHLERHAASFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == true)
                    {
                        for (int cvIndex = 1; cvIndex <= DOEHLERHAASFeatures.GetLength(1) - 1; cvIndex++)
                        {
                            ushort nextCV = ushort.Parse(DOEHLERHAASFeatures[i, cvIndex]);
                            if (nextCV != 0)
                            {
                                //  Check if we have already read this CV. If so, skip this CV
                                if (ConfigVariablesToRead.Contains(nextCV) == false)
                                {
                                    ConfigVariablesToRead.Add(nextCV);
                                }
                            }
                        }
                    }
                }
            }

            return ConfigVariablesToRead;            
        }

        /// <summary>
        /// Returns a list with all configuration variables which have been modified.
        /// </summary>
        /// <returns>Returns a list of configuration variable numbers</returns>
        public static List<int> GetModifiedConfigurationVariables(string decSpecName, NMRA.DCCProgrammingModes mode)
        {
            List<int> SupportConfigVariables = new List<int>();
            List<int> ModifiedConfigVariables = new List<int>();

            for (int i = 0; i <= RCNFeatures.GetLength(0) - 1; i++)
            {
                if (DeqSpecReader.FeatureSupported(decSpecName, RCNFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == true)
                {
                    //  We check whether we are allowed to describe the configuration variables belonging to the feature.
                    if (DeqSpecReader.IsWriteable(decSpecName, RCNFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == false) continue;

                    for (int cvIndex = 1; cvIndex <= RCNFeatures.GetLength(1) - 1; cvIndex++)
                    {
                        ushort nextCV = ushort.Parse(RCNFeatures[i, cvIndex]);
                        if (nextCV != 0)
                        {
                            //  Check if we have already written this CV. If so, skip this CV
                            if (SupportConfigVariables.Contains(nextCV) == false)
                            {
                                SupportConfigVariables.Add(nextCV);
                            }
                        }
                    }
                }
            }

            if ((DecoderConfiguration.ConfigurationVariables[8].Value == NMRA.ManufacturerID_Trix) ||
            (DecoderConfiguration.ConfigurationVariables[8].Value == NMRA.ManufacturerID_DoehlerAndHaass))
            {

                for (int i = 0; i <= DOEHLERHAASFeatures.GetLength(0) - 1; i++)
                {
                    if (DeqSpecReader.FeatureSupported(decSpecName, DOEHLERHAASFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == true)
                    {
                        //  We check whether we are allowed to describe the configuration variables belonging to the feature.
                        if (DeqSpecReader.IsWriteable(decSpecName,  DOEHLERHAASFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == false) continue;

                        for (int cvIndex = 1; cvIndex <= DOEHLERHAASFeatures.GetLength(1) - 1; cvIndex++)
                        {
                            ushort nextCV = ushort.Parse(DOEHLERHAASFeatures[i, cvIndex]);
                            if (nextCV != 0)
                            {
                                //  Check if we have already written this CV. If so, skip this CV
                                if (SupportConfigVariables.Contains(nextCV) == false)
                                {
                                    SupportConfigVariables.Add(nextCV);
                                }
                            }
                        }
                    }
                }

            }

            if (DecoderConfiguration.ConfigurationVariables[8].Value == NMRA.ManufacturerID_Zimo)
            {
                for (int i = 0; i <= ZIMOFeatures.GetLength(0) - 1; i++)
                {
                    if (DeqSpecReader.FeatureSupported(decSpecName, ZIMOFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == true)
                    {
                        //  We check whether we are allowed to describe the configuration variables belonging to the feature.
                        if (DeqSpecReader.IsWriteable(decSpecName, ZIMOFeatures[i, 0], ApplicationFolders.DecSpecsFolderPath) == false) continue;

                        for (int cvIndex = 1; cvIndex <= ZIMOFeatures.GetLength(1) - 1; cvIndex++)
                        {
                            ushort nextCV = ushort.Parse(ZIMOFeatures[i, cvIndex]);
                            if (nextCV != 0)
                            {
                                //  Check if we have already written this CV. If so, skip this CV
                                if (SupportConfigVariables.Contains(nextCV) == false)
                                {
                                    SupportConfigVariables.Add(nextCV);
                                }
                            }
                        }
                    }
                }
            }

            //  Read all configuration variables from the decoder
            for (int i = 0; i <= SupportConfigVariables.Count - 1; i++)
            {

                //  Check if this CV values has been changed, otherwise skip
                byte currentValue = DecoderConfiguration.ConfigurationVariables[SupportConfigVariables[i]].Value;
                byte backupValue = DecoderConfiguration.BackupCVs[SupportConfigVariables[i]].Value;
                if (DecoderConfiguration.ConfigurationVariables[SupportConfigVariables[i]].Value != DecoderConfiguration.BackupCVs[SupportConfigVariables[i]].Value)
                {
                    ModifiedConfigVariables.Add(SupportConfigVariables[i]);
                }
            }

            return ModifiedConfigVariables;

        }

        /// <summary>
        /// Uploads the configuration variables from the datastore to the selected decoder.
        /// </summary>
        /// <param name="cancelToken">A CancellationToke to cancel the upload of the data.</param>
        /// <param name="locomotiveAddress">The address of the required decoder.</param>
        /// <param name="decSpecName">The decoder specification name.</param>
        /// <param name="mode">The programming mode.</param>
        /// <param name="progressPercentage">The current progress state in percent.</param>
        /// <param name="progressCV">The currently processed CV.</param>
        /// <returns></returns>
        internal static Task<bool> UploadDecoderData(CancellationToken cancelToken, ushort locomotiveAddress, string decSpecName, NMRA.DCCProgrammingModes mode, IProgress<int> progressPercentage, IProgress<int> progressCV)
        {
            _mode = mode;
            _decSpecName = decSpecName;

            Logger.PrintDevConsole("ReadWriteDecoder: Enter UploadDecoderData");

            List<int> ConfigVariablesToRead = new List<int>();

            //  We check if we can reach the command station
            Logger.PrintDevConsole("ReadWriteDecoder: CommandStation.Connect()");
            if (CommandStation.Connect(cancelToken,5000) == false) { return Task.FromResult(false); }

            //  Turn the track power ON if we are in POM mode
            if (mode == NMRA.DCCProgrammingModes.POMMainTrack)
            {
                Logger.PrintDevConsole("ReadWriteDecoder: CommandStation.SetTrackPowerON()");
                SetTrackPowerON();
            }

            //   Is the automatic decoder detection activated? If yes - try to detect the decoder automatically
            if (int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_AUTODECODER_DETECT_KEY, AppConstants.PREFERENCES_AUTODECODER_DETECT_DEFAULT)) == 1)
            {
                _decSpecName = DetectDecoderDeqSpec(cancelToken, locomotiveAddress);
                DecoderSpecification.DeqSpecName = _decSpecName;
                DecoderConfiguration.SetDecoderSpecification(DecoderSpecification.DeqSpecName);
                //DecoderConfiguration.EnableAllCVsSupportedByDecSpec(DecoderSpecification.DeqSpecName);
                WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
            }

            if (cancelToken.IsCancellationRequested == true)
            {
                Logger.PrintDevConsole("ReadWriteDecoder: The user has cancelled the operation ... returning");
                CommandStation.Z21.SetTrackPowerOn();
                return Task.FromResult(false);
            }

            //  Create a list of supported and readable configuration variables.
            ConfigVariablesToRead = GetAllReadableConfigurationVariables(_decSpecName);

            //  The required configuration variables have now been collected.
            //  We can now read out the configuration variables.
            Logger.PrintDevConsole("ReadWriteDecoder: Reading the features from the decoder ...");
            for (int i = 0; i<= ConfigVariablesToRead.Count -1; i++)
            {
                // Before uploading, we need to check whether this variable is enabled.
                if (DecoderConfiguration.IsCVEnabled((ushort)ConfigVariablesToRead[i]) == true)
                {
                    //  We report the configuration variable that will be read next.
                    progressCV.Report(ConfigVariablesToRead[i]);

                    //  Read the next configuration variable from the collected list.
                    if (ReadCV((ushort)ConfigVariablesToRead[i], locomotiveAddress, _mode, cancelToken) == false)
                    {
                        // We now check the user-specific setting to see whether the function must be aborted in the event
                        // of a read error. If yes, the process is canceled. Otherwise, reading continues.
                        if (int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_QUITONREADERROR_KEY, AppConstants.PREFERENCES_QUITONREADERROR_VALUE)) == 1)
                        {
                            CommandStation.Z21.SetTrackPowerOn();
                            return Task.FromResult(false);
                        }
                    }
                }
                else
                {
                    //  Just skip the CV variable - nothing to do
                    int o = 0; o++;
                }

                //  Reporting the current percentage value.
                int percent = (int)(((double)100 / (double)ConfigVariablesToRead.Count) * (double)i);
                Logger.PrintDevConsole("UploadDecoderData: Percentage:" + percent);
                progressPercentage.Report(percent);

                //  Check if the cancel token has been set.  
                if (cancelToken.IsCancellationRequested == true)
                {
                    CommandStation.Z21.SetTrackPowerOn();
                    return Task.FromResult(false);
                }
            }
            progressPercentage.Report(100);


            // We turn the power on to stop the programming mode
            Logger.PrintDevConsole("ReadWriteDecoder: Turning on the track power (to disable programming mode) ...");
            CommandStation.Z21.SetTrackPowerOn();

            Logger.PrintDevConsole("ReadWriteDecoder: Leave UploadDecoderData");
            return Task.FromResult(true);

        }

        /// <summary>
        /// Turns the main track power on. This also implicitly deactivates the programming track.
        /// </summary>
        /// <returns></returns>
        public static bool SetTrackPowerON()
        {
            int ElapsedTime = 0;

            //  We need to turn on the track power if we are in POM mode
            if (_mode == NMRA.DCCProgrammingModes.POMMainTrack)
            {
                CommandStation.Z21.SetTrackPowerOn();

                // TODO: The "unattractive" sleep command should be replaced by waiting for feedback LAN_X_BC_TRACK_POWER_ON.
                ElapsedTime = 0;
                while (ElapsedTime < 200)
                {
                    Thread.Sleep(10);
                    ElapsedTime++;
                }
            }

            return true;
        }

        /// <summary>
        /// Read a CV value
        /// </summary>
        /// <param name="cv"></param>
        /// <param name="locomotiveAddress"></param>
        /// <returns></returns>
        internal static bool ReadCV(ushort cv, ushort locomotiveAddress, NMRA.DCCProgrammingModes mode, CancellationToken token)
        {
            _mode = mode;

            //  Do we have a valid locomotive address to read data in POM mode?
            if ((_mode == NMRA.DCCProgrammingModes.POMMainTrack) && (locomotiveAddress <= 0)) return false;

            //  We support CV greater than 0
            if (cv < 1) return false;

            //  Now we are reading the data from the given cv
            _waitingForResultReceived = true; _commandSuccessFull = false;
            bool read = (_mode == NMRA.DCCProgrammingModes.POMMainTrack) ? CommandStation.Z21.ReadCVPOM(cv, locomotiveAddress) : CommandStation.Z21.ReadCVProgramTrack(cv);

            //  Waiting for response of the command station
            if (read == false) return false;

            if (WaitForAck(token) == false)
            {
                //  After NACK from the Z21 we need to update the Z21 operating mode
                CommandStation.Z21.GetOperatingMode();
                return false;
            }

            //  Success
            return true;

        }

        /// <summary>
        /// Read a CV value
        /// </summary>
        /// <param name="cv"></param>
        /// <param name="locomotiveAddress"></param>
        /// <returns></returns>
        internal static bool WriteCV(ushort cv, ushort locomotiveAddress, byte value, NMRA.DCCProgrammingModes mode, CancellationToken token)
        {
            _mode = mode;

            //  Do we have a valid locomotive address to read data in POM mode?
            if ((_mode == NMRA.DCCProgrammingModes.POMMainTrack) && (locomotiveAddress <= 0)) return false;

            //  We support CV greater than 0
            if (cv < 1) return false;

            //  Now we are reading the data from the given cv
            _waitingForResultReceived = true; _commandSuccessFull = false;
         

            bool read = (_mode == NMRA.DCCProgrammingModes.POMMainTrack) ? CommandStation.Z21.WriteCVBytePOM(cv, locomotiveAddress, value) : CommandStation.Z21.WriteCVProgramTrack(cv, value);

            //  Waiting for response of the command station
            if (read == false) return false;

            //  Handling the ACK
            if (_mode == NMRA.DCCProgrammingModes.POMMainTrack)
            {
                //  The Z21 does not provide a ACK for writing bytes in POM mode
                Thread.Sleep(200);
            }
            else
            {
                if (WaitForAck(token) == false)
                {
                    //  After NACK from the Z21 we need to update the Z21 operating mode
                    CommandStation.Z21.GetOperatingMode();
                    return false;
                }
            }

            //  Success
            return true;

        }

        /// <summary>
        /// Waits the given seconds for an ACK of the command station.
        /// </summary>
        /// <param name="cancelToken">The cancel token.</param>
        /// <returns></returns>
        private static bool WaitForAck(CancellationToken cancelToken)
        {

            Logger.PrintDevConsole("ReadWriteDecoder:WaitForAck");

            //  Wait for the acknowledge (_watingForData will be false), or for the command timeout
            int ellapsedSeconds = 0;

            //while ((cancelToken.IsCancellationRequested == false) && (_waitingForResultReceived == true) && (ellapsedSeconds < seconds))
            //{
            //    Thread.Sleep(1000);
            //    ellapsedSeconds++;
            //}

            while ((cancelToken.IsCancellationRequested == false) && (_waitingForResultReceived == true))
            {
                //  
                //  ATTENTION:
                //  Do not replace Thread.Sleep(1000) with Task.Delay(1000). Task.Delay(1000) will break compatibility with Android.
                Thread.Sleep(50);
                ellapsedSeconds++;
            }

            //  Return false if our internal application timer limit has been reached.
            //if (ellapsedSeconds >= seconds) return false;

            //  Return false if the user has canceled the progress
            if (cancelToken.IsCancellationRequested == true) return false;
            //  Return the return value of the ACK message
            return _commandSuccessFull;
        }

        /// <summary>
        /// Return the decoder specification. If the decoder could not be detected GENERICDECSPEQNAME will be returned
        /// </summary>
        /// <returns></returns>
        private static string DetectDecoderDeqSpec(CancellationToken cancelToken, ushort locomotiveAddress)
        {
            //  First we read the manufacturer of CV8
            _waitingForResultReceived = true; _commandSuccessFull = false;  
            bool read = (_mode == NMRA.DCCProgrammingModes.POMMainTrack) ? CommandStation.Z21.ReadCVPOM(8, locomotiveAddress) : CommandStation.Z21.ReadCVProgramTrack(8);

            if (WaitForAck(cancelToken) == false) return DeqSpecReader.GetDefaultDecSpecName();

            //  Automatic detection of ZIMO decoder
            if (DecoderConfiguration.BackupCVs[8].Value == NMRA.ManufacturerID_Zimo)
            {
                _waitingForResultReceived = true; _commandSuccessFull = false;
                read = (_mode == NMRA.DCCProgrammingModes.POMMainTrack) ? CommandStation.Z21.ReadCVPOM(250, locomotiveAddress) : CommandStation.Z21.ReadCVProgramTrack(250);

                if (WaitForAck(cancelToken) == false) return DeqSpecReader.GetDefaultDecSpecName();

                return FileAndFolderManagement.DeqSpecReader.GetDecoderDecSpeqName(DecoderConfiguration.BackupCVs[8].Value, DecoderConfiguration.BackupCVs[250].Value);
            }

            //  Automatic detection of Minitrix decoder
            if (DecoderConfiguration.BackupCVs[8].Value == NMRA.ManufacturerID_Trix)
            {
                _waitingForResultReceived = true; _commandSuccessFull = false;
                read = (_mode == NMRA.DCCProgrammingModes.POMMainTrack) ? CommandStation.Z21.ReadCVPOM(261, locomotiveAddress) : CommandStation.Z21.ReadCVProgramTrack(261);

                if (WaitForAck(cancelToken) == false) return DeqSpecReader.GetDefaultDecSpecName();

                return FileAndFolderManagement.DeqSpecReader.GetDecoderDecSpeqName(DecoderConfiguration.BackupCVs[8].Value, DecoderConfiguration.BackupCVs[261].Value);
            }


            //  Automatic detection of Doehler & Haass 
            if (DecoderConfiguration.BackupCVs[8].Value == NMRA.ManufacturerID_DoehlerAndHaass)
            {
                _waitingForResultReceived = true; _commandSuccessFull = false;
                read = (_mode == NMRA.DCCProgrammingModes.POMMainTrack) ? CommandStation.Z21.ReadCVPOM(261, locomotiveAddress) : CommandStation.Z21.ReadCVProgramTrack(261);

                if (WaitForAck(cancelToken) == false) return DeqSpecReader.GetDefaultDecSpecName();

                return FileAndFolderManagement.DeqSpecReader.GetDecoderDecSpeqName(DecoderConfiguration.BackupCVs[8].Value, DecoderConfiguration.BackupCVs[261].Value);
            }

            return DeqSpecReader.GetDefaultDecSpecName();
        }

        /// <summary>
        /// Event handler for the ProgramResultReceived event of the Z21 library
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnProgramResultReceived(object? sender, ProgramEventArgs e)
        {

            // Copy the data of the result into the data store (decoder and application)
            DecoderConfiguration.BackupCVs[e.CV.Number].Value = e.CV.Value;
            DecoderConfiguration.BackupCVs[e.CV.Number].Enabled = true;

            DecoderConfiguration.ConfigurationVariables[e.CV.Number].Value = e.CV.Value;
            DecoderConfiguration.ConfigurationVariables[e.CV.Number].Enabled = true;

            Logger.PrintDevConsole("ReadWriteDecoder:OnProgramResultReceived _commandSuccessFull = " + e.Success.ToString());
            _commandSuccessFull = e.Success;

            Logger.PrintDevConsole("ReadWriteDecoder:OnProgramResultReceived _waitingForResultReceived = false");
            _waitingForResultReceived = false;

        }

        public static bool AllCVsEnabled (string featureName)
        {
            //  Search the RCN225 features     
            for (int i = 0; i<= RCNFeatures.GetLength(0) - 1; i++)            
            {
                if(RCNFeatures[i,0] == featureName)
                {
                    for (int cvIndex = 1; cvIndex <= RCNFeatures.GetLength(1) - 1; cvIndex++)
                    {
                        int CVNumber = int.Parse(RCNFeatures[i, cvIndex]);
                        if (CVNumber == 0) continue;
                        ConfigurationVariableType item = DecoderConfiguration.ConfigurationVariables.Single(s => s.Number == CVNumber);
                        if (item.Enabled == false) return false;
                    }
                    return true;
                }
            }

            //  Search the ZIMO features  
            for (int i = 0; i<= ZIMOFeatures.GetLength(0) - 1; i++)            
            {
                if(ZIMOFeatures[i,0] == featureName)
                {
                    for (int cvIndex = 1; cvIndex <= ZIMOFeatures.GetLength(1) - 1; cvIndex++)
                    {
                        int CVNumber = int.Parse(ZIMOFeatures[i, cvIndex]);
                        if (CVNumber == 0) continue;
                        ConfigurationVariableType item = DecoderConfiguration.ConfigurationVariables.Single(s => s.Number == CVNumber);
                        if (item.Enabled == false) return false;
                    }
                    return true;
                }
            }

            //  Search the Döhler and Haass features
            for (int i = 0; i<= DOEHLERHAASFeatures.GetLength(0) - 1; i++)            
            {
                if(ZIMOFeatures[i,0] == featureName)
                {
                    for (int cvIndex = 1; cvIndex <= DOEHLERHAASFeatures.GetLength(1) - 1; cvIndex++)
                    {
                        int CVNumber = int.Parse(DOEHLERHAASFeatures[i, cvIndex]);
                        if (CVNumber == 0) continue;
                        ConfigurationVariableType item = DecoderConfiguration.ConfigurationVariables.Single(s => s.Number == CVNumber);
                        if (item.Enabled == false) return false;
                    }
                    return true;
                }
            }

            return false;
        }

    }
}
