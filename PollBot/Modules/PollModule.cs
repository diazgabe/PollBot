using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollBot.Commands {
    public class PollModule : ModuleBase {
        // Set the bot to delete commands after executing them.
        [Command( "deleteCommands" )]
        public async Task HideCommands() {
            await ReplyAsync( "Hiding commands." );
        }

        // Set the bot to keep commands after executing them.
        [Command( "keepCommands" )]
        public async Task ShowCommands() {
            await ReplyAsync( "Showing commands." );
        }

        // Display the help message.
        [Command( "help" )]
        public async Task Help() {
            List<string> emojiCodes = new List<string> {
                "\uD83C\uDF83",
                "\uD83D\uDC40",
                "\uD83C\uDF50"
            };

            string message = "A bot used to create polls." + Environment.NewLine + Environment.NewLine
                + "Commands:" + Environment.NewLine
                + "    help                                 Shows this message" + Environment.NewLine
                + $"    poll                                  Creates a poll. Example: poll a({emojiCodes[ 0 ]}), b({emojiCodes[ 1 ]}), c({emojiCodes[ 2 ]})" + Environment.NewLine
                + "    deleteCommands        Deletes commands after executing them" + Environment.NewLine
                + "    keepCommands           Keeps commands after executing them" + Environment.NewLine;

            await ReplyAsync( message );
        }

        // Create a new poll.
        [Command( "poll" )]
        public async Task Poll( [Remainder] string args = "" ) {
            string[] options = args.Split( ',' );
            string message = string.Empty;

            List<IEmote> emojiCodes = new List<IEmote>();
            foreach ( string option in options ) {
                // Get the poll option text.
                message += option.TrimStart() + Environment.NewLine;

                // Get the poll option reaction.
                string emojiCode = option.Split( '(', ')' )[ 1 ].Trim();
                if ( emojiCode.Contains( ':' ) ) {
                    // If the reaction is a custom emoji, get the emoji code from the Guild.
                    string customEmojiName = emojiCode.Split( ':' )[ 1 ];
                    emojiCodes.Add( Context.Guild.Emotes.First( x => x.Name == customEmojiName ) );
                } else {
                    emojiCodes.Add( new Emoji( emojiCode ) );
                }
            }

            // Create the poll in the channel.
            IUserMessage sent = await ReplyAsync( message );
            // Add reactions to the poll.
            await sent.AddReactionsAsync( emojiCodes.ToArray() );
        }
    }
}