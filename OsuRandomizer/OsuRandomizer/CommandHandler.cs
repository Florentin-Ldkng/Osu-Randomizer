using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace OsuRandomizer
{
    public class CommandHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DiscordSocketClient _client;
        private CommandService _service;
        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            _service.AddModulesAsync(Assembly.GetEntryAssembly(), services: null);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            
            var msg = s as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;
            if (msg.HasCharPrefix('.',ref argPos))
            {
                var result = await _service.ExecuteAsync(context: context,argPos: argPos,services: null);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    EmbedBuilder embed = new EmbedBuilder()
                        .WithTitle("Internal Error")
                        .WithColor(Color.Red)
                        .WithDescription($"Please contact **Suchtpatient#8768**!" +
                                         $"\nPlease include this Timestamp" +
                                         $"\n```{DateTime.Now}```");
                    await context.Channel.SendMessageAsync(null,false,embed.Build());
                    log.Error(result.ErrorReason);
                }
            }
        }
    }
}
