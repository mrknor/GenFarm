namespace GenFarm
{
    public class BlogPost
    {
        public string Title { get; set; }
        public List<SubHeader> SubHeaders { get; set; }
        public BlogMetadata Metadata { get; set; }
        public string Content { get; set; } // Optional: Full content
        public DateTime CreatedAt { get; set; }
    }

    public class SubHeader
    {
        public string HeaderText { get; set; }
        public string BodyText { get; set; }
    }

    public class BlogMetadata
    {
        public string Author { get; set; }
        public string SEOKeywords { get; set; }
        public DateTime PublishedDate { get; set; }
    }

    public class AgentTask
    {
        public string TaskType { get; set; }
        public string Payload { get; set; }
        public AgentTaskStatus Status { get; set; }
        public string Result { get; set; } // Optional: Result of the task
    }

    public enum AgentTaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed
    }

    public class AgentMessage
    {
        public string FromAgent { get; set; }
        public string ToAgent { get; set; }
        public string Content { get; set; } // Can be serialized data
    }

    public class ErrorLog
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    public class ResponseMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; } // Optional: Additional data
    }

    public class SEOKeywords
    {
        public List<string> Keywords { get; set; }
    }

    public class DeploymentInfo
    {
        public string Platform { get; set; }
        public DateTime DeploymentTime { get; set; }
        public bool IsSuccessful { get; set; }
    }


}