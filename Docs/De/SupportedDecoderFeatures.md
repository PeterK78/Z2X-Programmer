# Unterstützte Decoder-Features
## RCN-225

| Schlüsselwort |  Konfiguration  | Beschreibung | Registerkarte
| :-------- | :------- | :------- |  :------- |
| `RCN225_BASEADDRESS_CV1`  | CV1 | Definiert die kurze Fahrzeugadresse. | Adresse
| `RCN225_MINIMALSPEED_CV2`  | CV2 | Definiert die minimale Geschwindigkeit. | Motoreigenschaften
| `RCN225_ACCELERATIONFACTOR_CV3`  | CV3 | Definiert den Faktor der Beschleunigung (Beschleunigungszeit) | Fahreigenschaften
| `RCN225_DECELERATIONFACTOR_CV4`  | CV4 | Definiert den Faktor des Bremsen (Bremszeit) | Fahreigenschaften
| `RCN225_MAXIMALSPEED_CV5`  | CV5 | Definiert die maximale Geschwindigkeit. | Motoreigenschaften
| `RCN225_MEDIUMSPEED_CV6`  | CV6 | Definiert die mittlere Geschwindigkeit. | Motoreigenschaften
| `RCN225_DECODERVERSION_CV7`  | CV7 | Die Versionsnummer des Decoders. | Decoder-Informationen 
| `RCN225_MANUFACTUERID_CV8`  | CV8 | Die Herstellerkennung des Decoders. | Decoder-Informationen 
| `RCN225_DECODERRESET_CV8` | CV8 | Setzt den Decoder auf Werksteinstellungen zurück. | Wartung
| `RCN225_DECODERLOCK_CV15X` | CV15 + CV16 | Aktiviert oder deaktiviert die Decodersperre. | Sicherheit
| `RCN225_CONSISTADDRESS_CV19` | CV19 | Definiert die Verbundadresse. | Adresse
| `RCN225_ABC_CV27_X` | CV27 Bit 0 und Bit 1 | Aktiviert die ABC-Bremsstrecke. | Fahreigenschaften
| `RCN225_HLU_CV27_2` | CV27 Bit 2 | Aktiviert die HLU-Bremsstrecke. | Protokolle
| `RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0` | CV28 Bit 0 | RailCom®: Aktiviert oder deaktiviert das Senden der Adresse auf Kanal 1. | Protokolle
| `RCN225_RAILCOMCHANNEL2DATA_CV28_1` | CV28 Bit 1 | RailCom®: Aktiviert oder deaktiviert die Datenübertragung auf Kanal 2. | Protokolle
| `RCN225_AUTOMATICREGISTRATION_CV28_7` | CV28 Bit 7 | Aktiviert und deaktiviert die automatische Anmeldung per RCN-218 oder RailComPlus®. | Protokolle
| `RCN225_DIRECTION_CV29_0` | CV29 Bit 0 | Invertiert die Fahrtrichtung. | Fahreigenschaften
| `RCN225_SPEEDSTEPS_CV29_1` | CV29 Bit 1 | Definiert das Fahrstufensystem ( 14. oder 28/128 Fahrstufen). | Fahreigenschaften
| `RCN225_ANALOGMODE_CV29_2` | CV 29 Bit 2  | Aktiviert oder deaktiviert den Analogbetrieb. | Protokolle
| `RCN225_RAILCOMENABLED_CV29_3` | CV29 Bit 3 | Aktiviert oder deaktiviert die bidirekte Kommunikation RailCom®. | Protokolle
| `RCN225_SPEEDTABLE_CV29_4` | CV29 Bit 4 | Aktiviert oder deaktiviert die erweiterte Geschwindigkeitskennlinie. | Motoreigenschaften
| `RCN225_LONGSHORTADDRESS_CV29_5` | CV29 Bit 5 | Adressmodus - definiert ob kurze Adressen aus CV1, oder lange Adressen aus CV17+CV18 verwendet werden. | Adresse
| `RCN225_FUNCTIONKEYMAPPING_CV3346` | CV33 - CV46 | Zuordnung von Funktionstaste zu Funktionsausgang. | Funktionstasten
| `RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X` | CV67 - CV94  | Die Kennlinienpunkte der erweiterten Geschwindigkeitskennlinie. | Motoreigenschaften

## ZIMO

| Schlüsselwort |  Konfiguration  | Beschreibung | Registerkarte
| :-------- | :------- | :------- |  :------- |
| `ZIMO_FUNCKEY_SOUNDALLOFF_CV310`  | CV310 | Taste zum Ein- und Ausschalten des Sounds. | Funktionstasten
| `ZIMO_FUNCKEY_CURVESQUEAL_CV308`  | CV308 | Taste zum Ein- und Ausschalten des Kurvenquietschens. | Funktionstasten
| `ZIMO_SELFTEST_CV30`| CV30 | Selbsttest. | Wartung
| `ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57`| CV57 | Referenz-Spannungswert für die Motor-Regelung. | Motoreigenschaften
| `ZIMO_LIGHT_EFFECTS_CV125X`| CV125 + CV 126 | Konfiguriert Lichteffekte für Funktionsausgänge wie z. B. das Auf- und Abblenden. | Licht
| `ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396`| CV396 | Konfiguriert eine Funktionstaste zum Verringern der Lautstärke. | Funktionstasten
| `ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397`| CV397 | Konfiguriert eine Funktionstaste zum Erhöhen der Lautstärke. | Funktionstasten
| `ZIMO_SUBVERSIONNR_CV65`| CV65 | Liest die Subversionsnummer aus. | Decoder-Informationen
| `ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156`| CV156 | Funktionstaste zum Deaktivieren der Brems- und Beschleunigungszeit | Funktionstasten
| `ZIMO_DECODERTYPE_CV250`| CV250 | Liest den Decoder-Typ aus. | Decoder-Informationen
| `ZIMO_DECODERID_CV25X`| CV250, CV251, CV252 und CV253 | Liest die Decoder-ID (Seriennummer) des Decoders aus. | Decoder-Informationen
| `ZIMO_LIGHT_DIM_CV60`| CV60 | Aktiviert und Deaktiviert das Dimmen einzelner Funktionsausgänge. | Licht
| `ZIMO_BOOTLOADER_VERSION_24X`| CV248 und CV249 | Liest die Bootloaderversion aus. | Decoder-Informationen
| `ZIMO_MXMOTORCONTROLFREQUENCY_CV9`| CV9 | Konfiguriert die Motoransteuerungsperiode und den EMK-Abtast-Algorithmus. | Motoreigenschaften
| `ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57`| CV57 | Konfiguriert die Regelungsreferenz. | Motoreigenschaften
| `ZIMO_MXUPDATELOCK_CV144`| CV144 | Konfiguriert die Programmier- und Updatesperre. | Sicherheit
| `ZIMO_MXFX_SECONDADDRESS_CV64`| CV64 | Konfiguriert die Zweitadresse eines Funktionsdecoders. | Adresse
| `ZIMO_MXMOTORCONTROLPID_CV56`| CV56 | Konfiguriert die Motorregelungs-Referenz. | Motoreigenschaften
| `ZIMO_BRAKESQUEAL_CV287`| CV287 | Konfiguriert die Schwelle für das Bremsenquietschen.| Sound
| `ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61`| CV61 | Konfiguriert die Variante des Funktionsmappings.| Funktionstasten
| `ZIMO_SOUND_VOLUME_GENERIC_C266`| CV266 | Konfiguriert die Gesamtlautstärke.| Sound
| `ZIMO_SOUND_VOLUME_STEAM_CV27X`| CV275, CV276, CV283 und CV286 | Konfiguriert verschiedene Lautstärken von Dampflokomotiven.| Sound
| `ZIMO_SOUND_VOLUME_DIESELELEC_CV29X`| CV296 and CV298| Konfiguriert verschiedene Lautstärken für Diesel- und Elektrolokomotiven.| Sound
| `ZIMO_FUNCKEY_MUTE_CV313`| CV313| Funktionstaste zum Ein- und Ausblenden des Sounds.| Funktionstaste
| `ZIMO_SOUND_STARTUPDELAY_CV273`| CV273| Konfiguriert die Anfahrverzögerung des Sounds.| Sound
| `ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285`| CV285| Konfiguriert die Dauer der Geräuschreduktion bei Verzögerung.| Sound
