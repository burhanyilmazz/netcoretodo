using Todo.Core.Helpers;
using Newtonsoft.Json;
using System.Net;
using Todo.Core.Enums;

namespace Todo.Core.Entities
{
    [JsonConverter(typeof(ApiErrorConverter))]
    public class ServiceResult
    {
        [JsonProperty("MessageType")]
        public EMessageType MessageType { get; set; }

        [JsonProperty("MessageTypeText")]
        public string MessageTypeText { get; set; }

        [JsonProperty("HttpStatusCode")]
        public HttpStatusCode HttpStatusCode { get; set; }
        [JsonProperty("HttpStatusText")]
        public string HttpStatusText { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Result")]
        public object Result { get; set; }
        [JsonProperty("ShowNotification")]
        public bool? ShowNotification { get; set; }
    }
}
