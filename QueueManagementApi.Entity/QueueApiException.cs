using System.Runtime.Serialization;

namespace QueueManagementApi.Core;

public class QueueApiException : Exception
{
    public QueueApiException() {}
    
    public QueueApiException(string message) : base(message) { }
    
    public QueueApiException(string messageFormat, params object[] args) : base(string.Format(messageFormat, args)) { }
    
    public QueueApiException(string message, Exception innerException) : base(message, innerException) { }
}