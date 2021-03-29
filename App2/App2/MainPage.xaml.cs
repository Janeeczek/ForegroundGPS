using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using FCM;

namespace App2
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        INotificationManager notificationManager;
        IMyLocation loc;
        public MainPage()
        {
            InitializeComponent();
            check();
            loc = DependencyService.Get<IMyLocation>();
            loc.locationObtained += (object sender, ILocationEventArgs e) => {
                    var lat = e.lat;
                    var lng = e.lng;
                    x.Text = lat.ToString();
                    y.Text = lng.ToString();
                };
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
            
        }
        void OnSendClick(object sender, EventArgs e)
        {
            string title = $"Uruchomiono usluge";
            string message = $"Usługa lokalizacji w tle została włączona";
            x.Text = "Pobieram lokalizacje X";
            y.Text = "Pobieram lokalizacje Y";
            notificationManager.SendNotification(title, message);
            DependencyService.Get<IAndroidService>().StartService();
        }
        async void check()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
                }

                if (status == PermissionStatus.Granted)
                {

                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
                
            }
            catch (Exception ex)
            {
            }
        }
        void OnScheduleClick(object sender, EventArgs e)
        {
            DependencyService.Get<IAndroidService>().StopService();
            string title = $"Zamykam usługe";
            string message = $"Usługa lokalizacji w tle została wyłączona";
            x.Text = "Nie pobieram już lokalizacji X";
            y.Text = "Nie pobieram już lokalizacji Y";
            notificationManager.SendNotification(title, message, DateTime.Now.AddSeconds(10));
        }

        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };
                stackLayout.Children.Add(msg);
            });
        }
    }
}
