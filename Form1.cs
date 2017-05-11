using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
//using System.Drawing;
using System.Drawing.Imaging;
using AForge.Video;
using AForge.Video.VFW;
using AForge.Controls;
using AForge.Video.DirectShow;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Cvb;
using Emgu.CV.UI;

namespace WebCam
{
    public partial class Form1 : Form
    {

        //AVIReader reader;
         // list of video devices
        FilterInfoCollection videoDevices;
        // stop watch for measuring fps
        private Stopwatch stopWatch = null;
        private CascadeClassifier faceCascade;
        public Form1( )
        {
            //reader = new AVIReader();
            //reader.Open("123.avi");
            //Capture 
            InitializeComponent( );
             faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
             

            camera1FpsLabel.Text = string.Empty;
            videoSourcePlayer1.NewFrame += new VideoSourcePlayer.NewFrameHandler(this.playerControl_NewFrame);
         

            // show device list
			try
			{
                // enumerate video devices
                videoDevices = new FilterInfoCollection( FilterCategory.VideoInputDevice );

                if ( videoDevices.Count == 0 )
                {
                    throw new Exception( );
                }

                for ( int i = 1, n = videoDevices.Count; i <= n; i++ )
                {
                    string cameraName = i + " : " + videoDevices[i - 1].Name;

                    camera1Combo.Items.Add( cameraName );
                    
                }

                // check cameras count
               
                camera1Combo.SelectedIndex = 0;
            }
            catch
            {
                startButton.Enabled = false;

                camera1Combo.Items.Add( "No cameras found" );
               

                camera1Combo.SelectedIndex = 0;
               

                camera1Combo.Enabled = false;
              
            }
        }

        // On form closing
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCameras( );
        }

        // On "Start" button click
        private void startButton_Click( object sender, EventArgs e )
        {
            StartCameras( );

            startButton.Enabled = false;
            stopButton.Enabled = true;
        }

        // On "Stop" button click
        private void stopButton_Click( object sender, EventArgs e )
        {
           // if (bitmap != null)
              //  bitmap.Save("123.png", ImageFormat.Png);
            StopCameras( );
            

            startButton.Enabled = true;
            stopButton.Enabled = false;

            camera1FpsLabel.Text = string.Empty;
           
        }

        // Start cameras
        private void StartCameras( )
        {
            // create first video source
            VideoCaptureDevice videoSource1 = new VideoCaptureDevice(videoDevices[camera1Combo.SelectedIndex].MonikerString );
            
           
            //videoSource1.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSourcePlayer1.VideoSource = videoSource1;
           
            videoSourcePlayer1.Start( );

            // create second video source
           

            // reset stop watch
            stopWatch = null;
            // start timer
            timer.Start( );
        }

        // Stop cameras
        private void StopCameras( )
        {
            timer.Stop( );

            videoSourcePlayer1.SignalToStop( );
            

            videoSourcePlayer1.WaitForStop( );
           
        }

        Bitmap bitmap=null;
      
        // new frame event handler
        private void playerControl_NewFrame(object sender, ref Bitmap image)
        {
           // image = reader.GetNextFrame();
            Image<Bgr, Byte> tempImage = new Image<Bgr, Byte>(image);
            var Face = faceCascade.DetectMultiScale(tempImage);
            foreach (var face in Face)
            {

                Graphics.FromImage(image).DrawRectangle(new Pen(Brushes.Black, 5), face);

            }
           
           
        }
       // private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
       // {
            
           // bitmap =new Bitmap( eventArgs.Frame);
           
      //  }
        // On times tick - collect statistics
        private void timer_Tick( object sender, EventArgs e )
        {
            IVideoSource videoSource1 = videoSourcePlayer1.VideoSource;

            
            int framesReceived1 = 0;
            int framesReceived2 = 0;

            // get number of frames for the last second
            if ( videoSource1 != null )
            {
                framesReceived1 = videoSource1.FramesReceived;
              
            
               
            }

           

            if ( stopWatch == null )
            {
                stopWatch = new Stopwatch( );
                stopWatch.Start( );
            }
            else
            {
                stopWatch.Stop( );

                float fps1 = 1000.0f * framesReceived1 / stopWatch.ElapsedMilliseconds;
                float fps2 = 1000.0f * framesReceived2 / stopWatch.ElapsedMilliseconds;

                camera1FpsLabel.Text = fps1.ToString( "F2" ) + " fps";
               

                stopWatch.Reset( );
                stopWatch.Start( );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //flag = true;
            if (bitmap != null )
                bitmap.Save("123.jpg", ImageFormat.Png);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
       // public Form1()
      //  {
           // InitializeComponent();
       // }
    }
}
