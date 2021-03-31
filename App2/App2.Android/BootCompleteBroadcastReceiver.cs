using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace App2.Droid
{

    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted, "android.intent.action.QUICKBOOT_POWERON", Intent.ActionScreenOn })]
    public class BootCompleteBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Toast.MakeText(context, "Action Boot Completed!", ToastLength.Long).Show();
            //var jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
            //DependencyService.Get<IAndroidService>().StartService();
            //Intent serviceStart = new Intent(context, typeof(MainActivity));
            //serviceStart.AddFlags(ActivityFlags.NewTask);
            // context.StartActivity(serviceStart);
            /*
            if (intent.Action == Intent.ActionBootCompleted)
            {
                var intenta = new Intent(context, typeof(SimpleService));

                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    Toast.MakeText(context, "11111111", ToastLength.Long).Show();
                    context.StartForegroundService(intenta);
                }
                else
                {
                    Toast.MakeText(context, "22222222", ToastLength.Long).Show();
                    context.StartService(intenta);
                }
            }
            */
            Intent myIntent = new Intent(context, typeof(MainActivity));
            myIntent.AddFlags(ActivityFlags.NewTask);
            context.StartActivity(myIntent);

        }
    }
}