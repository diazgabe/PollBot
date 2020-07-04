using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace PollBot.Services {
    public class CommandHandlingService {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        /// <summary>
        /// The prefix of commands that this bot responds to.
        /// </summary>
        public string Prefix { get; set; } = string.Empty;
        /// <summary>
        /// Whether or not to delete commands from the text channel after execution.
        /// </summary>
        public bool DeleteCommands { get; set; } = true;

        public CommandHandlingService( IServiceProvider services ) {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discord.MessageReceived += HandleCommandAsync;
        }

        /// <summary>
        /// Handles incoming messages.
        /// </summary>
        /// <param name="rawMessage"> The incoming message. </param>
        /// <returns> A task that executes a command, if one was found in the message. </returns>
        private async Task HandleCommandAsync( SocketMessage rawMessage ) {
            // Ignore system messages.
            if ( !(rawMessage is SocketUserMessage message) ) {
                return;
            }
            // Ignore bot messages.
            if ( message.Author.IsBot ) {
                return;
            }

            int argPos = 0;
            // Check if the message was intended for this bot.
            if ( !message.HasStringPrefix( Prefix, ref argPos ) ) {
                return;
            }

            // Execute the command.
            SocketCommandContext context = new SocketCommandContext( _discord, message );
            await _commands.ExecuteAsync( context, argPos, _services );
        }

        /// <summary>
        /// Registers command modules.
        /// </summary>
        /// <returns> A Task that registers public command modules. </returns>
        public async Task InitializeAsync() {
            await _commands.AddModulesAsync( Assembly.GetEntryAssembly(), _services );
        }

        /// <summary>
        /// Handles post-command logic.
        /// </summary>
        /// <param name="command"> The executed command. </param>
        /// <param name="context"> The command context. </param>
        /// <param name="result"> The result of the command. </param>
        /// <returns> A task that sends an error message if necessary. </returns>
        private async Task CommandExecutedAsync( Optional<CommandInfo> command, ICommandContext context, IResult result ) {
            // Check if the command was invalid. 
            if ( !command.IsSpecified ) {
                await context.Channel.SendMessageAsync( $"Error: Command not found. Try {Prefix}help for a list of valid commands." );
                return;
            }

            // Handle successful command executions.
            if ( result.IsSuccess ) {
                if ( command.Value.Name == "deleteCommands" ) {
                    DeleteCommands = true;
                } else if ( command.Value.Name == "keepCommands" ) {
                    DeleteCommands = false;
                }

                if ( DeleteCommands ) {
                    await context.Message.DeleteAsync();
                }
                return;
            }

            // The command failed, send an error message.
            await context.Channel.SendMessageAsync( $"error: {result}" );
        }
    }
}