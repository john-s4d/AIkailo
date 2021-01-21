﻿using System;
using AIkailo.Messaging;
using AIkailo.External.Model;
using AIkailo.Messaging.Messages;

namespace AIkailo.External
{
    public class AIkailoAdapter
    {
        private MessageService _messageService;

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

        public AIkailoInput RegisterInput(string name)
        {
            AIkailoInput result = new AIkailoInput(name);
            result.InputEvent += OnInputEvent;
            return result;
        }

        public void RegisterOutput(string name, Action<FeatureVector> callback)
        {
            AIkailoOutput result = new AIkailoOutput(name);
            _messageService.RegisterConsumer(result.Name, result.Consumer);
            result.OutputEvent += callback;
        }

        private void OnInputEvent(string source, FeatureVector data)
        {
            _messageService.Publish(new InputMessage(source, data));
        }
    }
}