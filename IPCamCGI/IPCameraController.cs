using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;

namespace IPCamCGI
{
    public class IPCameraController : INotifyPropertyChanged
    {
        public enum PTZAction
        {
            Up = 0,
            Up_Stop = 1,
            Down = 2,
            Down_Stop = 3,
            Left = 4,
            Left_Stop = 5,
            Right = 6,
            Right_Stop = 7,
            Center = 25,
            Patrol_Vertical = 26,
            Patrol_Vertical_Stop = 27,
            Patrol_Horizon = 28,
            Patrol_Horizon_Stop = 29,
            IO_High = 94,
            IO_Low = 95,
        }

        public enum DisplayResolution
        {
            QVGA = 8,
            VGA = 32,
        }

        public enum DisplayMode
        {
            HZ_50 = 0,
            HZ_60 = 1,
            Outdoor = 2,
        }

        public enum DisplayAlteration
        {
            Default = 0,
            Flip = 1,
            Mirror = 2,
            FlipAndMirror = 3,
        }

        string UserName = "";
        string Password = "";
        string BaseURL = "";

        bool fNoCommit = false;

        public IPCameraController(string BaseURL, string UserName, string Password)
        {
            this.BaseURL = BaseURL;
            this.UserName = UserName;
            this.Password = Password;

            LoadVariables();
        }

        void LoadVariables()
        {
            string vars = Req("get_camera_params.cgi?");
            fNoCommit = true;
            foreach(string s in vars.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries))
            {
                Match m = Regex.Match(s, ".* (.*)=(.*);");
                if (m.Success)
                {
                    try
                    {
                        string key = m.Groups[1].Value;
                        int value = int.Parse(m.Groups[2].Value);
                        switch (key)
                        {
                            case "resolution":
                                Resolution = (DisplayResolution)value;
                                break;
                            case "brightness":
                                Brightness = value;
                                break;
                            case "contrast":
                                Contrast = value;
                                break;
                            case "mode":
                                ImageMode = (DisplayMode)value;
                                break;
                            case "flip":
                                ImageAlteration = (DisplayAlteration)value;
                                break;
                            default:
                                Trace.WriteLine("Invalid variable: " + s);
                                break;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Trace.WriteLine("Variable not integer: " + s);
                    }
                }
                else
                {
                    Trace.WriteLine("Unrecognized variable: " + s);
                }
            }
            fNoCommit = false;
        }

        string Req(string req)
        {
            if (fNoCommit) return "";
            string ret = new WebClient().DownloadString(BaseURL + req
                + "&user=" + UserName + "&pwd=" + Password);
            Trace.WriteLine(req + " => " + ret);
            return ret;
        }

        void DecoderControl(int cmd)
        {
            new Thread(() => Req("decoder_control.cgi?command=" + cmd)).Start(); // + "&onestep=20";
        }

        void CameraControl(int param, int value)
        {
            new Thread(() => Req("camera_control.cgi?param=" + param + "&value=" + value)).Start();
        }

        int _Brightness = 0;
        public int Brightness
        {
            get
            {
                return _Brightness;
            }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Brightness"));
                _Brightness = value;
                CameraControl(1, _Brightness);
            }
        }

        int _Contrast = 0;
        public int Contrast
        {
            get
            {
                return _Contrast;
            }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Contrast"));
                _Contrast = value;
                CameraControl(2, _Contrast);
            }
        }

        DisplayResolution _Resolution = DisplayResolution.QVGA;
        public DisplayResolution Resolution
        {
            get
            {
                return _Resolution;
            }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Resolution"));
                _Resolution = value;
                CameraControl(0, (int)_Resolution);
            }
        }

        DisplayMode _ImageMode = DisplayMode.HZ_50;
        public DisplayMode ImageMode
        {
            get
            {
                return _ImageMode;
            }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ImageMode"));
                _ImageMode = ImageMode;
                CameraControl(1, (int)_ImageMode);
            }
        }

        DisplayAlteration _ImageAlteration = DisplayAlteration.Default;
        private DisplayAlteration ImageAlteration
        {
            get
            {
                return _ImageAlteration;
            }
            set
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ImageAlteration"));
                _ImageAlteration = ImageAlteration;
                CameraControl(5, (int)_ImageAlteration);
            }
        }

        public void PTZ(PTZAction action)
        {
            DecoderControl((int)action);
        }

        public void Reboot()
        {
            Req("reboot.cgi");
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
