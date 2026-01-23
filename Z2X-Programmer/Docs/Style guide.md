# Z2X-Programmer style guide

## Responsive design pattern

### Simple number widget

```
<!-- We use a label with the style Z2XProgrammerLabelHeading2Style as the heading -->
<Label Style="{StaticResource Z2XProgrammerLabelHeading2Style}" Text="{x:Static strings:AppResources.FrameVehicleAddressTitle}" VerticalOptions="Center"  HorizontalOptions="Start" />

<!-- We surround the entire widget with a border  -->
<Border Style="{StaticResource Z2XBorderFrame}">

	<!-- Our widget should not extend beyond the screen. For this reason, we place the widget's content in a grid. -->	
	<Grid>		
	<!--
		Our grid consists of three columns:
		Column 1 contains the icon with a fixed width of 40
		Column 2 contains the the description of the widget and the description of the value
		Column 3 contains the value
	-->
	<Grid.ColumnDefinitions>
		<ColumnDefinition Width="40"></ColumnDefinition>
		<ColumnDefinition Width="*"></ColumnDefinition>
		<ColumnDefinition Width="*"></ColumnDefinition>
	</Grid.ColumnDefinitions>

	<!-- Our grid consists of two rows:
		Row 1 contains the icon and the description of the widget
		Row 2 contains the description of the value and the value
	-->
	<Grid.RowDefinitions>
		<RowDefinition Height="*"></RowDefinition>
		<RowDefinition Height="*"></RowDefinition>
	</Grid.RowDefinitions>

        <!-- The icon of the widget in row 0, column 0 -->
        <Image Margin="0,5,0,0" Grid.Row="0" Grid.Column="0" Source="{AppThemeBinding Light=ic_fluent_vehicle_address_24_regular.png, Dark=ic_fluent_vehicle_address_24_dark.png}" HorizontalOptions="Start" VerticalOptions="Start" HeightRequest="24"></Image>

	<!-- The description of the widget in row 0, column 1 -->
        <Label Margin="0,0,0,20"  Grid.Row="0" Grid.Column="1" Grid.ColumnSpan="2" Style="{StaticResource Z2XProgrammerLabelStandardStyle}"  Text="{x:Static strings:AppResources.FrameLocomotiveAddressDescription}" VerticalOptions="Start"  HorizontalOptions="Start" LineBreakMode="WordWrap"/>

        <!--	To ensure that the description of the value and the input of the value are fully responsive, we put them in a FlexLayout.
		The direction of the FlexLayout is then determined between
		Column and Row using OrientationStateTrigger and AdaptiveTrigger. 
		-->
        <FlexLayout Direction="Row" JustifyContent="SpaceBetween" Grid.Row="1" Grid.Column="1" Grid.ColumnSpan="2" HorizontalOptions="End">

		<!-- We use the following configuration of the VisualStateManager to switch between a horizontal and vertical layout -->
		<VisualStateManager.VisualStateGroups>
				<VisualStateGroup>

					<!-- Portrait -->
					<VisualState x:Name="Portrait">
						<VisualState.StateTriggers>
							<OrientationStateTrigger Orientation="Portrait" />
						</VisualState.StateTriggers>
						<VisualState.Setters>
							<Setter Property="Direction" Value="Column" />
						</VisualState.Setters>
					</VisualState>

					<!-- LandscapeVertical -->
					<VisualState x:Name="LandscapeVertical">
						<VisualState.StateTriggers>
							<AdaptiveTrigger MinWindowWidth="0"/>
						</VisualState.StateTriggers>
						<VisualState.Setters>
							<Setter Property="Direction" Value="Column" />
						</VisualState.Setters>
					</VisualState>

					<!-- LandscapeHorizontal -->
					<VisualState x:Name="LandscapeHorizontal">
						<VisualState.StateTriggers>
							<AdaptiveTrigger MinWindowWidth="950"/>
							<OrientationStateTrigger Orientation="Landscape" />
						</VisualState.StateTriggers>
						<VisualState.Setters>
							<Setter Property="Direction" Value="Row" />
						</VisualState.Setters>
					</VisualState>
				</VisualStateGroup>
			</VisualStateManager.VisualStateGroups>

			<Label x:Name="FrameAddressVehicleAddressLabel" Style="{StaticResource Z2XProgrammerLabelHeading3Style}" HorizontalOptions="Start" Text="{x:Static strings:AppResources.FrameAddressVehicleAddressLabel}" VerticalOptions="Center" />
			<VerticalStackLayout>
				<Entry  MinimumWidthRequest="200" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" HorizontalOptions="Start" VerticalOptions="Center" Placeholder="{x:Static strings:AppResources.FrameLocomotiveAddressPlaceholder}" Text="{Binding VehicleAddress}" Keyboard="Numeric" ClearButtonVisibility="WhileEditing" >
					<Entry.Behaviors>
						<toolkit:NumericValidationBehavior                                     
							BindingContext="{Binding BindingContext, Source={x:Reference ThisPage}, x:DataType=ContentPage}"
							InvalidStyle="{StaticResource Z2XInvalidEntry}"
							ValidStyle="{StaticResource Z2XValidEntry}"
							Flags="ValidateOnValueChanged"
							MinimumValue="{Binding LimitMinimumAddress}"
							MaximumValue="{Binding LimitMaximumAddress}"
							MaximumDecimalPlaces="0" />
					</Entry.Behaviors>
				</Entry>
				<Label IsVisible="{Binding AdditionalDisplayOfCVValues}" Text="{Binding VehicleAddressCVConfiguration}" Style="{StaticResource Z2XProgrammerLabelAdditionalCVValuesStyle}" HorizontalOptions="Center" HorizontalTextAlignment="Center"></Label>
			</VerticalStackLayout>
		</FlexLayout>
	</Grid>
</Border>
```

## Picker
* HorizontalTextAlignment="End"

## Entry
* HorizontalTextAlignment="End"
* Wrong input is highlighted with a red background color

## Representation of the units
If the set value is also displayed in a specific unit (e.g. seconds, percent etc.), we always display the value from the CV first and then the value with the unit in brackets.

