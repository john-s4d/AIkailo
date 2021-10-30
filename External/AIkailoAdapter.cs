using System;
using AIkailo.Messaging;
using AIkailo.External.Common;
using AIkailo.Messaging.Messages;
using System.Collections.Generic;

namespace AIkailo.External
{
    public class AIkailoAdapter
    {
        private MessageService _messageService;

        public delegate void Input(FeatureArray data);

        private AIkailoAdapter() { }

        public AIkailoAdapter(string host)
        {
            _messageService = new MessageService(host);
        }

        public void Start()
        {
            _messageService.Start();
        }

        public void Stop()
        {
            _messageService.Stop();
        }

        public void RegisterInput(string name, out AIkailoInputInterface input)
        {
            input = new AIkailoInputInterface(name);
            input.InputEvent += OnInputEvent;     
         
        }

        public void RegisterOutput(string name, Action<Dictionary<ulong,float>> callback)
        {
            AIkailoOutput result = new AIkailoOutput(name);
            _messageService.RegisterConsumer(result.Name, result.Consumer);
            result.OutputEvent += callback;
        }

        //private void OnInputEvent(string source, FeatureArray data)
        private void OnInputEvent(string source, Dictionary<ulong,float> data)
        {
            _messageService.Publish(new InputMessage(source, data));
        }

        /*
         public void Train(TrainingStep flow)
        {
            InputMessage input = new InputMessage(flow.Source, flow.Input);
            ExternalMessage output = new ExternalMessage(flow.Target, flow.Output);

            _messageService.Publish(new TrainingMessage(input, output));
        }
        */

        public void Train(TrainingData data)
        {
            InputMessage input = new InputMessage(data.Source, data.Input);
            ExternalMessage output = new ExternalMessage(data.Target, data.Output);

            _messageService.Publish(new TrainingMessage(input, output));
        }
    }
}