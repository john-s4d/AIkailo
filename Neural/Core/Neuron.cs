using AIkailo.External.Common;
using AIkailo.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace AIkailo.Neural.Core
{
    /// <summary>
    /// A defined point in the network
    /// Can be grounded (with a label) or ungrounded (no label)       
    /// </summary>
    public class Neuron : INeuron
    {

        public string Id { get; set; }
        public Property Label { get; set; }
        public NeuronType NodeType { get; set; } = NeuronType.UNKNOWN;
        public List<Synapse> SynapsesOut { get; } = new List<Synapse>();
        public List<Synapse> SynapsesIn { get; } = new List<Synapse>();

        public float Threshold { get; set; } // Neuron fires when [CurrentCharge > Threshold]
        public float CurrentCharge { get; set; } // Current voltage
        public float LeakRate { get; set; } // Rate at which the voltage tends towards rest
        public float Rest { get; set; } // Voltage at rest. TODO: Variable based on external cluster charge

        // TODO: When Charge is between zero and rest, neuron is in 'feedback' mode and will accept rewards to modify the neuron threshold and leak, or synapse-out weights and speeds.

        public void Generate(float voltage)
        {
            // Neuron generates its own voltage (ie: input)    
            CurrentCharge += voltage;
        }

        private void Leak()
        {
            // Move the charge towards Rest according to LeakRate

            float leakedCharge = CurrentCharge + LeakRate;

            if ((LeakRate > 0 && leakedCharge > Rest) || (LeakRate < 0 && leakedCharge < Rest))
            {
                CurrentCharge = Rest;
            }
            else
            {
                CurrentCharge = leakedCharge;
            }
        }

        public void Charge()
        {
            //Move charge from Synapses to Neuron
            foreach (Synapse synapse in SynapsesIn)
            {
                if (synapse.CurrentCharge != 0)
                {
                    CurrentCharge += synapse.CurrentCharge;
                    synapse.CurrentCharge = 0;
                }
            }
        }

        private void Spike()
        {
            // Distribute charge from Neuron to Synapses
            if (Math.Abs(CurrentCharge) < Math.Abs(Threshold))
            {
                return;
            }

            float totalSynapseCharge = SynapsesOut.Sum(s => s.MaxCharge);

            float chargeToDistribute = CurrentCharge > totalSynapseCharge ? totalSynapseCharge : CurrentCharge;

            foreach (Synapse synapse in SynapsesOut)
            {
                synapse.CurrentCharge = chargeToDistribute * (synapse.MaxCharge / totalSynapseCharge);
            }

            CurrentCharge -= chargeToDistribute;
        }

        private Task OnTick()
        {   
            Charge();
            Spike();
            Leak();
            return Task.CompletedTask;
        }

        public async void Tick()
        {
            await OnTick();
        }
    }

    public enum NeuronType
    {
        UNKNOWN,
        INPUT,
        HIDDEN,
        OUTPUT,
        MODEL,     // Defines an embedded neural model        
        HINT       // Nodes at edges are potentially related
    }
}