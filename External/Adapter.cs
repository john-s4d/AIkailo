using System;
using System.Collections.Generic;
using AIkailo.Messaging;
using AIkailo.Model;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.External
{
    public class Adapter
    {
        private MessageService _messageService;

        public Adapter(string host)
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

        public void AddInput(Input input)
        {
            input.InputEvent += OnInputEvent;
        }

        private void OnInputEvent(string sender, List<Tuple<IConvertible, IConvertible>> data)
        {
            _messageService.Publish(new InputMessage(sender, data));
        }

        public Output AddOutput(Output output)
        {
            _messageService.RegisterConsumer(output.Name, output.Consumer);
            return output;
        }
    }
}