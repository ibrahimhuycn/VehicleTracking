using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using VehicleTracking.Models;
using VehicleTracking.Services;
using Xamarin.Essentials;

namespace VehicleTracking.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Timer refreshTimer;
        private readonly Timer locationReportingTimer;
        private readonly RestService api;
        private readonly List<VehicleLocationModel> vehicleLocationCurrentData;

        private string locationDisplayText;
        private string uniqueVehicleName;
        private bool isTrackingActive;
        private string checkBoxLabel;
        private string errorMessage;
        private bool isCheckboxEnabled;

        public event EventHandler<List<VehicleLocationModel>> VehicleLocationsUpdated;
        public MainViewModel()
        {
            api = new RestService();
            vehicleLocationCurrentData = new List<VehicleLocationModel>();

            refreshTimer = new Timer() { Interval = 6000, Enabled = false };
            locationReportingTimer = new Timer() { Interval = 6000, Enabled = false };

            refreshTimer.Elapsed += RefreshTimer_RealtimeLocations;
            locationReportingTimer.Elapsed += LocationReportingTimer_Elapsed;
            this.PropertyChanged += MainViewModel_PropertyChanged;

            IsCheckboxEnabled = true;
        }

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine(e.PropertyName);
            //UI rules: Tracking checkbox
            if (e.PropertyName == nameof(IsCheckboxEnabled))
            {
                if (!IsCheckboxEnabled) { CheckBoxLabel = "Processing..."; }

                if (IsCheckboxEnabled)
                {
                    if (IsTrackingActive) { CheckBoxLabel = "Tracking ACTIVE"; } 
                    else { CheckBoxLabel = "Tracking INACTIVE"; }
                }
            }

            //toast messages
            if(e.PropertyName == nameof(ErrorMessage))
            {
                Acr.UserDialogs.UserDialogs.Instance.Toast(ErrorMessage, new TimeSpan(5));
            }
        }

        public string LocationDisplayText
        {
            get => locationDisplayText; set
            {
                if (locationDisplayText == value) return;
                locationDisplayText = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => errorMessage; set
            {
                if (errorMessage == value) return;
                errorMessage = value;
                OnPropertyChanged();
            }
        }
        public string UniqueVehicleName
        {
            get => uniqueVehicleName; set
            {
                if (uniqueVehicleName == value) return;
                uniqueVehicleName = value;
                OnPropertyChanged();
            }
        }
        public bool IsTrackingActive
        {
            get => isTrackingActive; set
            {
                if (isTrackingActive == value) return;
                isTrackingActive = value;
                OnPropertyChanged();

                ManageTrackingStatus();

            }
        }

        private void ManageTrackingStatus()
        {
            if (!string.IsNullOrEmpty(uniqueVehicleName))
            {
                refreshTimer.Enabled = isTrackingActive;
                locationReportingTimer.Enabled = isTrackingActive;
            }
        }


        public string CheckBoxLabel
        {
            get => checkBoxLabel;  
            set
            {
                //if (checkBoxLabel == value) return;
                checkBoxLabel = value;
                OnPropertyChanged();
            }
        }

        public bool IsCheckboxEnabled
        {
            get => isCheckboxEnabled; set
            {
                if (isCheckboxEnabled == value) return;
                isCheckboxEnabled = value;
                OnPropertyChanged();

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void LocationReportingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsTrackingActive) return;
            try
            {
                var location = await RequestDeviceLocation();
                LocationDisplayText = $"{uniqueVehicleName} at: LAT: {location.Latitude} LONG: {location.Longitude}";
                var response = await api.ReportVehicleLocation(PreparetDataToReport(location));

                if (!IsInternetConnected())
                {
                    ErrorMessage = $"No Internet Connection while sending location... next try in {locationReportingTimer.Interval / 1000} seconds";
                    return;
                }
                if (!response) ErrorMessage = $"Cannot send Location at {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";

            }

        }

        private bool IsInternetConnected()
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            return true;
        }

        private VehicleLocationModel PreparetDataToReport(Location location)
        {
            return new VehicleLocationModel()
            {
                UniqueVehicleName = this.uniqueVehicleName,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };

        }

        private async Task<Location> RequestDeviceLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                return await Geolocation.GetLocationAsync(request);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async void RefreshTimer_RealtimeLocations(object sender, ElapsedEventArgs e)
        {
            refreshTimer.Enabled = false;

            try
            {
                if (!IsInternetConnected())
                {
                    ErrorMessage = $"No Internet Connection while fetching vehile locations... next try in {refreshTimer.Interval / 1000} seconds";
                    refreshTimer.Enabled = true;
                    return;
                }

                var updatedData = await api.GetAllVehicalLocations();
                ReplaceCurrentLocations(updatedData);
                VehicleLocationsUpdated?.Invoke(this, updatedData);
                refreshTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                refreshTimer.Enabled = true;
            }
        }

        private void ReplaceCurrentLocations(List<VehicleLocationModel> updatedData)
        {
            vehicleLocationCurrentData.Clear();
            foreach (var vehicle in updatedData)
            {
                vehicleLocationCurrentData.Add(vehicle);
            }
        }
    }
}
