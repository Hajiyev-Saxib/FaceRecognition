using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Cvb;
using Emgu.CV.UI;
namespace WebCam
{
    class FaceRecognition
    {
         public CascadeClassifier faceCascade;
         LBPHFaceRecognizer recognizer;
        public FaceRecognition()
         {
             faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
             recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 130);
         }
        public void FaceLearn()
        {
            recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 80);
            Image<Gray, Byte>[] images = new Image<Gray, Byte>[30];
            Image<Gray, Byte> image;
           // image = getFace("123.jpg");
            int[] labels = new int[30];
            open_image(ref images, ref labels);
            for (int i = 25; i < 30; i++)
            {
                images[i] = getFace((i - 24) + ".jpg");
                labels[i] = 6;
            }
            recognizer.Train<Gray, Byte>(images, labels);
            Console.WriteLine("Training iss over");

            FaceRecognizer.PredictionResult r;
            Console.WriteLine("!");
            //for (int i = 0; i < 5; i++)
            //{
            //    r = recognizer.Predict(getFace("yalefaces/subject1" + (i + 1) + ".centerlight"));
            //    Console.WriteLine("labels:" + r.Label + " and distance:" + r.Distance);
            //}
            //r = recognizer.Predict(image);
            //Console.WriteLine("labels:" + r.Label + " and distance:" + r.Distance);

           // Console.ReadKey();

           
        }

        public FaceRecognizer.PredictionResult Recognition(Image<Gray, Byte> image)
         {
             FaceRecognizer.PredictionResult r;
             r = recognizer.Predict(image);
             //Console.WriteLine("labels:" + r.Label + " and distance:" + r.Distance);
             return r;
         }
        public void open_image(ref Image<Gray, Byte>[] image, ref int[] labels)
        {
            String path = "yalefaces/subject";
            //subject11.leftlight surprised noglasses sad glasses
            for (int j = 1; j <= 5; j++)
            {
                int i = j - 1;
                image[i * 5] = getFace(path + "1" + j + ".leftlight");
                // image[i*5]=iamge[i]
                labels[i * 5] = j;
                image[i * 5 + 1] = getFace(path + "1" + j + ".surprised");
                labels[i * 5 + 1] = j;
                image[i * 5 + 2] = getFace(path + "1" + j + ".noglasses");
                labels[i * 5 + 2] = j;
                image[i * 5 + 3] = getFace(path + "1" + j + ".sad");
                labels[i * 5 + 3] = j;
                image[i * 5 + 4] = getFace(path + "1" + j + ".glasses");
                labels[i * 5 + 4] = j;
                Console.WriteLine("all file readen");

            }



        }
        public Image<Gray, Byte> getFace(String s)
        {
            Image<Gray, Byte> image = new Image<Gray, byte>(s);
            Rectangle[] k = faceCascade.DetectMultiScale(image);
            if (k.Length > 0)
                image = image.Copy(k[0]);
            return image;

        }

        
    }
}
