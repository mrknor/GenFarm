using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenFarm.Infrastructure
{
    public class MessageQueue
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly List<AgentTask> _taskStatus = new List<AgentTask>(); // Track task status

        public MessageQueue()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost", // Ensure this is correct
                Port = 5672, // Default port for AMQP is 5672
                UserName = "guest", // Default RabbitMQ user
                Password = "guest", // Default RabbitMQ password
            };


            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "GenFarmQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: "GenFarmQueue", basicProperties: null, body: body);

            // Add task status to tracking
            var task = System.Text.Json.JsonSerializer.Deserialize<AgentTask>(message);
            _taskStatus.Add(task); // Store initial status
        }

        public void StartListening(Func<string, Task> messageHandler)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) => {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Update task status to "InProgress"
                var task = System.Text.Json.JsonSerializer.Deserialize<AgentTask>(message);
                var existingTask = _taskStatus.Find(t => t.TaskType == task.TaskType && t.Payload == task.Payload);

                if (existingTask != null)
                {
                    existingTask.Status = AgentTaskStatus.InProgress;
                }

                await messageHandler(message);

                // After processing, update status to "Completed"
                if (existingTask != null)
                {
                    existingTask.Status = AgentTaskStatus.Completed;
                }
            };

            _channel.BasicConsume(queue: "GenFarmQueue", autoAck: true, consumer: consumer);
        }

        public List<AgentTask> GetTaskStatus()
        {
            // Return a copy of the task status list
            return new List<AgentTask>(_taskStatus);
        }

        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }

        public void PurgeQueue()
        {
            _channel.QueuePurge("GenFarmQueue");
        }
    }
}
