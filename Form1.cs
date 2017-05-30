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
using Emgu.CV.Face;

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
        FaceRecognition faceRecog;
        String[] mas= {"Unknow","Чел 1 ","Чел 2 ","Чел 3 ","Чел 4 ","Чел 5 "," Gadjiev Saxib 21 year old, " };
        public Form1( )
        {

            //reader = new AVIReader();
            //reader.Open("123.avi");
            //Capture 
            faceRecog = new FaceRecognition();
            InitializeComponent( );
             faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
        //  button2.Enabled = false;
        //   button3.Enabled = false;
            // buttonLearn.Enabled = false;
             LBPHFaceRecognizer recognizer = new LBPHFaceRecognizer(1,8,8,8,123);
             

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
             //camera1Combo.Items.Add("2 : WebCam GG-321");
             //camera1Combo.Items.Add("3 : WebCam SC-01DCL32219N");
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

        // On "Stop" button clickpla
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
            Image<Gray, Byte> tempImage = new Image<Gray, Byte>(image);
         //   Mat sharp = new Mat(m);
            var Face = faceCascade.DetectMultiScale(tempImage, 1.1, 6, new Size(90, 90));
            FaceRecognizer.PredictionResult k;
            int f;
            Font drawFont = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            foreach (var face in Face)
            { 
                Graphics.FromImage(image).DrawRectangle(new Pen(Brushes.Black, 5), face);
                if (flag == true)
                {
                    k = faceRecog.Recognition(tempImage.Copy(face));
                    if (k.Distance > 75)
                    {
                        f = 0;
                        drawBrush = new SolidBrush(Color.Red);
                    }
                    else
                        f = k.Label;
                    PointF drawPoint = new PointF(face.X, face.Y-30);
                Graphics.FromImage(image).DrawString(mas[f]+k.Distance, drawFont, drawBrush,drawPoint);
                }
               
               

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

       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Connect(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

       

        private void label7_Click(object sender, EventArgs e)
        {

        }
        bool flag = false;
        private void CLickForLearn(object sender, EventArgs e)
        {
            WaitInfo.Text = "" + "wait/ system learning";

            faceRecog.FaceLearn();
            flag = true;
            WaitInfo.Text = "" + "system is ready";
        }
       // public Form1()
      //  {
           // InitializeComponent();
       // }
    }
}
