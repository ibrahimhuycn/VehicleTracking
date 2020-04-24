using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VehicleTracking.Models;
using VehicleTracking.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace VehicleTracking
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private MapSpan span;
        public MainPage()
        {
            InitializeComponent();
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            Geolocation.GetLocationAsync(request);
            _viewModel.UniqueVehicleName = "Thrombocyte";
            _viewModel.VehicleLocationsUpdated += ViewModel_VehicleLocationsUpdated;
        }

        private void ViewModel_VehicleLocationsUpdated(object sender, List<Models.VehicleLocationModel> e)
        {
            if (MainThread.IsMainThread)
            {
                CreatePinsForUpdatedLocations(e);
                ZoomToDeviceLocation(e);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(new Action(() =>
                {
                    CreatePinsForUpdatedLocations(e);
                    ZoomToDeviceLocation(e);
                }));
            }

        }

        private void ZoomToDeviceLocation(List<VehicleLocationModel> e)
        {
            //find the current devices location and zoom-in.
            var MyLocation = e.Find(x => x.UniqueVehicleName == _viewModel.UniqueVehicleName);

            if (MyLocation is null) { MyLocation = e.FirstOrDefault(); }
            if (MyLocation is null) { return; }

            ZoomInOnLocation(MyLocation);
        }

        private void ZoomInOnLocation(VehicleLocationModel myLocation)
        {
            try
            {
                var mapSpan = MapSpan.FromCenterAndRadius
                    (new Position(myLocation.Latitude, myLocation.Longitude), Distance.FromKilometers(0.1));

                span = mapSpan;

                mapOnDevice.MoveToRegion(mapSpan);


            }
            catch (Exception ex)
            {

                _viewModel.ErrorMessage = ex.Message;
            }

        }

        private void CreatePinsForUpdatedLocations(List<VehicleLocationModel> e)
        {
            //clear all pins
            mapOnDevice.Pins.Clear();

            //add pins
            foreach (var vehicle in e)
            {
                mapOnDevice.Pins.Add(new Pin()
                {
                    Position = new Position(vehicle.Latitude, vehicle.Longitude),
                    Label = vehicle.UniqueVehicleName,
                    Type = PinType.Generic
                });
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            mapOnDevice.MoveToRegion(span);
        }

    }
}