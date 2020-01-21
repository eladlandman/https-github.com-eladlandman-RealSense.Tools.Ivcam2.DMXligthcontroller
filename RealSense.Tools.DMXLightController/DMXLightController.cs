using IVCam.Tools.Infrastructure.Logging;

using RealSense.Tools.ProjectorTester.Cmc;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSense.Tools.IVCam2.Validation.Main
{
    //Add option to read current light state so it won't be overridden by other lights settings
    class DMXLightController : ILightController
    {
        const int PortNumber = 3;
        SerialPort _serialPort;

        public bool Init(Logger logger=null)
        {
            _serialPort = new SerialPort("COM" + PortNumber);

            try
            {
                _serialPort.Open();
                return true;
            }
            catch (Exception e)
            {
                logger?.Error($"Failed init DMX serial port: {PortNumber}", e);
                return false;
            }
        }

        public void TurnTesterLightOff(Logger logger)
        {
            logger.Info("Setting tester light off");
            SendMessage(0, 0);
        }

        public void TurnTesterLightOn(Logger logger)
        {
            logger.Info("Setting tester light on");
            SendMessage(0xff, 0xff);
        }


        public void SetStartupLights()
        {
            SetLights(false, true, true, false);
        }

        public void SetFailureLights()
        {
            SetLights(false, false, false, true);
        }

        public void SetQualityLights()
        {
            SetLights(false, false, true, true);
        }

        public void SetPassLights()
        {
            SetLights(false, true, false, false);
        }

        private void SetLights(bool internalLight, bool green, bool yellow, bool red)
        {
            byte internalLightsByte = internalLight ? (byte)0xff : (byte)0;
            byte greenByte = green ? (byte)0xff : (byte)0;
            byte yellowByte = yellow ? (byte)0xff : (byte)0;
            byte redByte = red ? (byte)0xff : (byte)0;

            SendMessage(internalLightsByte, internalLightsByte, greenByte, yellowByte, redByte);
        }

        /// <summary>
        ///     bytes [0:1] - Internal tester lights
        ///     bytes [2] - Green
        ///     bytes [3] - Yellow
        ///     bytes [4] - Red 
        /// </summary>
        /// <param name="data"></param>
        private void SendMessage(params byte[] data)
        {
            var message = new byte[data.Length + 6];
            message[0] = 0x7E;
            message[1] = 0x06;
            message[2] = (byte)((data.Length + 1) & 0xFF);
            message[3] = (byte)((data.Length >> 8) & 0xFF);
            message[4] = 0x00;

            data.CopyTo(message, 5);
            message[data.Length + 5] = 0xE7;

            _serialPort.Write(message, 0, message.Length);
        }
    }

    class MockLightController : ILightController
    {
        public bool Init(Logger logger=null)
        {
            return true;
        }

        public void SetFailureLights() { }

        public void SetPassLights() { }

        public void SetQualityLights() { }

        public void SetStartupLights() { }

        public void TurnTesterLightOff(Logger logger)
        {
            logger.Info("Mocking - turning tester light off");
        }

        public void TurnTesterLightOn(Logger logger)
        {
            logger.Info("Mocking - turning tester light on");
        }
    }
}