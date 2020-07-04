using Newtonsoft.Json;

namespace PollBot {
    public class JsonConfig {
        /// <summary>
        /// The command prefix that this bot will respond to.
        /// </summary>
        [JsonProperty( "prefix" )]
        public string Prefix { get; private set; }
        /// <summary>
        /// The Discord bot token.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; private set; }
    }
}