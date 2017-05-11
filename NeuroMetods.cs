using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WebCam
{
    class NeuroMetods
    {
        
        
 
        private double[] ImageToVector(Bitmap img)
        {
            int Size = img.Size.Height*img.Size.Width;
            double[] vector = new double[Size];
            int i = 0;
            for (int x = 0; x < img.Size.Width; x++)
            {
                for (int y = 0; y < img.Size.Height; y++)
                { 
                    Color pixel = img.GetPixel(x,y);
                    Byte lum = (Byte)((pixel.R * 77 + pixel.G * 151 + pixel.B * 28) >> 8);
                    vector[i++] = 1.0f - lum / 255.0f;
                }
            }
            return vector;
        }
 
        private int Test(NeuroNet net, double[] InputVector)
        {
            double MinDistance = EuclideanDistance(net.neurons[0], InputVector);
            int BMUIndex = 0;
            for (int i = 1; i < net.neurons.Count; i++)
            {
                double tmp_ED = EuclideanDistance(net.neurons[i], InputVector);
                if (tmp_ED < MinDistance)
                {
                    BMUIndex = i;
                    MinDistance = tmp_ED;
                }
            }
            return BMUIndex;
        }
 
        private void Study(ref NeuroNet net, double[][] InputVector)
        {
            int c;
            for (int k = 0; k < 6; k++) // цикл, в котором предъявляем сети входные вектора - InputVector
            {
                double MinDistance = EuclideanDistance(net.neurons[0], InputVector[k]);
                int BMUIndex = 0;
                for (int i = 1; i < net.neurons.Count; i++)
                {
                    double tmp_ED = EuclideanDistance(net.neurons[i], InputVector[k]); //находим Евклидово расстояние между i-ым нейроном и k-ым входным  вектором
                    if (tmp_ED < MinDistance) // если Евклидово расстояние минимально, то это нейрон-победитель
                    {
                        BMUIndex = i; // индекс нейрона-победителя
                        MinDistance = tmp_ED; 
                    }
                }
 
                for (int i = 0; i < net.neurons.Count; i++)
                {
                    for (int g = 0; g < InputVector[k].Length; g++)
                    {
                        double hfunc = hc(k, net.neurons[BMUIndex].weights[g], net.neurons[i].weights[g]);
                        double normfunc = normLearningRate(k);
                        net.neurons[i].weights[g] = net.neurons[i].weights[g] + hfunc * normfunc * (InputVector[k][g] - net.neurons[i].weights[g]);
                        if (i > 0 && g > 282)
                            c = 0;
                    }
                }
                double Error = EuclideanDistance(net.neurons[BMUIndex], InputVector[k]);
            
            }
        }
 
        private double hc(int k, double winnerCoordinate, double Coordinate)
        {
            double dist = Distance(winnerCoordinate, Coordinate);
            double s = sigma(k);
            return Math.Exp(-dist * dist / (2 * Sqr(sigma(k))));
        }
 
        private double sigma(int k)
        {
            //return -0.01 * k + 2;
            return 1 * Math.Exp(-k / 5);
 
            //double nf = 1000 / Math.Log(2025);
            //return Math.Exp(-k / nf) * 2025;
        }
 
        private double normLearningRate(int k)
        {
            return 0.1 * Math.Exp(-k / 1000);
        }
 
        private double EuclideanDistance(Neuron neuron, double[] InputVector)
        {
            double Sum = 0;
            for (int i = 0; i < InputVector.Length; i++)
            {
                Sum += Sqr(InputVector[i] - neuron.weights[i]);
            }
            return Math.Sqrt(Sum);
        }
 
        private double Distance(double winnerCoordinate, double Coordinate)
        {
            return Math.Sqrt(Sqr(winnerCoordinate - Coordinate));
        }
 
        private double Sqr(double value)
        {
            return value * value;
        }
    }
 
   
 
 
    
    }


