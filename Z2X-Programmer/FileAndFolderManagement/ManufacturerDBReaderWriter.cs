/*

Z2X-Programmer
Copyright (C) 2024 - 2026
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

using System.Xml.Linq;
using Z2XProgrammer.DataModel;

namespace Z2XProgrammer.FileAndFolderManagement
{
    /// <summary>
    /// This class implements functions to read and write the NMRA manufacturer database file according to the standard of DecoderDB.
    /// </summary>
    /// <remarks>
    /// The implementation was carried out with kind permission from DecoderDB Stephan Bauer (info@decoderdb.de).
    /// </remarks>
    internal static class ManufacturerDBReaderWriter
    {
        /// <summary>
        /// Load manufacturers from a local Manufacturers.decdb file and return as a list of NMRAManufacturerType.
        /// This parses the file in Downloads (or any provided path) and converts each manufacturer element
        /// into an NMRAManufacturerType instance.
        /// </summary>
        /// <param name="localFilePath">Full path to the Manufacturers.decdb file.</param>
        /// <returns>List of NMRAManufacturerType parsed from the XML.</returns>
        public static List<NMRAManufacturerType> LoadManufacturersFromLocalFile(string localFilePath)
        {
            if (string.IsNullOrWhiteSpace(localFilePath)) throw new ArgumentException("localFilePath must not be empty", nameof(localFilePath));
            if (!File.Exists(localFilePath)) throw new FileNotFoundException("Local manufacturers XML not found.", localFilePath);

            XDocument doc = XDocument.Load(localFilePath);
            XNamespace ns = doc.Root!.GetDefaultNamespace();

            var manufacturers = doc.Descendants(ns + "manufacturer")
                .Select(x => new NMRAManufacturerType
                {
                    Id = (int?)x.Attribute("id") ?? 0,
                    ExtendedId = (int?)x.Attribute("extendedId"),
                    Name = (string?)x.Attribute("name") ?? string.Empty,
                    ShortName = (string?)x.Attribute("shortName") ?? string.Empty,
                    Country = (string?)x.Attribute("country") ?? string.Empty,
                    Url = (string?)x.Attribute("url") ?? string.Empty,
                    DecoderDBLink = (string?)x.Attribute("decoderDBLink") ?? string.Empty
                })
                .ToList();

            return manufacturers;
        }

        /// <summary>
        /// Writes the manufacturer database file to the manufacturer db folder.
        /// </summary>
        /// <param name="targetFileName">The file name of the target file.</param>
        /// <param name="text">The content of the XML file.</param>
        internal static bool WriteManufacturerDB(string targetFileName, string text)
        {
            try
            {
                // Write the file content to the app data directory.
                string targetFile = System.IO.Path.Combine(ApplicationFolders.ManufacturerDBFolderPath, targetFileName);

                //  If the manufacturer DB already exists, delete it first.
                if (File.Exists(targetFile)) File.Delete(targetFile);

                //  Write the new manufacturer file.
                using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
                using StreamWriter streamWriter = new StreamWriter(outputStream);
                streamWriter.Write(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //  The default Manufacturers.decdb file taken from DecoderDB.de from 2025-12-17.
        public static string DefaultManufacturers = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!-- © by DecoderDB. Licence see https://www.decoderdb.de/licence/ -->
<manufacturersList xmlns=""http://www.decoderdb.de/schema/manufacturers/1.1"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <version createdBy=""DecoderDB"" creatorLink=""https://www.decoderdb.de"" nmraListDate=""2025-05-08"" lastUpdate=""2025-12-17T01:05:14""/>
  <manufacturers decoderDBLink=""www.decoderdb.de?manufacturersPage"">
    <manufacturer id=""0"" name=""Standard"" shortName=""Standard"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=157""/>
    <manufacturer id=""1"" name=""CML Electronics Limited"" shortName=""CML"" country=""GB""/>
    <manufacturer id=""2"" name=""Train Technology"" shortName=""Train Technology"" country=""BE""/>
    <manufacturer id=""11"" name=""NCE Corporation (formerly North Coast Engineering)"" shortName=""NCE"" country=""US"" url=""https://www.ncedcc.com""/>
    <manufacturer id=""12"" name=""Wangrow Electronics"" shortName=""Wangrow"" country=""US""/>
    <manufacturer id=""13"" name=""Public Domain &amp; Do-It-Yourself Decoders"" shortName=""DIY""/>
    <manufacturer id=""13"" extendedId=""256"" name=""DecoderDB"" shortName=""DecoderDB"" country=""DE"" url=""www.decoderdb.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=159""/>
    <manufacturer id=""13"" extendedId=""257"" name=""OpenCarSystem"" shortName=""OpenCar"" country=""DE"" url=""www.opencarsystem.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=158""/>
    <manufacturer id=""13"" extendedId=""258"" name=""OpenDCC"" shortName=""OpenDCC"" country=""DE"" url=""www.opendcc.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=161""/>
    <manufacturer id=""13"" extendedId=""259"" name=""Schwedes &amp; Kuhn"" shortName=""S&amp;K"" country=""DE"" url=""https://www.stummiforum.de/t140441f21-SUSI-Funktions-und-Servo-Controller-Familie.html""/>
    <manufacturer id=""13"" extendedId=""260"" name=""moba-licht.de"" shortName=""moba-licht.de"" country=""DE""/>
    <manufacturer id=""13"" extendedId=""261"" name=""sfx Engineering"" shortName=""sfx"" country=""DE""/>
    <manufacturer id=""13"" extendedId=""262"" name=""Beri"" shortName=""Beri"" country=""DE""/>
    <manufacturer id=""13"" extendedId=""263"" name=""MicroBahner"" shortName=""MicroBahner"" country=""DE"" url=""https://github.com/MicroBahner""/>
    <manufacturer id=""13"" extendedId=""264"" name=""Fichtelbahn"" shortName=""Fichtelbahn"" country=""DE"" url=""www.fichtelbahn.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=175""/>
    <manufacturer id=""13"" extendedId=""265"" name=""RTB"" shortName=""RTB"" country=""DE"" url=""https://rtb4dcc.de""/>
    <manufacturer id=""13"" extendedId=""266"" name=""xDuinoRails"" shortName=""xDuinoRails"" country=""CH"" url=""https://github.com/xDuinoRails""/>
    <manufacturer id=""13"" extendedId=""267"" name=""gab-k"" shortName=""gab-k"" country=""DE"" url=""https://github.com/gab-k""/>
    <manufacturer id=""14"" name=""PSI - Dynatrol"" shortName=""PSI"" country=""US""/>
    <manufacturer id=""15"" name=""Ramfixx Technologies (Wangrow)"" shortName=""Ramfixx"" country=""CA""/>
    <manufacturer id=""17"" name=""Advance IC Engineering"" shortName=""Advice"" country=""US"" url=""https://www.advice1.com""/>
    <manufacturer id=""18"" name=""JMRI"" shortName=""JMRI"" country=""US"" url=""https://www.jmri.org""/>
    <manufacturer id=""19"" name=""AMW"" shortName=""AMW"" country=""AT"" url=""https://amw.huebsch.at""/>
    <manufacturer id=""20"" name=""T4T - Technology for Trains GmbH"" shortName=""T4T"" country=""DE"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=131""/>
    <manufacturer id=""21"" name=""Kreischer Datentechnik"" shortName=""Kreischer"" country=""DE""/>
    <manufacturer id=""22"" name=""KAM Industries"" shortName=""KAM"" country=""US""/>
    <manufacturer id=""23"" name=""S Helper Service"" shortName=""S Helper Service"" country=""US""/>
    <manufacturer id=""24"" name=""mobatron.de"" shortName=""MoBaTron"" country=""DE"" url=""https://mobatron.4lima.de""/>
    <manufacturer id=""25"" name=""Team Digital"" shortName=""Team Digital"" country=""US"" url=""https://www.teamdigital1.com""/>
    <manufacturer id=""26"" name=""MBTronik - PiN GITmBH"" shortName=""MBTronik"" country=""DE"" url=""http://www.mbtronik.de""/>
    <manufacturer id=""27"" name=""MTH Electric Trains, Inc."" shortName=""MTH"" country=""US"" url=""https://www.mthtrains.com""/>
    <manufacturer id=""28"" name=""Heljan A/S"" shortName=""Heljan"" country=""DK"" url=""https://heljan.dk""/>
    <manufacturer id=""29"" name=""Mistral Train Models"" shortName=""Mistral"" country=""BE"" url=""https://www.mistraltrains.be""/>
    <manufacturer id=""30"" name=""Digisight"" shortName=""Digisight"" country=""CN"" url=""http://www.digsight.com""/>
    <manufacturer id=""31"" name=""Brelec"" shortName=""Brelec"" country=""BE"" url=""https://www.brelec.eu""/>
    <manufacturer id=""32"" name=""Regal Way Co. Ltd"" shortName=""Regal Way"" country=""HK"" url=""http://www.regalway.com""/>
    <manufacturer id=""33"" name=""Praecipuus"" shortName=""Praecipuus"" country=""CA""/>
    <manufacturer id=""34"" name=""Aristo-Craft Trains"" shortName=""Aristo-Craft"" country=""US""/>
    <manufacturer id=""35"" name=""Electronik &amp; Model Produktion"" shortName=""EMP"" country=""SE""/>
    <manufacturer id=""36"" name=""DCCconcepts"" shortName=""DCCconcepts"" country=""AU"" url=""https://www.dccconcepts.com""/>
    <manufacturer id=""37"" name=""NAC Services, Inc"" shortName=""NAC Services"" country=""US""/>
    <manufacturer id=""38"" name=""Broadway Limited Imports, LLC"" shortName=""BLI"" country=""US"" url=""https://broadway-limited.com""/>
    <manufacturer id=""39"" name=""Educational Computer, Inc."" shortName=""Educational Computer"" country=""US""/>
    <manufacturer id=""40"" name=""KATO Precision Models"" shortName=""KATO"" country=""JP"" url=""https://www.katomodels.com""/>
    <manufacturer id=""41"" name=""Passmann"" shortName=""Passmann"" country=""DE""/>
    <manufacturer id=""42"" name=""Digirails / Digikeijs"" shortName=""Digikeijs"" country=""NL"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=35""/>
    <manufacturer id=""43"" name=""Ngineering"" shortName=""Ngineering"" country=""US"" url=""https://www.ngineering.com""/>
    <manufacturer id=""44"" name=""SPROG-DCC"" shortName=""SPROG"" country=""GB"" url=""https://sprog-dcc.co.uk""/>
    <manufacturer id=""45"" name=""ANE Model Co, Ltd"" shortName=""ANE"" country=""TW"" url=""https://www.anemodel.com""/>
    <manufacturer id=""46"" name=""GFB Designs"" shortName=""GFB Designs"" country=""GB""/>
    <manufacturer id=""47"" name=""Capecom"" shortName=""Capecom"" country=""AU""/>
    <manufacturer id=""48"" name=""Hornby Hobbies Ltd"" shortName=""Hornby"" country=""GB"" url=""https://www.hornby.com""/>
    <manufacturer id=""49"" name=""Joka Electronic"" shortName=""Joka"" country=""DE""/>
    <manufacturer id=""50"" name=""N&amp;Q Electronics"" shortName=""N&amp;Q Electronics"" country=""ES""/>
    <manufacturer id=""51"" name=""DCC Supplies, Ltd"" shortName=""DCC Supplies"" country=""GB"" url=""https://www.dccsupplies.com""/>
    <manufacturer id=""52"" name=""Krois-Modell"" shortName=""Krois"" country=""AT"" url=""https://www.krois-modell.at""/>
    <manufacturer id=""53"" name=""Rautenhaus Digital Vertrieb"" shortName=""Rautenhaus"" country=""DE"" url=""https://www.rautenhaus-digital.de""/>
    <manufacturer id=""54"" name=""TCH Technology"" shortName=""TCH Technology"" country=""US""/>
    <manufacturer id=""55"" name=""QElectronics GmbH"" shortName=""QElectronics"" country=""DE"" url=""https://www.qdecoder.de""/>
    <manufacturer id=""56"" name=""LDH"" shortName=""LDH"" country=""AR"" url=""http://www.ldhtrenes.com.ar""/>
    <manufacturer id=""57"" name=""Rampino Elektronik"" shortName=""Rampino"" country=""DE"" url=""moba.rampino.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=118""/>
    <manufacturer id=""58"" name=""KRES GmbH"" shortName=""KRES"" country=""DE"" url=""https://modelle.kres.de""/>
    <manufacturer id=""59"" name=""Tam Valley Depot"" shortName=""Tam Valley Depot"" country=""US"" url=""https://www.tamvalleydepot.com""/>
    <manufacturer id=""60"" name=""Blücher-Electronic"" shortName=""Blücher"" country=""DE"" url=""https://www.bluecher-elektronik.de""/>
    <manufacturer id=""61"" name=""TrainModules"" shortName=""TrainModules"" country=""HU"" url=""https://trainmodules.com""/>
    <manufacturer id=""62"" name=""Tams Elektronik GmbH"" shortName=""Tams"" country=""DE"" url=""tams-online.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=133""/>
    <manufacturer id=""63"" name=""Noarail"" shortName=""Noarail"" country=""AU""/>
    <manufacturer id=""64"" name=""Digital Bahn"" shortName=""Digital Bahn"" country=""DE"" url=""www.digital-bahn.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=36""/>
    <manufacturer id=""65"" name=""Gaugemaster"" shortName=""Gaugemaster"" country=""GB"" url=""https://www.gaugemasterretail.com""/>
    <manufacturer id=""66"" name=""Railnet Solutions, LLC"" shortName=""Railnet"" country=""US""/>
    <manufacturer id=""67"" name=""Heller Modelbahn"" shortName=""Heller"" country=""DE""/>
    <manufacturer id=""68"" name=""MAWE Elektronik"" shortName=""MAWE"" country=""CH""/>
    <manufacturer id=""69"" name=""E-Modell"" shortName=""E-Modell"" country=""DE"" url=""https://www.e-modell.eu""/>
    <manufacturer id=""70"" name=""Rocrail"" shortName=""Rocrail"" country=""DE"" url=""http://rocrail.net""/>
    <manufacturer id=""71"" name=""New York Byano Limited"" shortName=""New York Byano Limited"" country=""HK""/>
    <manufacturer id=""72"" name=""MTB Model"" shortName=""MTB"" country=""CZ"" url=""https://mtb-model.com"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=87""/>
    <manufacturer id=""73"" name=""The Electric Railroad Company"" shortName=""Electric Railroad"" country=""US"" url=""http://www.electricrr.com""/>
    <manufacturer id=""74"" name=""PpP Digital"" shortName=""PpP Digital"" country=""ES"" url=""http://ppp-digital.com""/>
    <manufacturer id=""75"" name=""Digitools Elektronika, Kft"" shortName=""Digitools"" country=""HU"" url=""https://digitools.hu""/>
    <manufacturer id=""76"" name=""Auvidel"" shortName=""Auvidel"" country=""DE""/>
    <manufacturer id=""77"" name=""LS Models Sprl"" shortName=""LS Models"" country=""BE"" url=""https://lsmodels.eu""/>
    <manufacturer id=""78"" name=""Tehnologistic (train-O-matic)"" shortName=""train-O-matic"" country=""RO"" url=""www.train-o-matic.com"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=137""/>
    <manufacturer id=""79"" name=""Hattons Model Railways"" shortName=""Hattons"" country=""GB"" url=""https://www.hattons.co.uk""/>
    <manufacturer id=""80"" name=""Spectrum Engineering"" shortName=""Spectrum Engineering"" country=""US""/>
    <manufacturer id=""81"" name=""GooVerModels"" shortName=""GooVerModels"" country=""BE""/>
    <manufacturer id=""82"" name=""HAG Modelleisenbahn AG"" shortName=""HAG"" country=""CH"" url=""https://www.hag.swiss""/>
    <manufacturer id=""83"" name=""JSS-Elektronic"" shortName=""JSS"" country=""DE""/>
    <manufacturer id=""84"" name=""Railflyer Model Prototypes, Inc."" shortName=""Railflyer"" country=""CA""/>
    <manufacturer id=""85"" name=""Uhlenbrock GmbH"" shortName=""Uhlenbrock"" country=""DE"" url=""www.uhlenbrock.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=147""/>
    <manufacturer id=""86"" name=""Wekomm Engineering GmbH"" shortName=""Wekomm"" country=""DE"" url=""https://www.wekomm.de""/>
    <manufacturer id=""87"" name=""RR-Cirkits"" shortName=""RR-Cirkits"" country=""US"" url=""https://www.rr-cirkits.com""/>
    <manufacturer id=""88"" name=""HONS Model"" shortName=""HONS Model"" country=""HK""/>
    <manufacturer id=""89"" name=""Pojezdy.EU"" shortName=""Pojezdy"" country=""CZ"" url=""https://pojezdy.eu""/>
    <manufacturer id=""90"" name=""Shourt Line"" shortName=""Shourt Line"" country=""US"" url=""https://www.shourtline.com""/>
    <manufacturer id=""91"" name=""Railstars Limited"" shortName=""Railstars"" country=""US""/>
    <manufacturer id=""92"" name=""Tawcrafts"" shortName=""Tawcrafts"" country=""GB""/>
    <manufacturer id=""93"" name=""Kevtronics cc"" shortName=""Kevtronics"" country=""ZA""/>
    <manufacturer id=""94"" name=""Electroniscript, Inc"" shortName=""Electroniscript"" country=""US""/>
    <manufacturer id=""95"" name=""Sanda Kan Industrial, Ltd"" shortName=""Sanda Kan"" country=""HK"" url=""https://www.kader.com.hk""/>
    <manufacturer id=""96"" name=""PRICOM Design"" shortName=""PRICOM"" country=""US"" url=""https://www.pricom.com""/>
    <manufacturer id=""97"" name=""Doehler &amp; Haass"" shortName=""D&amp;H"" country=""DE"" url=""doehler-haass.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=40""/>
    <manufacturer id=""98"" name=""Harman DCC"" shortName=""Harman"" country=""GB"" url=""http://signalist.co.uk""/>
    <manufacturer id=""99"" name=""Lenz Elektronik GmbH"" shortName=""Lenz"" country=""DE"" url=""www.lenz-elektronik.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=73""/>
    <manufacturer id=""100"" name=""Trenes Digitales"" shortName=""Trenes Digitales"" country=""AR"" url=""https://www.trenesdigitales.com.ar""/>
    <manufacturer id=""101"" name=""Bachmann Trains"" shortName=""Bachmann"" country=""US"" url=""https://www.bachmanntrains.com""/>
    <manufacturer id=""102"" name=""Integrated Signal Systems"" shortName=""ISS"" country=""US"" url=""https://www.integratedsignalsystems.com""/>
    <manufacturer id=""103"" name=""Nagasue System Design Office"" shortName=""Nagasue"" country=""JP"" url=""https://www.snjpn.com""/>
    <manufacturer id=""104"" name=""TrainTech"" shortName=""TrainTech"" country=""NL""/>
    <manufacturer id=""105"" name=""Computer Dialysis France"" shortName=""Computer Dialysis"" country=""FR""/>
    <manufacturer id=""106"" name=""Opherline1"" shortName=""Opherline1"" country=""FR""/>
    <manufacturer id=""107"" name=""Phoenix Sound Systems, Inc."" shortName=""Phoenix Sound"" country=""US""/>
    <manufacturer id=""108"" name=""Nagoden"" shortName=""Nagoden"" country=""JP"" url=""https://nagoden.cart.fc2.com""/>
    <manufacturer id=""109"" name=""Viessmann Modellspielwaren GmbH"" shortName=""Viessmann"" country=""DE"" url=""www.viessmann-modell.com"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=149""/>
    <manufacturer id=""110"" name=""AXJ Electronics"" shortName=""AXJ"" country=""CN""/>
    <manufacturer id=""111"" name=""Haber &amp; Koenig Electronics GmbH"" shortName=""HKE"" country=""AT""/>
    <manufacturer id=""112"" name=""LSdigital"" shortName=""LSdigital"" country=""DE"" url=""https://www.lsdigital-shop.de""/>
    <manufacturer id=""113"" name=""QS Industries (QSI)"" shortName=""QSI"" country=""US"" url=""http://www.qsindustries.com""/>
    <manufacturer id=""114"" name=""Benezan Electronics"" shortName=""Benezan"" country=""ES""/>
    <manufacturer id=""115"" name=""Dietz Modellbahntechnik"" shortName=""Dietz"" country=""DE"" url=""www.d-i-e-t-z.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=33""/>
    <manufacturer id=""116"" name=""MyLocoSound"" shortName=""MyLocoSound"" country=""AU"" url=""https://www.mylocosound.com""/>
    <manufacturer id=""117"" name=""cT Elektronik"" shortName=""cT / Tran"" country=""AT"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=25""/>
    <manufacturer id=""118"" name=""Digirail - Digitale Modellbahnsteuerung"" shortName=""Digirail"" country=""DE""/>
    <manufacturer id=""119"" name=""W. S. Ataras Engineering"" shortName=""W. S. Ataras Engineering"" country=""US"" url=""http://www.wsaeng.com""/>
    <manufacturer id=""120"" name=""csikos-muhely"" shortName=""csikos-muhely"" country=""HU"" url=""http://csikos-muhely.hu""/>
    <manufacturer id=""122"" name=""Berros"" shortName=""Berros"" country=""NL"" url=""https://www.berros.eu""/>
    <manufacturer id=""123"" name=""Massoth Elektronik GmbH"" shortName=""Massoth"" country=""DE"" url=""https://www.massoth.de""/>
    <manufacturer id=""124"" name=""DCC-Gaspar-Electronic"" shortName=""DCC-Gaspar-Electronic"" country=""HU"" url=""https://www.dcc-gaspar-electronic.com""/>
    <manufacturer id=""125"" name=""ProfiLok Modellbahntechnik GmbH"" shortName=""ProfiLok"" country=""DE""/>
    <manufacturer id=""126"" name=""Möllehem Gårdsproduktion"" shortName=""Möllehem Gårdsproduktion"" country=""SE"" url=""http://mollehem.se""/>
    <manufacturer id=""127"" name=""Atlas Model Railroad Products"" shortName=""Atlas"" country=""US"" url=""https://www.atlasrr.com""/>
    <manufacturer id=""128"" name=""Frateschi Model Trains"" shortName=""Frateschi"" country=""BR"" url=""https://www.frateschi.com.br""/>
    <manufacturer id=""129"" name=""Digitrax"" shortName=""Digitrax"" country=""US"" url=""https://www.digitrax.com""/>
    <manufacturer id=""130"" name=""cmOS Engineering"" shortName=""cmOS"" country=""AU"" url=""http://www.cmoseng.com.au""/>
    <manufacturer id=""131"" name=""Trix Modelleisenbahn"" shortName=""Trix"" country=""DE"" url=""www.trix.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=146""/>
    <manufacturer id=""132"" name=""ZTC"" shortName=""ZTC"" country=""GB"" url=""https://tauntoncontrolsltd.co.uk""/>
    <manufacturer id=""133"" name=""Intelligent Command Control"" shortName=""ICC"" country=""US""/>
    <manufacturer id=""134"" name=""LaisDCC"" shortName=""LaisDCC"" country=""CN"" url=""https://laisdcc.com""/>
    <manufacturer id=""135"" name=""CVP Products"" shortName=""CVP"" country=""US"" url=""https://www.cvpusa.com""/>
    <manufacturer id=""136"" name=""NYRS"" shortName=""NYRS"" country=""US"" url=""https://nyrs.com""/>
    <manufacturer id=""138"" name=""Train ID Systems"" shortName=""Train ID Systems"" country=""US""/>
    <manufacturer id=""139"" name=""RealRail Effects"" shortName=""RealRail Effects"" country=""US""/>
    <manufacturer id=""140"" name=""Desktop Station"" shortName=""Desktop Station"" country=""JP"" url=""https://desktopstation.net""/>
    <manufacturer id=""141"" name=""Throttle-Up (Soundtraxx)"" shortName=""Soundtraxx"" country=""US"" url=""https://soundtraxx.com""/>
    <manufacturer id=""142"" name=""SLOMO Railroad Models"" shortName=""SLOMO"" country=""JP"" url=""http://www.slomo.jp.net""/>
    <manufacturer id=""143"" name=""Model Rectifier Corp"" shortName=""MRC"" country=""US"" url=""https://www.modelrectifier.com""/>
    <manufacturer id=""144"" name=""DCC Train Automation"" shortName=""DCC Train Automation"" country=""GB"" url=""https://www.dcctrainautomation.co.uk""/>
    <manufacturer id=""145"" name=""Zimo Elektronik"" shortName=""Zimo"" country=""AT"" url=""www.zimo.at"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=155""/>
    <manufacturer id=""146"" name=""Rails Europ Express"" shortName=""Rails Europ Express"" country=""FR"" url=""https://www.raileuropexpress.com""/>
    <manufacturer id=""147"" name=""Umelec Ing. Buero"" shortName=""Umelec"" country=""CH""/>
    <manufacturer id=""148"" name=""BLOCKsignalling"" shortName=""BLOCKsignalling"" country=""GB"" url=""https://blocksignalling.co.uk""/>
    <manufacturer id=""149"" name=""Rock Junction Controls"" shortName=""Rock Junction Controls"" country=""US""/>
    <manufacturer id=""150"" name=""Wm. K. Walthers, Inc"" shortName=""Walthers"" country=""US"" url=""https://www.walthers.com""/>
    <manufacturer id=""151"" name=""Electronic Solutions Ulm GmbH"" shortName=""ESU"" country=""DE"" url=""www.esu.eu"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=43""/>
    <manufacturer id=""152"" name=""Digi-CZ"" shortName=""Digi-CZ"" country=""CZ"" url=""https://www.digi-cz.cz""/>
    <manufacturer id=""153"" name=""Train Control Systems"" shortName=""TCS"" country=""US"" url=""https://www.tcsdcc.com""/>
    <manufacturer id=""154"" name=""Dapol Limited"" shortName=""Dapol"" country=""GB"" url=""https://www.dapol.co.uk""/>
    <manufacturer id=""155"" name=""Gebr. Fleischmann GmbH &amp; Co."" shortName=""Fleischmann"" country=""DE"" url=""https://www.fleischmann.de""/>
    <manufacturer id=""156"" name=""Nucky"" shortName=""Nucky"" country=""JP"" url=""https://web.nucky.jp""/>
    <manufacturer id=""157"" name=""Kühn Ing."" shortName=""Kühn"" country=""DE"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=71""/>
    <manufacturer id=""158"" name=""Fučík"" shortName=""Fučík"" country=""CZ"" url=""https://www.fucik.name""/>
    <manufacturer id=""159"" name=""LGB (Ernst Paul Lehmann Patentwerk)"" shortName=""LGB"" country=""DE"" url=""https://www.lgb.de""/>
    <manufacturer id=""160"" name=""micron-dynamics"" shortName=""mXion"" country=""DE"" url=""www.mxion.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=164""/>
    <manufacturer id=""161"" name=""Modelleisenbahn GmbH (formerly Roco)"" shortName=""ROCO"" country=""AT"" url=""https://www.roco.cc""/>
    <manufacturer id=""162"" name=""PIKO Spielwaren GmbH"" shortName=""PIKO"" country=""DE"" url=""www.piko.de"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=167""/>
    <manufacturer id=""163"" name=""WP Railshops"" shortName=""WP Railshops"" country=""CA""/>
    <manufacturer id=""164"" name=""drM"" shortName=""drM"" country=""TW"" url=""https://drm.com.tw""/>
    <manufacturer id=""165"" name=""Model Electronic Railway Group"" shortName=""MERG"" country=""GB"" url=""https://www.merg.org.uk""/>
    <manufacturer id=""166"" name=""Maison de DCC"" shortName=""Maison de DCC"" country=""JP"" url=""https://dcc.client.jp""/>
    <manufacturer id=""167"" name=""Helvest Systems GmbH"" shortName=""Helvest"" country=""CH"" url=""https://helvest.ch""/>
    <manufacturer id=""168"" name=""Model Train Technology"" shortName=""Model Train Technology"" country=""US"" url=""https://www.modeltraintechnology.com""/>
    <manufacturer id=""169"" name=""AE Electronic Ltd."" shortName=""AE Electronic"" country=""CN""/>
    <manufacturer id=""170"" name=""AuroTrains"" shortName=""AuroTrains"" country=""IT"" url=""https://www.aurotrains.com""/>
    <manufacturer id=""171"" name=""bogobit"" shortName=""bogobit"" country=""DE"" url=""https://bogobit.de""/>
    <manufacturer id=""172"" name=""RailBOX Electronics"" shortName=""RailBOX"" country=""PL"" url=""www.railbox.pl"" decoderDBLink=""https://www.decoderdb.de/?manufacturerPage=179""/>
    <manufacturer id=""173"" name=""Arnold - Rivarossi"" shortName=""Arnold"" country=""DE"" url=""https://www.rivarossi.com""/>
    <manufacturer id=""186"" name=""BRAWA Modellspielwaren GmbH &amp; Co."" shortName=""BRAWA"" country=""DE"" url=""https://www.brawa.de""/>
    <manufacturer id=""204"" name=""Con-Com GmbH"" shortName=""Con-Com"" country=""AT""/>
    <manufacturer id=""225"" name=""Blue Digital"" shortName=""Blue Digital"" country=""PL""/>
    <manufacturer id=""238"" name=""NMRA Reserved (for extended ID #’s)"" shortName=""NMRA Reserved"" country=""US"" url=""https://www.nmra.org""/>
  </manufacturers>
</manufacturersList>";
    }
}



