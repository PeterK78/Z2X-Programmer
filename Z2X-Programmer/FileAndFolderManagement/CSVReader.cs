using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;

namespace Z2XProgrammer.FileAndFolderManagement
{

    /// <summary>
    /// This class implements the functions to read CV-set files in CSV format.
    /// </summary>
    internal static class CSVReader
    {
        /// <summary>
        /// Reads the CSV file from the given file stream and updates the decoder configuration.
        /// </summary>
        /// <param name="csvFileStream">A file stream of a CSV file.</param>
        /// <param name="delimiter">Delimiter used in the CSV file, typically a comma or semicolon.</param>
        /// <returns></returns>
        public static bool ReadFile(Stream csvFileStream, char delimiter)
        {
            try
            {
                var configVarList = new List<CSVCommandType>();

                using var reader = new StreamReader(csvFileStream, Encoding.UTF8);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(delimiter);
                        if (values.Length != 4) throw new FormatException("Incorrect number of parameters");

                        configVarList.Add(new CSVCommandType()
                        {
                            CommandName = values[0].Trim(),
                            CVNumber = int.Parse(values[1].Trim()),
                            Parameter = byte.Parse(values[2].Trim()),
                            Comment = values[3].Trim()
                        });

                        foreach (CSVCommandType item in configVarList)
                        {
                            switch (item.CommandName.ToUpper())
                            {
                                case "SETBYTE":
                                    DecoderConfiguration.ConfigurationVariables[item.CVNumber].Value = item.Parameter;
                                    break;
                                case "SETBIT":
                                    DecoderConfiguration.ConfigurationVariables[item.CVNumber].Value = Bit.Set(DecoderConfiguration.ConfigurationVariables[item.CVNumber].Value, item.Parameter, true);
                                    break;
                                case "CLEARBIT":
                                    DecoderConfiguration.ConfigurationVariables[item.CVNumber].Value = Bit.Set(DecoderConfiguration.ConfigurationVariables[item.CVNumber].Value, item.Parameter, false);
                                    break;
                                default:
                                    throw new FormatException("Unknown command " + item.CommandName);
                            }
                        }
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("CSVReader:ReadFile " + ex.Message);
                return false;
            }

        }
    }
}
