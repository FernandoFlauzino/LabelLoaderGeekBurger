using AutoMapper;
using GeekBurger.Ingredients.Contract.Response;
using GeekBurger.LabelLoader.Web.Application.Interface;
using GeekBurger.LabelLoader.Web.Extension;
using GeekBurger.LabelLoader.Web.Models;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.LabelLoader.Web.Application.Service
{
    public class LabelLoaderChangedService : ILababelLoaderChangedService
    {
        private const string Topic = "LabelLoaderChanged";
        private readonly IConfiguration _configuration;
        private IMapper _mapper;
        private readonly List<Message> _messages;
        private Task _lastTask;
        private readonly IServiceBusNamespace _namespace;
        private readonly ILogService _logService;
        private CancellationTokenSource _cancelMessages;
        private IServiceProvider _serviceProvider { get; }

        public LabelLoaderChangedService(IMapper mapper,
            IConfiguration configuration, ILogService logService, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _configuration = configuration;
            _logService = logService;
            _messages = new List<Message>();
            _namespace = _configuration.GetServiceBusNamespace();
            _cancelMessages = new CancellationTokenSource();
            _serviceProvider = serviceProvider;
        }

        public void EnsureTopicIsCreated()
        {
            if (!_namespace.Topics.List()
                .Any(topic => topic.Name
                    .Equals(Topic, StringComparison.InvariantCultureIgnoreCase)))
                _namespace.Topics.Define(Topic)
                    .WithSizeInMB(1024).Create();
        }

        public async void SendMessagesAsync()
        {
            var config = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var topicClient = new TopicClient(config.ConnectionString, Topic);
            EnsureTopicIsCreated();
            _logService.SendMessagesAsync("LabelLoader was changed");

            _lastTask = SendAsync(topicClient);

            await _lastTask;

            var closeTask = topicClient.CloseAsync();
            await closeTask;
            HandleException(closeTask);
        }

        public void AddToMessageList(IngredientsToUpsert ingredient)
        {
            _messages.Add(GetMessage(ingredient));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancelMessages.Cancel();

            return Task.CompletedTask;
        }
        public async Task SendAsync(TopicClient topicClient)
        {
            var sendTask = topicClient.SendAsync(_messages.FirstOrDefault());
            await sendTask;
            var success = HandleException(sendTask);
        }
        public bool HandleException(Task task)
        {
            if (task.Exception == null || task.Exception.InnerExceptions.Count == 0) return true;

            task.Exception.InnerExceptions.ToList().ForEach(innerException =>
            {
                Console.WriteLine($"Error in SendAsync task: {innerException.Message}. Details:{innerException.StackTrace} ");

                if (innerException is ServiceBusCommunicationException)
                    Console.WriteLine("Connection Problem with Host. Internet Connection can be down");
            });

            return false;
        }

        public Message GetMessage(IngredientsToUpsert entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var jsonByte = Encoding.UTF8.GetBytes(json);

            return new Message
            {
                Body = jsonByte,
                MessageId = entity.ProductId.ToString()
            };
        }

    }
}
