using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OsuRandomizer.Modules;

namespace OsuRandomizer
{
    public class CommandHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DiscordSocketClient _client;
        private Commands _commands;
        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _commands = new Commands();
            _client.SlashCommandExecuted += HandleSlashCommandAsync;
        }

        private async Task HandleSlashCommandAsync(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "creator":
                    await _commands.CreatorCommand(command);
                    break;
                case "database":
                    await _commands.DataBaseCommand(command);
                    break;
                case "rnd":
                    await _commands.RndCommand(command);
                    break;
            }
        }
    }
}
