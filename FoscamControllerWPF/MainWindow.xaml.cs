using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FoscamControllerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPCamCGI.IPCameraController controller = null;

        public MainWindow()
        {
            InitializeComponent();

            SourceInitialized += (s, e) =>
                {
                    controller = new IPCamCGI.IPCameraController
                        ("http://192.168.1.200/", "admin", "dave");
                    DataContext = controller;
                };
        }


        private void btnUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            controller.PTZ(IPCamCGI.IPCameraController.PTZAction.Up);
        }

        private void btnUp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            controller.PTZ(IPCamCGI.IPCameraController.PTZAction.Up_Stop);
        }

        private void btnLeft_MouseDown(object sender, MouseButtonEventArgs e)
        {
            controller.PTZ(IPCamCGI.IPCameraController.PTZAction.Left);
        }

        private void btnLeft_MouseUp(object sender, MouseButtonEventArgs e)
        {
            controller.PTZ(IPCamCGI.IPCameraController.PTZAction.Left_Stop);
        }

        private void btnStop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            controller.PTZ(IPCamCGI.IPCameraController.PTZAction.Center);
        }

        private void btnStop_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // 
        }

        private void btnRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            controller.PTZ(IPCamCGI.IPCameraController.PTZAction.Right);
        }

        private void btnRight_MouseUp(object sender, MouseButtonEventArgs e)
        {
            controller.PTZ(IPCamCGI.IPCameraController.PTZAction.Right_Stop);
        }

        private void btnDown_MouseDown(object sender, MouseButtonEventArgs e)
        {
            controller.PTZ(IPCamCGI.IPCameraController.PTZAction.Down);
        }

        private void btnDown_MouseUp(object sender, MouseButtonEventArgs e)
        {
            controller.PTZ(IPCamCGI.IPCameraController.PTZAction.Down_Stop);
        }

        private void btnHz50_Click(object sender, RoutedEventArgs e)
        {
            controller.ImageMode = IPCamCGI.IPCameraController.DisplayMode.HZ_50;
        }

        private void btnHz60_Click(object sender, RoutedEventArgs e)
        {
            controller.ImageMode = IPCamCGI.IPCameraController.DisplayMode.HZ_60;
        }

        private void btnHzOutdoor_Click(object sender, RoutedEventArgs e)
        {
            controller.ImageMode = IPCamCGI.IPCameraController.DisplayMode.Outdoor;
        }

        private void btnVGA_Click(object sender, RoutedEventArgs e)
        {
            controller.Resolution = IPCamCGI.IPCameraController.DisplayResolution.VGA;
        }

        private void btnQVGA_Click(object sender, RoutedEventArgs e)
        {
            controller.Resolution = IPCamCGI.IPCameraController.DisplayResolution.QVGA;
        }
    }
}
