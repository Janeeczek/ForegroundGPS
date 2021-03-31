using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.Permissions;
using Xamarin.Forms;
using Firebase;
using Android.Gms.Common;
using Android;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using AndroidX.Core.App;

//using static Xamarin.Essentials.Platform;

namespace App2.Droid
{
    [Activity(Label = "App2", Icon = "@mipmap/icon", Theme = "@style/MainTheme", LaunchMode = LaunchMode.SingleTop, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        readonly string[] _permissionRequests =
            {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.Nfc,
                Manifest.Permission.WakeLock,
                Manifest.Permission.ReceiveBootCompleted,
                Manifest.Permission.ReorderTasks,
                
            };
        int REQUEST_PERMISSION_CODE = 1003;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            RequestPermission(Manifest.Permission.ReadPhoneState, Manifest.Permission.ProcessOutgoingCalls, Manifest.Permission.ReadPhoneNumbers, Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage);




            Task.Run(async () => await getRequiredPermissions(_permissionRequests));
            try
            {
                
                
                CreateNotificationFromIntent(Intent);
                FirebaseApp.InitializeApp(this);
                
                IsPlayServicesAvailable();
                LoadApplication(new App());
            }
            catch (Exception EX)
            {

            }


        }

        public void RequestPermission(params string[] permissions)
        {
            // Request required permission
            ActivityCompat.RequestPermissions(this, permissions, REQUEST_PERMISSION_CODE);
        }
        async Task getRequiredPermissions(String[] permissions)
        {
            var permissionsWeDontHave = new List<String>();

            foreach (var permission in permissions)
            {
                if (CheckSelfPermission(permission) != (int)Permission.Granted)
                    permissionsWeDontHave.Add(permission);
            }

            //Check if we already have all we need
            if (!permissionsWeDontHave.Any())
                return;

            bool fShowAlert = false;
            foreach (var permission in permissionsWeDontHave)
            {
                if (ShouldShowRequestPermissionRationale(permission))
                    fShowAlert = true;
            }

            if (fShowAlert)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("App Requires Permissions");
                builder.SetMessage("To Perform its function, app requires the permissions that follow to be granted!");
                builder.Show();
            }

            RequestPermissions(permissionsWeDontHave.ToArray(),22);
        }
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {

                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    //msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }

                else
                {
                    // msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                // do whatever if play service is not available
                //msgText.Text = "Google Play Services is available.";
                return true;
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }
        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
                string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }

    }

}