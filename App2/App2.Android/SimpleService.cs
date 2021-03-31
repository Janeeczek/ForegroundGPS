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
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Threading;

namespace App2.Droid
{
    //[Service(IsolatedProcess = true)]
    [Service]
    class SimpleService : Service
    {
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        private PowerManager.WakeLock wakeLock = null;
        int number = 0;
        IMyLocation loc;
        INotificationManager notificationManager;
        double x, y;
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Console.WriteLine("OnStartCommand");
            CreateNotificationChannel();
            //loc = DependencyService.Get<IMyLocation>();
            string messageBody = "Aplikacja pobiera lokalizacje";
            // / Create an Intent for the activity you want to start
            Intent resultIntent = new Intent(this, typeof(MainActivity));
            // Create the TaskStackBuilder and add the intent, which inflates the back stack
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
            stackBuilder.AddNextIntentWithParentStack(resultIntent);
            // Get the PendingIntent containing the entire back stack
            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
            var notification = new Notification.Builder(this, "10111")
             .SetContentIntent(resultPendingIntent)
             .SetContentTitle("Lokalizacja w tle")
             .SetContentText(messageBody)
             .SetSmallIcon(Resource.Drawable.navigation_empty_icon)
             .SetOngoing(true)
             .Build();
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            loc = DependencyService.Get<IMyLocation>();
            notificationManager = DependencyService.Get<INotificationManager>();
            loc.locationObtained += (object sender,
                ILocationEventArgs e) => {
                    var lat = e.lat;
                    var lng = e.lng;
                    x = lat;
                    y = lng;
                    string title = $"Aktualna lokalizacja";
                    string message = $"To: {x}|{y}";
                    notificationManager.SendNotification(title, message);
                    Console.WriteLine(lat.ToString() +" "+ lng.ToString());
                  
                };
            loc.ObtainMyLocation();         
            return StartCommandResult.Sticky;
        }
        public static bool ServiceStarted { get; private set; }
        public override void OnCreate()
        {
            Console.WriteLine("OnCreate");
            Toast.MakeText(this, "toast" + number++ + " Uruchiomiono usluge ", ToastLength.Short).Show();
            PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
            wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, this.PackageName);
            wakeLock.Acquire();

        }
        public override void OnRebind(Intent intent)
        {
            Console.WriteLine("OnRebind");
            Toast.MakeText(this, "toast" + number++ + " Uruchiomiono usluge ", ToastLength.Short).Show();
            

        }


        public override void OnDestroy()
        {
            Console.WriteLine("OnDestroy");
            Toast.MakeText(this, "" + number++ + " Zakonczono  usluge ", ToastLength.Short).Show();
            if (wakeLock != null)
            {
                wakeLock.Release();
                wakeLock = null;
            }
            loc.Destroy();
            base.OnDestroy();
            StopForeground(true);
        }
        public override IBinder OnBind(Intent intent)
        {
            Console.WriteLine("OnBind");
            return null;
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {

                return;
            }

            var channelName = "Channel name";
            var channelDescription = "Channel descipripn";
            var channel = new NotificationChannel("10111", channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };
            
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        


    }
}