using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YeelightAPI;

namespace LimeLight
{
    public class LampHandler
    {
        // Yeelight to be updated
        private Device Yeelight;

        // Timer loop for updating the lamp color
        private System.Windows.Forms.Timer lampUpdateTimer;

        // Function to connect to lamp and start update loop
        public void ConnectLampAndStartUpdateLoop(string ip)
        {
            // Connect to specified light
            Yeelight = new Device(ip);
            Yeelight.Connect();

            // Set up timer to update lights every 1 second
            lampUpdateTimer = new System.Windows.Forms.Timer();
            lampUpdateTimer.Tick += new EventHandler(UpdateLamp);
            lampUpdateTimer.Interval = 1000;
            lampUpdateTimer.Start();
        }

        // Function to disconnect from light and stop timer
        public void DisconnectLampAndStopUpdateLoop()
        {
            lampUpdateTimer.Stop();

            if (Yeelight != null && Yeelight.IsConnected) 
            {
                Yeelight.Disconnect();
            }
        }

        // Function to update light colors
        public void UpdateLamp(Object myObject, EventArgs myEventArg)
        {
            if (Yeelight != null && Yeelight.IsConnected)
            {
                //Bitmap for screen capture
                using (Bitmap captureBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                               Screen.PrimaryScreen.Bounds.Height,
                               PixelFormat.Format32bppArgb))
                {
                    // Get screen bounds
                    Rectangle captureRectangle = Screen.PrimaryScreen.Bounds;

                    // Get graphics for bitmap
                    using (Graphics captureGraphics = Graphics.FromImage(captureBitmap))
                    {
                        // Get screen capture
                        captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

                        // Reduce bitmap to 1 pixel to get average color
                        using (Bitmap avg = new Bitmap(1, 1))
                        {
                            using (Graphics avgGfx = Graphics.FromImage(avg))
                            {
                                avgGfx.DrawImage(captureBitmap, 0, 0, avg.Width, avg.Height);
                                Color color = avg.GetPixel(0, 0);
                                
                                //avg.Save(@"D/img.jpg", ImageFormat.Jpeg);

                                // Update lamp colors
                                Yeelight.SetRGBColor(color.R, color.G, color.B, 750);
                            }
                        }
                    }
                }
            }

            else
            {
                Yeelight.Connect();
            }
        }
    }
}