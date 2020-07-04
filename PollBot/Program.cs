using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PollBot.Services;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PollBot {
    public class Program {
        public static void Main( ) => new Program().RunAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Initializes the bot.
        /// </summary>
        /// <returns> An infinite delay Task. </returns>
        public async Task RunAsync() {
            JsonConfig jsonConfig = ReadConfig();
            // Dispose of the service provider at the end of the bot's lifetime.
            using (ServiceProvider services = ConfigureServices()) {
                // Initialize the DiscordSocketClient.
                DiscordSocketClient client = services.GetRequiredService<DiscordSocketClient>();
                await client.LoginAsync( TokenType.Bot, jsonConfig.Token );
                await client.StartAsync();

                // Initialize the CommandHandlingService.
                CommandHandlingService commandHandlingService = services.GetRequiredService<CommandHandlingService>();
                commandHandlingService.Prefix = jsonConfig.Prefix;
                await commandHandlingService.InitializeAsync();

                // Delay indefinitely.
                await Task.Delay( Timeout.Infinite );
            }
        }

        /// <summary>
        /// Reads the .json config and returns it as an object. 
        /// </summary>
        /// <returns> A JsonConfig object containing the values in the .json config. </returns>
        private JsonConfig ReadConfig() {
            string json = File.ReadAllText( "config.json" );
            return JsonConvert.DeserializeObject<JsonConfig>( json );
        }

        /// <summary>
        /// Returns a configured ServiceProvider.
        /// </summary>
        /// <returns> A configured ServiceProvider. </returns>
        private ServiceProvider ConfigureServices() {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<CommandService>()
                .BuildServiceProvider();
        }
    }
}