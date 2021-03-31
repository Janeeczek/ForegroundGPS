using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
    public interface IAndroidService
    {
        void StartService();

        void StopService();
        //static bool serviceStarted;
    }
}
