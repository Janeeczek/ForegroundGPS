using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App2.Droid
{
    [BroadcastReceiver(Name = "com.companyname.app2.OutgoingCallBroadcastReceiver")]
    [IntentFilter(new[] { Intent.ActionNewOutgoingCall, TelephonyManager.ActionPhoneStateChanged })]
    public class OutgoingCallBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            switch (intent.Action)
            {
                case Intent.ActionNewOutgoingCall:
                    var outboundPhoneNumber = intent.GetStringExtra(Intent.ExtraPhoneNumber);
                    Toast.MakeText(context, $"Rozpoczynam: połączenie do {outboundPhoneNumber}", ToastLength.Long).Show();
                    break;
                case TelephonyManager.ActionPhoneStateChanged:
                    var state = intent.GetStringExtra(TelephonyManager.ExtraState);
                    if (state == TelephonyManager.ExtraStateIdle)
                        Toast.MakeText(context, "SKOŃCZONO POŁACZENIE", ToastLength.Long).Show();
                    else if (state == TelephonyManager.ExtraStateOffhook)
                        Toast.MakeText(context, "TRWA POŁĄCZENIE", ToastLength.Long).Show();
                    else if (state == TelephonyManager.ExtraStateRinging)
                        Toast.MakeText(context, "KTOŚ DZWONI", ToastLength.Long).Show();
                    else if (state == TelephonyManager.ExtraIncomingNumber)
                    {
                        var incomingPhoneNumber = intent.GetStringExtra(TelephonyManager.ExtraIncomingNumber);
                        Toast.MakeText(context, $"Dzwoni do mnie: {incomingPhoneNumber}", ToastLength.Long).Show();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}