using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;

using AndroidX.Core.App;
using App2.Droid;

using Firebase.Messaging;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using System.Collections.Generic;
using static AndroidX.Core.App.NotificationCompat;

namespace App2
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        const string channelId = "default";
        int messageId = 0;
        int pendingIntentId = 0;
        INotificationManager notificationManager =  DependencyService.Get<INotificationManager>();
        public MyFirebaseMessagingService()
        {

        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            if (message.Data.ContainsKey("Test")) Console.WriteLine("TEASD3423423423434SDASDDASASDSD(_(()@*#$()@#$()#($)#@&$): ASDASDASD");
            Console.WriteLine(TAG+ "From: " + message.From);
            IDictionary<string, string> data = message.Data;

            foreach (KeyValuePair<string, string> kvp in data)
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
            var name = string.Empty;
            
            if (message.Data.Count > 0)
            {
                Console.WriteLine(TAG + "Message data payload: " + message.Data);
                if (message.Data.ContainsKey("text"))
                    name = message.Data["text"];
            }

            

            if (!string.IsNullOrEmpty(name))
                SendNotification("Z", message.Data);
            else
                SendNotification("BEZ", message.Data);
            //notificationManager.SendNotification(message.GetNotification().Title, message.GetNotification().Body);
            
        }
        public override void OnNewToken(String s)
        {
            base.OnNewToken(s);
            Console.WriteLine("TOKEN " + s);
        }
        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.navigation_empty_icon);

            BigPictureStyle picStyle = new BigPictureStyle();
            picStyle.BigPicture(bitmap);



            picStyle.SetSummaryText("This is a BigPicture");
            
            var pendingIntent = PendingIntent.GetActivity(this, pendingIntentId++, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, channelId)
                                      .SetSmallIcon(Resource.Drawable.navigation_empty_icon)
                                      .SetContentTitle("FCM Message")
                                      .SetContentText(messageBody)
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent).SetStyle(picStyle);
            //.SetStyle(picStyle);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(messageId++, notificationBuilder.Build());
        }
    }
}
