using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App2.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(GetMyLocation))]
namespace App2.Droid
{
    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class GetMyLocation : Java.Lang.Object,
                                IMyLocation,
                                ILocationListener
    {
        LocationManager lm;
        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider,
            Availability status, Android.OS.Bundle extras)
        { }
        //---fired whenever there is a change in location---
        public void OnLocationChanged(Location location)
        {
            if (location != null)
            {
                LocationEventArgs args = new LocationEventArgs();
                args.lat = location.Latitude;
                args.lng = location.Longitude;
                locationObtained(this, args);
            };
        }
        //---an EventHandler delegate that is called when a location
        // is obtained---
        public event EventHandler<ILocationEventArgs>
            locationObtained;
        //---custom event accessor that is invoked when client
        // subscribes to the event---
        event EventHandler<ILocationEventArgs>
            IMyLocation.locationObtained
        {
            add
            {
                locationObtained += value;
            }
            remove
            {
                locationObtained -= value;
            }
        }
        //---method to call to start getting location---
        public void ObtainMyLocation()
        {
            lm = (LocationManager)
                Forms.Context.GetSystemService(
                    Context.LocationService);
            lm.RequestLocationUpdates(
                LocationManager.NetworkProvider,
                    0,   //---time in ms---
                    0,   //---distance in metres---
                    this);
        }
        //---stop the location update when the object is set to
        // null--
        public void Destroy()
        {
            lm.RemoveUpdates(this);
        }
        ~GetMyLocation()
        {
            lm.RemoveUpdates(this);
        }
    }
}