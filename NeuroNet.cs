using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCam
{
    class NeuroNet
    {
        public int inputs = 0;
        public List<Neuron> neurons;

        public NeuroNet(int inputs_, int neurons_)
        {
            neurons = new List<Neuron>();
            inputs = inputs_;
            for (int i = 0; i < neurons_; i++)
            {
                Neuron neuron = new Neuron(i, inputs_);
                neurons.Add(neuron);
            }
        }
    }
}
