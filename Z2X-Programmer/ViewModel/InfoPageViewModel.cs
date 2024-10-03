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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class InfoPageViewModel: ObservableObject
    {

        #region REGION: PUBLIC PROPERTIES
    
        [ObservableProperty]
        string applicationName;

        [ObservableProperty]
        string packageName;

        [ObservableProperty]
        string applicationVersion;

        #endregion

        #region REGION: CONSTRUCTOR

        public InfoPageViewModel()
        {
            ApplicationName = AppInfo.Current.Name;
            PackageName = AppInfo.Current.PackageName;
            ApplicationVersion = AppInfo.Current.VersionString;
        }

        #endregion

        #region REGION: COMMANDS  

        [RelayCommand]
        async Task OpenLink(string url)
        {
            try
            {
                await Browser.OpenAsync(url);
            }
            catch (Exception ex)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, ex.Message, AppResources.OK);
                }
            }
        }

        #endregion        


    }
}
