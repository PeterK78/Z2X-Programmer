/*

Z2X-Programmer
Copyright (C) 2025
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
using System.ComponentModel;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.Helper
{
    /// <summary>
    /// This class is responsible for managing the undo and redo operations.
    /// </summary>
    public static class UndoRedoManager
    {
        public static event PropertyChangedEventHandler? PropertyChanged;

        private static List<UndoRedoType> UndoInformation = new List<UndoRedoType>();
        private static List<UndoRedoType> RedoInformation = new List<UndoRedoType>();

        private static bool FilterNextUndoEvent = false;

        /// <summary>
        /// Is TRUE if there is a redo is available.
        /// </summary>
        public static bool RedoAvailable
        {
            get
            {
                if (RedoInformation.Count > 0) return true;
                return false;
            }
        }

        /// <summary>
        /// Is TRUE if the UndoRedoManager is enabled.
        /// </summary>
        public static bool Enabled { get; set; } = true;

        /// <summary>
        /// Is TRUE if there is an undo available.
        /// </summary>
        public static bool UndoAvailable
        {
            get
            {
                if (UndoInformation.Count > 0) return true;
                return false;
            }
        }

        /// <summary>
        /// Initializes the UndoRedoManager.
        /// </summary>
        public static void Init()
        {
            WeakReferenceMessenger.Default.Register<UndoRedoMessage>(typeof(UndoRedoManager), (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    AddCVChange(m.Value);
                });
            });

        }

        /// <summary>
        /// Resets the UndoRedoManager.
        /// </summary>
        public static void Reset()
        {
            UndoInformation.Clear();
            RedoInformation.Clear();
            OnPropertyChanged(nameof(UndoAvailable));
            OnPropertyChanged(nameof(RedoAvailable));
        }


        /// <summary>
        /// Is called when a CV is modified. It adds the information about the modification
        /// to the UndoInformation list.
        /// </summary>
        /// <param name="cvModifiedInfo"></param>
        public static void AddCVChange(UndoRedoType cvModifiedInfo)
        {
            //  Check if the UndoRedoManager is enabled.
            if (Enabled == false) return;

            //  Check if we have to handle this undo event.
            if (FilterNextUndoEvent == true)
            {
                FilterNextUndoEvent = false;
                return;
            }


            //  Add the undo information to the list.
            AddUndoInformation(cvModifiedInfo);

            //Logger.PrintDevConsole("UndoRedoManager: New undo information available (CV:" + cvModifiedInfo.CVNumber + " Old value:" + cvModifiedInfo.OldValue + " New value:" + cvModifiedInfo.NewValue + ")");

        }

        /// <summary>
        /// Adds a new entry to the undo information list.
        /// </summary>
        /// <param name="undoInfo"></param>
        private static void AddUndoInformation (UndoRedoType undoInfo)
        {
            //  Check if the undo information list is too long. If so remove the first element.
            if (UndoInformation.Count > 50)   UndoInformation.RemoveAt(0);

            //  Add the undo information to the list.
            UndoInformation.Add(undoInfo);

            //  Inform the UI that an undo operation is available.
            OnPropertyChanged(nameof(UndoAvailable));
        }

        /// <summary>
        /// This function is called when the user wants to undo the last change.
        /// </summary>
        public static void UndoLastCVChange()
        {
            //  Check if an undo information is available - if not just return.
            if(UndoAvailable == false) return;

            //  Grab the last undo information.
            UndoRedoType lastUndoInfo = GrabLastUndoInformation()!;
            if(lastUndoInfo == null) return;

            //  Add the last undo information to the redo list.
            RedoInformation.Add(lastUndoInfo);
            OnPropertyChanged(nameof(RedoAvailable));

            //  Important:
            //  We need to suppress the handling of the next undo event in function AddCVChange.
            FilterNextUndoEvent = true;

            //  Set the old value of the CV.
            DecoderConfiguration.ConfigurationVariables[lastUndoInfo.CVNumber].Value = lastUndoInfo.OldValue;

            
        }

        /// <summary>
        /// This function is called when the user wants to redo the last undo operation.
        /// </summary>
        public static void RedoLastUndo()
        {
            //  Check if a redo information is available - if not just return.  
            if (RedoAvailable == false) return;

            //  Grab the last redo information.
            UndoRedoType lastRedoInfo = GrabLastRedoUInformation();

            //  Set the new value of the CV.
            DecoderConfiguration.ConfigurationVariables[lastRedoInfo.CVNumber].Value = lastRedoInfo.NewValue;

        }

        /// <summary>
        /// Grabs the last undo information from the list.
        /// </summary>
        /// <returns></returns>
        private static UndoRedoType? GrabLastUndoInformation()
        {
            //  Check if there is any undo information available.
            if (UndoInformation.Count == 0) return null;

            //  Grab the latest undo information.
            UndoRedoType item =  UndoInformation[UndoInformation.Count - 1];

            //  Remove the latest undo information from the list.
            UndoInformation.RemoveAt(UndoInformation.Count - 1);

            //  Inform the UI that no more undo operations are available.
            if (UndoInformation.Count == 0) OnPropertyChanged(nameof(UndoAvailable));

            return item;

        }

        /// <summary>
        /// Grabs the last redo information from the list.
        /// </summary>
        /// <returns></returns>
        private static UndoRedoType GrabLastRedoUInformation()
        {
            //  Check if there is any redo information available.
            if (RedoInformation.Count == 0) return null!;

            //  Grab the latest redo information.
            UndoRedoType item = RedoInformation[RedoInformation.Count - 1];

            // Remove the latest redo information from the list.
            RedoInformation.RemoveAt(RedoInformation.Count - 1);

            return item;    
        }

        private static void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName)); }



    }


}
