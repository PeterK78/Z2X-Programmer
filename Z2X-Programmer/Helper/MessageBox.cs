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

namespace Z2XProgrammer.Helper
{
    /// <summary>
    /// Contains the implementation of the MessageBox package.
    /// </summary>
    internal static class MessageBox
    {

        /// <summary>
        /// Displays an alert dialog to the application user with a single cancel button.
        /// </summary>
        /// <param name="title">The title of the alert dialog. Can be null to hide the title.</param>
        /// <param name="message">The body text of the alert dialog.</param>
        /// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
        /// <param name="accept">Text to be displayed on the 'Accept' button. Can be null to hide this button.</param>
        /// <returns></returns>
        public async static Task<bool>Show(string title, string message, string accept, string cancel)
		{
		    try
            {
                if (Application.Current == null) return false;
                if (Application.Current.Windows == null) return false;
                if (Application.Current.Windows.Count == 0) return false;
                if (Application.Current.Windows[0] == null) return false;
                if (Application.Current.Windows[0].Page == null) return false;

                return await Application.Current.Windows[0].Page!.DisplayAlertAsync(title, message, accept, cancel,FlowDirection.MatchParent);
            }
            catch (Exception ex) 
            {
                Logger.PrintDevConsole("MessageBox.Show  " + ex.Message);
                return false;   
            }  
		}

        /// <summary>
        /// Displays an alert dialog to the application user with a single cancel button.
        /// </summary>
        /// <param name="title">The title of the alert dialog. Can be null to hide the title.</param>
        /// <param name="message">The body text of the alert dialog.</param>
        /// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
        /// <returns></returns>
        public async static Task Show(string title, string message, string cancel)
        {

            try
            {
                if (Application.Current == null) return;
                if (Application.Current.Windows == null) return;
                if (Application.Current.Windows.Count == 0) return;
                if (Application.Current.Windows[0] == null) return;
                if (Application.Current.Windows[0].Page == null) return;

                await Application.Current.Windows[0].Page!.DisplayAlertAsync(title, message, cancel);

                return;
            }
            catch (Exception ex) 
            {
                 Logger.PrintDevConsole("MessageBox.Show  " + ex.Message);
            }   
        }    
    }
}
