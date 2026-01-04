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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class SettingsSearchViewModel : ObservableObject
    {

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        string selectedSearchResult;

        [ObservableProperty]
        List<string> searchResults;

        #endregion

        #region REGION: CONSTRUCTOR

        public SettingsSearchViewModel()
        {

            selectedSearchResult = "";
            SearchResults = new List<string>();

            OnGetDecoderConfiguration();

            WeakReferenceMessenger.Default.Register<DecoderConfigurationUpdateMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDecoderConfiguration();
                });
            });

        }
        #endregion

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        public void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;
        }

        #endregion

        #region REGION: COMMANDS

        /// <summary>
        /// Handles the input of a new search text
        /// </summary>
        /// <param name="searchText"></param>
        [RelayCommand]
        private void SearchTextChanged(string searchText)
        {
            SearchResults = SettingsSearcher.GetResults(searchText);
        }

        /// <summary>
        /// Is called when the search result is clicked in the search result listview. It will use the search result to find the
        ///  search target. Is the target found, it will jump to the desired page.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task SearchResultSelected()
        {

            try
            {
                string pageName = "";
                string targetLabel = "";

                if (SettingsSearcher.GetNavigationTarget(SelectedSearchResult, out pageName, out targetLabel) == true)
                {
                    var navigationParameter = new ShellNavigationQueryParameters
                    {
                        { "SearchTarget", targetLabel }
                    };
                    Shell.Current.CurrentItem = Shell.Current.Items[0];
                    await Shell.Current.GoToAsync("//" + pageName, navigationParameter);
                }
            }
            catch (Exception ex)
            {
                
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        public ICommand PerformSearch => new Command<string>((string query) =>
        {
            SearchResults = SettingsSearcher.GetResults(query);
        });
        #endregion

    }
}
