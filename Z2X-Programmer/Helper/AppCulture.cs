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

using System.Globalization;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Helper
{
    public static class AppCulture
    {
        /// <summary>
        /// Sets the language of the application.
        /// </summary>
        /// <param name="languageKey"></param>
        public static void SetApplicationLanguageByKey(string languageKey)
        {
            if(string.Compare(languageKey.ToUpper(), AppConstants.PREFERENCES_LANGUAGE_KEY_GERMAN.ToUpper()) == 0)
            {
                CultureInfo cultureInfoDE = new CultureInfo("de-DE");
                AppCulture.SetCultureInfo(cultureInfoDE);
                return;
            }
            
            CultureInfo cultureInfoEN = new CultureInfo("en-US");
            AppCulture.SetCultureInfo(cultureInfoEN);
        }

        /// <summary>
        /// Sets the language of the application.
        /// </summary>
        /// <param name="languageDescription"></param>
        public static void SetApplicationLanguageByDescription(string languageDescription)
        {
            if(string.Compare(languageDescription, AppResources.FrameSettingsAppLanguageEnglish, StringComparison.OrdinalIgnoreCase) == 0)
            {
                CultureInfo cultureInfoEN = new CultureInfo("en-US");
                AppCulture.SetCultureInfo(cultureInfoEN);
                return;
            }
            if (string.Compare(languageDescription, AppResources.FrameSettingsAppLanguageGerman, StringComparison.OrdinalIgnoreCase) == 0)
            {
                CultureInfo cultureInfoDE = new CultureInfo("de-DE");
                AppCulture.SetCultureInfo(cultureInfoDE);
                return;
            }
            CultureInfo cultureInfoNeutral = new CultureInfo("en-US");
            AppCulture.SetCultureInfo(cultureInfoNeutral);
            return;
        }

        /// <summary>
        /// Returns a list with descriptions of available languages.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailableLanguagesDescriptions()
        {
            List<string> languages = new List<string>();
            languages.Add(AppResources.FrameSettingsAppLanguageEnglish);
            languages.Add(AppResources.FrameSettingsAppLanguageGerman);
            return languages;
        }


        /// <summary>
        /// Returns the description for the given language key.
        /// </summary>
        /// <param name="languageKey"></param>
        /// <returns></returns>
        public static string GetLanguageDescription (string languageKey)
        {
            if (string.Compare(languageKey, AppResources.FrameSettingsAppLanguageEnglish, StringComparison.OrdinalIgnoreCase) == 0) return AppResources.FrameSettingsAppLanguageEnglish;
            if (string.Compare(languageKey, AppResources.FrameSettingsAppLanguageGerman, StringComparison.OrdinalIgnoreCase) == 0) return AppResources.FrameSettingsAppLanguageGerman;
            return AppResources.FrameSettingsAppLanguageEnglish;
        }

        private static void SetCultureInfo(CultureInfo cultureInfo)
        {
            if (cultureInfo != CultureInfo.CurrentCulture)
            {
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
        }

        public static string GetLanguageKeyUsedByOS()
        {
            if (CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.ToUpper() == "DE") return AppConstants.PREFERENCES_LANGUAGE_KEY_GERMAN;
            return AppConstants.PREFERENCES_LANGUAGE_KEY_ENGLISH;
        }

    }
}
