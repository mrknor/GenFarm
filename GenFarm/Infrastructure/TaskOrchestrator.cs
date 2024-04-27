using System;
using System.Threading.Tasks;
using GenFarm.Services;
using System.Text.Json;

namespace GenFarm.Infrastructure
{
    public class TaskOrchestrator
    {
        private readonly MessageQueue _messageQueue;
        private readonly SEOKeywordAgent _seoKeywordAgent;
        private readonly HeaderGenerationAgent _headerGenerationAgent;
        private readonly BodyGenerationAgent _bodyGenerationAgent;
        private readonly EditorAgent _editorAgent;
        private readonly SEOOptimizationAgent _seoOptimizationAgent;
        private readonly DeploymentAgent _deploymentAgent;

        public TaskOrchestrator(
            MessageQueue messageQueue,
            SEOKeywordAgent seoKeywordAgent,
            HeaderGenerationAgent headerGenerationAgent,
            BodyGenerationAgent bodyGenerationAgent,
            EditorAgent editorAgent,
            SEOOptimizationAgent seoOptimizationAgent,
            DeploymentAgent deploymentAgent)
        {
            _messageQueue = messageQueue;
            _seoKeywordAgent = seoKeywordAgent;
            _headerGenerationAgent = headerGenerationAgent;
            _bodyGenerationAgent = bodyGenerationAgent;
            _editorAgent = editorAgent;
            _seoOptimizationAgent = seoOptimizationAgent;
            _deploymentAgent = deploymentAgent;
        }

        public void StartOrchestration()
        {
            _messageQueue.StartListening(async message => {
                var task = DeserializeMessage(message);

                switch (task.TaskType)
                {
                    case "SEOKeyword":
                        await HandleSEOKeywordTask(task.Payload);
                        break;
                    case "GenerateSubHeaders":
                        await HandleHeaderGenerationTask(task.Payload);
                        break;
                    case "GenerateBodyContent":
                        await HandleBodyGenerationTask(task.Payload);
                        break;
                    case "EditBlogPost":
                        await HandleEditorTask(task.Payload);
                        break;
                    case "OptimizeForSEO":
                        await HandleSEOOptimizationTask(task.Payload);
                        break;
                    case "DeployContent":
                        await HandleDeploymentTask(task.Payload);
                        break;
                    default:
                        throw new ArgumentException($"Unknown task type: {task.TaskType}");
                }
            });
        }

        private AgentTask DeserializeMessage(string message)
        {
            return JsonSerializer.Deserialize<AgentTask>(message);
        }

        private string SerializeTask(AgentTask task)
        {
            return JsonSerializer.Serialize(task);
        }

        private async Task HandleSEOKeywordTask(string payload)
        {
            var seoKeyword = _seoKeywordAgent.ProcessSEOKeyword(payload);
            var newTask = new AgentTask
            {
                TaskType = "GenerateSubHeaders",
                Payload = seoKeyword
            };
            _messageQueue.SendMessage(SerializeTask(newTask));
        }

        private async Task HandleHeaderGenerationTask(string payload)
        {
            var subHeaders = await _headerGenerationAgent.GenerateSubHeaders(payload);

            foreach (var subHeader in subHeaders)
            {
                var newTask = new AgentTask
                {
                    TaskType = "GenerateBodyContent",
                    Payload = subHeader
                };
                _messageQueue.SendMessage(SerializeTask(newTask));
            }
        }

        private async Task HandleBodyGenerationTask(string payload)
        {
            var bodyContent = await _bodyGenerationAgent.GenerateBodyContent(payload);

            var newTask = new AgentTask
            {
                TaskType = "EditBlogPost",
                Payload = bodyContent
            };
            _messageQueue.SendMessage(SerializeTask(newTask));
        }

        private async Task HandleEditorTask(string payload)
        {
            // Deserialize the payload to BlogPost object
            var blogPost = JsonSerializer.Deserialize<BlogPost>(payload);

            var editedBlogPost = _editorAgent.EditBlogPost(blogPost);

            var newTask = new AgentTask
            {
                TaskType = "OptimizeForSEO",
                Payload = SerializeBlogPost(editedBlogPost) // Serialize BlogPost object
            };
            _messageQueue.SendMessage(SerializeTask(newTask));
        }

        private async Task HandleSEOOptimizationTask(string payload)
        {
            // Deserialize the payload to BlogPost object
            var blogPost = JsonSerializer.Deserialize<BlogPost>(payload);

            var optimizedBlogPost = _seoOptimizationAgent.OptimizeBlogPost(blogPost, blogPost.Metadata.SEOKeywords);

            var newTask = new AgentTask
            {
                TaskType = "DeployContent",
                Payload = SerializeBlogPost(optimizedBlogPost) // Serialize BlogPost object
            };
            _messageQueue.SendMessage(SerializeTask(newTask));
        }

        private async Task HandleDeploymentTask(string payload)
        {
            // Deserialize the payload to BlogPost object
            var blogPost = JsonSerializer.Deserialize<BlogPost>(payload);

            var success = await _deploymentAgent.DeployToShopify(blogPost);

            if (!success)
            {
                throw new Exception("Deployment failed");
            }
        }

        private string SerializeBlogPost(BlogPost blogPost)
        {
            // Serialize BlogPost object to string
            return JsonSerializer.Serialize(blogPost);
        }
    }
}
