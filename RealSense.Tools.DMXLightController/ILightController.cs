using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IVCam.Tools.Infrastructure.Logging;
using System.Threading.Tasks;
using RealSense.Tools.ProjectorTester.Cmc;
using System.Threading.Tasks;

namespace RealSense.Tools.IVCam2.Validation.Main
{
    interface ILightController
    {
        void TurnTesterLightOff(Logger logger);
        void TurnTesterLightOn(Logger logger);
        




        void SetStartupLights();
        void SetFailureLights();
        void SetPassLights();
        void SetQualityLights();
        bool Init(Logger logger);
    }
}
