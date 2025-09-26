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

using Syncfusion.Maui.Toolkit.Charts;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.ViewModel;

namespace Z2XProgrammer.Helper
{
    public class ChartInteractionExt : ChartInteractiveBehavior
    {
        bool dragEnabled = false;
        int selectedIndex;

        protected override void OnTouchUp(ChartBase chart, float pointX, float pointY)
        {
            base.OnTouchUp(chart, pointX, pointY);

            //  We do nothing, if we are not dragging.            
            if (dragEnabled == false) return;

            // We also do nothing, if no point is selected.
            if (selectedIndex == -1) return;

            if (chart is SfCartesianChart cartesianChart)
            {
                double newValue = cartesianChart.PointToValue(cartesianChart.YAxes[0], pointX, pointY);
                if (cartesianChart.BindingContext is MotorCharacteristicsViewModel viewModel)
                {
                    double y = cartesianChart.PointToValue(cartesianChart.YAxes[0], pointX, pointY);
                    if (y < cartesianChart.YAxes[0].VisibleMinimum) y = 0;
                    if (y > cartesianChart.YAxes[0].VisibleMaximum) y = cartesianChart.YAxes[0].VisibleMaximum;

                    ConfigurationVariableType myItem = viewModel!.ExtendedSpeedCurveValues![selectedIndex];
                    if (myItem == null) return;

                    myItem.Value = (byte)y;
                    viewModel.ExtendedSpeedCurveValues.RemoveAt(selectedIndex);
                    viewModel.ExtendedSpeedCurveValues.Insert(selectedIndex, myItem);
                }
            }

            dragEnabled = false;

        }

        protected override void OnTouchDown(ChartBase chart, float pointX, float pointY)
        {
            base.OnTouchDown(chart, pointX, pointY);

            if (chart is SfCartesianChart cartesianChart)
            {
                // Find the index of the point that is touched. -1 means no point is touched. 
                selectedIndex = cartesianChart.Series[0].GetDataPointIndex(pointX, pointY);
            }

        }

        protected override void OnTouchMove(ChartBase chart, float pointX, float pointY)
        {
            base.OnTouchMove(chart, pointX, pointY);
            dragEnabled = true;
        }
    }
}
