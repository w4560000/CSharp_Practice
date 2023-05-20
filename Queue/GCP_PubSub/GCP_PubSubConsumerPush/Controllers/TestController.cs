using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GCP_PubSubConsumerPush.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handle authenticated push request coming from pubsub.
        /// </summary>
        [HttpPost]
        [Route("/AuthPush")]
        public async Task<IActionResult> AuthPushAsync([FromBody] PushBody body, [FromQuery] string token)
        {
            try
            {
                string bodyJson = JsonConvert.SerializeObject(body);

                string authorizaionHeader = HttpContext.Request.Headers["Authorization"];

                if (!string.IsNullOrEmpty(authorizaionHeader))
                {
                    string authToken = authorizaionHeader.StartsWith("Bearer ") ? authorizaionHeader.Substring(7) : string.Empty;
                    // Verify and decode the JWT.
                    var payload = await JsonWebSignature.VerifySignedTokenAsync<PubSubPayload>(authToken);

                    string payloadJson = JsonConvert.SerializeObject(payload);
                }

                string verificationToken = token ?? body.message.attributes["token"];
                if (verificationToken != "123")
                    return new BadRequestResult();

                var messageBytes = Convert.FromBase64String(body.message.data);
                string message = System.Text.Encoding.UTF8.GetString(messageBytes);

                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class PubSubPayload : JsonWebSignature.Payload
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public string EmailVerified { get; set; }
    }

    public class PushBody
    {
        public PushMessage message { get; set; }
        public string subscription { get; set; }
    }

    public class PushMessage
    {
        public Dictionary<string, string> attributes { get; set; }
        public string data { get; set; }
        public string message_id { get; set; }
        public string publish_time { get; set; }
    }
}