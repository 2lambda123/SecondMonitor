﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SecondMonitor.DataModel;
using SecondMonitor.DataModel.BasicProperties;
using SecondMonitor.Timing.Annotations;
using SecondMonitor.Timing.Model.Settings.Model;

namespace SecondMonitor.Timing.Model.Settings.ModelView
{
    public class DisplaySettingsModelView : DependencyObject, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TemperatureUnitsProperty = DependencyProperty.Register("TemperatureUnits", typeof(TemperatureUnits),typeof(DisplaySettingsModelView), new PropertyMetadata { PropertyChangedCallback = PropertyChangedCallback });
        public static readonly DependencyProperty PressureUnitsProperty = DependencyProperty.Register("PressureUnits", typeof(PressureUnits), typeof(DisplaySettingsModelView), new PropertyMetadata { PropertyChangedCallback = PropertyChangedCallback });
        public static readonly DependencyProperty VolumeUnitsProperty = DependencyProperty.Register("VolumeUnits", typeof(VolumeUnits), typeof(DisplaySettingsModelView), new PropertyMetadata { PropertyChangedCallback = PropertyChangedCallback });
        public static readonly DependencyProperty FuelCalculationScopeProperty = DependencyProperty.Register("FuelCalculationScope", typeof(FuelCalculationScope), typeof(DisplaySettingsModelView), new PropertyMetadata{ PropertyChangedCallback = PropertyChangedCallback});
        public static readonly DependencyProperty PaceLapsProperty = DependencyProperty.Register("PaceLaps", typeof(int), typeof(DisplaySettingsModelView), new PropertyMetadata { PropertyChangedCallback = PropertyChangedCallback });
        public static readonly DependencyProperty RefreshRateProperty = DependencyProperty.Register("RefreshRate", typeof(int), typeof(DisplaySettingsModelView), new PropertyMetadata { PropertyChangedCallback = PropertyChangedCallback });
        public static readonly DependencyProperty ScrollToPlayerProperty = DependencyProperty.Register("ScrollToPlayer", typeof(bool), typeof(DisplaySettingsModelView), new PropertyMetadata { PropertyChangedCallback = PropertyChangedCallback });

        public event PropertyChangedEventHandler PropertyChanged;

        public TemperatureUnits TemperatureUnits
        {
            get => (TemperatureUnits)GetValue(TemperatureUnitsProperty);
            set => SetValue(TemperatureUnitsProperty, value);
        }

        public PressureUnits PressureUnits
        {
            get => (PressureUnits)GetValue(PressureUnitsProperty);
            set => SetValue(PressureUnitsProperty, value);
        }

        public VolumeUnits VolumeUnits
        {
            get => (VolumeUnits)GetValue(VolumeUnitsProperty);
            set => SetValue(VolumeUnitsProperty, value);
        }

        public FuelCalculationScope FuelCalculationScope
        {
            get => (FuelCalculationScope) GetValue(FuelCalculationScopeProperty);
            set => SetValue(FuelCalculationScopeProperty, value);
        }

        public int PaceLaps
        {
            get => (int) GetValue(PaceLapsProperty);
            set => SetValue(PaceLapsProperty, value);
        }

        public int RefreshRate
        {
            get => (int) GetValue(RefreshRateProperty);
            set => SetValue(RefreshRateProperty, value);
        }

        public bool ScrollToPlayer
        {
            get => (bool) GetValue(ScrollToPlayerProperty);
            set => SetValue(ScrollToPlayerProperty, value);
        }


        public void FromModel(DisplaySettings settings)
        {
            TemperatureUnits = settings.TemperatureUnits;
            PressureUnits = settings.PressureUnits;
            VolumeUnits = settings.VolumeUnits;
            FuelCalculationScope = settings.FuelCalculationScope;
            PaceLaps = settings.PaceLaps;
            RefreshRate = settings.RefreshRate;
            ScrollToPlayer = settings.ScrollToPlayer;
        }

        public DisplaySettings ToModel()
        {
            return new DisplaySettings()
            {
                TemperatureUnits = TemperatureUnits,
                PressureUnits = PressureUnits,
                VolumeUnits = VolumeUnits,
                FuelCalculationScope = FuelCalculationScope,
                PaceLaps = PaceLaps,
                RefreshRate = RefreshRate,
                ScrollToPlayer = ScrollToPlayer,

            };
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            DisplaySettingsModelView sender = (DisplaySettingsModelView) dependencyObject;
            sender.OnPropertyChanged(dependencyPropertyChangedEventArgs.Property.Name);
        }
        

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
