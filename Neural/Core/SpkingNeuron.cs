using AIkailo.External.Common;
using AIkailo.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace AIkailo.Neural.Core
{
    /*
    public enum NeuronState
    {
        CHARGE,
        SPIKE,
        LEAK,
        ADAPT
    }*/

    /// <summary>
    /// A defined point in the network      
    /// </summary>
    public class SpikingNeuron : INeuron
    {
        public event Action<SpikingNeuron> Spike;

        public ulong Id { get; set; }
        public Property Label { get; set; }

        public List<SpikingSynapse> SynapsesOut { get; } = new List<SpikingSynapse>();
        public List<SpikingSynapse> SynapsesIn { get; } = new List<SpikingSynapse>();
        public SpikingNeuralSynchronizer Synchronizer { get; set; }

        public float Potential { get; set; } // Current Potential
        public float Threshold { get; set; } // Neuron spikes when [Absolute Potential > Threshold]
        public float Leak { get; set; } // Rate at which the Potential tends towards Rest
        public float Rest { get; set; } // Potential at Rest. TODO: Variable based on external cluster Potential


        private bool _doCharge;
        private bool _doSpike;
        private bool _doLeak;
        private bool _doAdapt;

        private float _incomingPotential = 0;
        private bool _isSpiking = false;

        public SpikingNeuron(ulong id, SpikingNeuralSynchronizer synchronizer)
            : this(null, id, synchronizer) { }

        public SpikingNeuron(string label, ulong id, SpikingNeuralSynchronizer synchronizer)
        {
            Label = label;
            Id = id;
            Synchronizer = synchronizer;
            //Synchronize();
        }

        private void Synchronize()
        {
            // Charge when any SynapseIn Potential != 0
            if (!_doCharge && SynapsesIn.Any(x => x.Potential != 0))
            {
                _doCharge = true;
                Synchronizer.Charge += OnCharge;
            }

            // Spike when Absolute Potential > Threshold
            if (!_doSpike && Math.Abs(Potential) > Math.Abs(Threshold))
            {
                _doSpike = true;
                Synchronizer.Spike += OnSpike;
            }

            // Leak when Potential != Rest
            if (!_doLeak && Potential != Rest)
            {
                _doLeak = true;
                Synchronizer.Leak += OnLeak;
            }

            // Adapt when Potential is between zero and rest
            if (!_doAdapt && (Rest < 0 && Potential > Rest && Potential <= 0) || (Rest > 0 && Potential < Rest && Potential >= 0))
            {
                _doAdapt = true;
                Synchronizer.Adapt += OnAdapt;
            }
        }

        // Neuron generates its own Potential (ie: input)                
        // TODO: Connect inputs to synapses instead of neurons?        
        public void Generate(float potential)
        {
            _incomingPotential += potential;

            if (!_isSpiking)
            {
                Potential += _incomingPotential;
                _incomingPotential = 0;
            }
        }

        internal void CreateSynapseTo(SpikingNeuron targetNeuron)
        {
            SpikingSynapse synapse = new SpikingSynapse();
            synapse.Source = this;
            synapse.Target = targetNeuron;
        }

        internal void CreateSynapseFrom(SpikingNeuron sourceNeuron)
        {
            SpikingSynapse synapse = new SpikingSynapse();
            synapse.Source = sourceNeuron;
            synapse.Target = this;
        }

        private void OnCharge()
        {
            // Don't charge while spiking
            if (_isSpiking) { return; }

            //Move charge from Synapses to Neuron
            Potential += SynapsesIn.Sum(s => s.Potential);
            SynapsesIn.ForEach(s => s.Potential = 0);

            // Done Charging
            _doCharge = false;
            Synchronizer.Charge -= OnCharge;
            Synchronize();
        }

        private void OnSpike()
        {
            _isSpiking = true;

            // Only spike as much as the Synapses can handle
            float allSynapsesMaxPotential = SynapsesOut.Sum(s => s.MaxPotential);
            float potentialToDistribute = Potential > allSynapsesMaxPotential ? allSynapsesMaxPotential : Potential;

            // Distribute the potential 
            foreach (SpikingSynapse synapse in SynapsesOut)
            {
                synapse.Potential = potentialToDistribute * (synapse.MaxPotential / allSynapsesMaxPotential);
            }

            Potential -= potentialToDistribute;

            _isSpiking = Potential != 0;

            if (!_isSpiking)
            {
                // Done spiking
                _doSpike = false;
                Synchronizer.Spike -= OnSpike;
                Synchronize();
            }

            Spike?.Invoke(this);
        }

        private void OnLeak()
        {
            // Move the charge towards Rest according to LeakRate

            if (Leak == 0) { return; }

            float newPotential = Potential + Leak;

            // Don't go past Rest
            if ((Leak > 0 && newPotential > Rest) || (Leak < 0 && newPotential < Rest))
            {
                Potential = Rest;
            }
            else
            {
                Potential = newPotential;
            }

            // Done Leaking
            _doLeak = false;
            Synchronizer.Leak -= OnLeak;
            Synchronize();
        }

        private void OnAdapt()
        {
            // TODO: STDP training and adaptation of neuron and synapse weights/thresholds

            _doAdapt = false;
            Synchronizer.Adapt -= OnAdapt;
            Synchronize();
        }

    }
}