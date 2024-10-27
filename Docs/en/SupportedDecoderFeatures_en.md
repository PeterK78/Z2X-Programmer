# Supported decoder features
## RCN-225
| Keyword |	Configuration Value	| Description | 	Tab |
| :----------- | :--------------: | :-------------------------: |:-------------------------: |
|RCN225_BASEADDRESS_CV1	| CV1		| Defines the short vehicle address.	| 	Address |
|RCN225_MINIMALSPEED_CV2		| CV2	| 	Defines the minimum speed.	| 	Motor characteristics |
|RCN225_ACCELERATIONFACTOR_CV3		| CV3	| 	Defines the acceleration factor (acceleration time)		| Driving characteristics|
|RCN225_DECELERATIONFACTOR_CV4	| CV4	| 	Defines the braking factor (braking time)		| Driving characteristics|
|RCN225_MAXIMALSPEED_CV5	| 	CV5		| Defines the maximum speed.	| 	Motor characteristics|
|RCN225_MEDIUMSPEED_CV6	| 	CV6		| Defines the average speed.		| Motor characteristics|
|RCN225_DECODERVERSION_CV7	| 	CV7		| The version number of the decoder.		| Decoder information|
|RCN225_MANUFACTUERID_CV8		| CV8	| 	The manufacturer ID of the decoder.	| 	Decoder information|
|RCN225_DECODERRESET_CV8	| 	CV8	| 	Resets the decoder to the factory settings.	| 	Maintenance|
|RCN225_DECODERLOCK_CV15X	| 	CV15, CV16		| Activates or deactivates the decoder lock.	| 	Security |
|RCN225_CONSISTADDRESS_CV19	| 	CV19		| Defines the network address.		| Address |
|RCN225_ABC_CV27_X		| CV270, CV27.1	| 	Activates the ABC braking distance.	| 	Driving characteristics |
|RCN225_HLU_CV27_2		| CV27.2		| Activates the HLU braking distance.	| 	Protocols |
|RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0		| CV28.0		| RailCom®: Activates or deactivates the sending of the address on channel 1.	| 	Protocols |
|RCN225_RAILCOMCHANNEL2DATA_CV28_1		| CV28.1		| RailCom®: Activates or deactivates data transmission on channel 2.	| 	Protocols |
|RCN225_AUTOMATICREGISTRATION_CV28_7	| 	CV28.7	| 	Activates and deactivates automatic log-on via RCN-218 or RailComPlus®.	| 	Protocols |
|RCN225_DIRECTION_CV29_0		| CV29.0		| Inverts the direction of travel.		| Driving characteristics |
|RCN225_SPEEDSTEPS_CV29_1		| CV29.1	| 	Defines the speed step system ( 14. or 28/128 speed steps).	| 	Driving characteristics |
|RCN225_ANALOGMODE_CV29_2		| CV 29.2	| 	Activates or deactivates analog operation.		| Protocols |
|RCN225_RAILCOMENABLED_CV29_3	| 	CV29.3	| 	Activates or deactivates RailCom® bidirectional communication.	| 	Protocols |
|RCN225_SPEEDTABLE_CV29_4		| CV29.4	| 	Activates or deactivates the extended speed curve.		| Motor characteristics |
|RCN225_LONGSHORTADDRESS_CV29_5		| CV29.5		| Address mode - defines whether short addresses from CV1 or long addresses from CV17+CV18 are used.	| 	Address |
|RCN225_FUNCTIONKEYMAPPING_CV3346	| 	CV33, CV46	| The assignment of function button to function output.		| Function keys |
|RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X	| 	CV67, CV94		| The characteristic curve points of the extended speed characteristic curve.		| Motor characteristics |

## ZIMO
| Keyword |	Configuration Value	| Description | 	Tab |
| :----------- | :--------------: | :-------------------------: |:-------------------------: |
| ZIMO_FUNCKEY_SOUNDALLOFF_CV310	 | CV310	 | Button for switching the sound on and off.	 | Function keys | 
| ZIMO_FUNCKEY_CURVESQUEAL_CV308	 | CV308	 | Button for switching the curve squeal on and off.	 | Function keys | 
| ZIMO_SELFTEST_CV30 | 	CV30 | 	Self-test.	 | Maintenance | 
| ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57	 | CV57	 | Reference voltage value for motor control.	 | Motor characteristics | 
| ZIMO_LIGHT_EFFECTS_CV125X | 	CV125, CV126	 | Configures lighting effects for function outputs such as fade-in and fade-out. | 	Light | 
| ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396	 | CV396	 | Configures a function key to reduce the volume.	 | Function keys | 
| ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397	 | CV397	 | Configures a function key to increase the volume.	 | Function keys | 
| ZIMO_SUBVERSIONNR_CV65	 | CV65	 | Reads out the subversion number.	 | Decoder information | 
| ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156 | CV156 | Function button for deactivating the braking and acceleration time	 | Function keys | 
| ZIMO_DECODERTYPE_CV250	 | CV250 | 	Reads out the decoder type.	 | Decoder information | 
| ZIMO_DECODERID_CV25X | 	CV250, CV251, CV252, CV253	 | Reads out the decoder ID (serial number) of the decoder. | 	Decoder information | 
| ZIMO_LIGHT_DIM_CV60 | 	CV60 | 	Activates and deactivates the dimming of individual function outputs. | 	Light | 
| ZIMO_BOOTLOADER_VERSION_24X |	CV248, CV249  | 	Reads out the bootloader version. | 	Decoder information | 
| ZIMO_MXMOTORCONTROLFREQUENCY_CV9	 | CV9 | 	Configures the motor control period and the EMF sampling algorithm.	 | Motor characteristics | 
| ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57 | 	CV57 | 	Configures the control reference.	 | Motor characteristics | 
| ZIMO_MXUPDATELOCK_CV144	 | CV144 | 	Configures the programming and update lock. | 	Security | 
| ZIMO_MXFX_SECONDADDRESS_CV64	 | CV64	 | Configures the second address of a function decoder. | 	Address | 
| ZIMO_MXMOTORCONTROLPID_CV56 | 	CV56 | 	Configures the motor control reference.	 | Motor characteristics | 
| ZIMO_BRAKESQUEAL_CV287	 | CV287	 | Configures the threshold for the brake squeal. | 	Sound | 
| ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61	 | CV61	 | Configures the function mapping variant. | 	Function keys | 
| ZIMO_SOUND_VOLUME_GENERIC_C266	 | CV266	| Configures the overall volume. | 	Sound | 
| ZIMO_SOUND_VOLUME_STEAM_CV27X	 | CV275, CV276, CV283, CV286 | 	Configures different volumes of steam locomotives. | 	Sound | 
| ZIMO_SOUND_VOLUME_DIESELELEC_CV29X | 	CV296, CV298	 | Configures different volume levels for diesel and electric locomotives. | 	Sound | 
| ZIMO_FUNCKEY_MUTE_CV313	 | CV313	 | Function button to fade the sound in and out.	 | Funtion keys | 
| ZIMO_SOUND_STARTUPDELAY_CV273	 | CV273 | 	Configures the start-up delay of the sound. | 	Sound | 
 | ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285 | 	CV285 | 	Configures the duration of the noise reduction on delay.	 | Sound | 
