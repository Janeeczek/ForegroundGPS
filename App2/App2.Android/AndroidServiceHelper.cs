using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App2.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidServiceHelper))]
namespace App2.Droid
{
    internal class AndroidServiceHelper : IAndroidService
    {
        private static Context context = global::Android.App.Application.Context;

        public void StartService()
        {
            var intent = new Intent(context, typeof(SimpleService));
            
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                
                context.StartForegroundService(intent);
            }
            else
            {
               
                context.StartService(intent);
            }
        }

        public void StopService()
        {
            
            var intent = new Intent(context, typeof(SimpleService));
            context.StopService(intent);
        }
    }
}