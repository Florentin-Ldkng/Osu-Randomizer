﻿using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Discord;
using Discord.WebSocket;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OsuRandomizer
{
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Stopwatch stopwatch = new Stopwatch();
        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandHandler _handler;

        public async Task StartAsync()
        {
            
            stopwatch.Start();
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(MySqlConnectionProvider.CreateConnection(Credentials.DBConnectionString), AutoCreateOption.None);
            _client = new DiscordSocketClient();
            log.Info("Logging in...");
            await _client.LoginAsync(TokenType.Bot, Credentials.DiscordToken);
            log.Info("Logged in");
            await _client.StartAsync();
            //await CreateCommands();
            _handler = new CommandHandler(_client);
            stopwatch.Stop();
            log.Info("Bot is Running after " + stopwatch.ElapsedMilliseconds + "ms of bootup");
            await Task.Delay(-1);
        }

        private async Task CreateCommands()
        {
            var globalCommand = new SlashCommandBuilder();

            #region DatabaseCommand
            globalCommand.WithName("database");
            globalCommand.WithDescription("Returns the amount of Beatmaps in the database");
            await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            #endregion
            
            #region CreatorCommand
            globalCommand.WithName("creator");
            globalCommand.WithDescription("Returns the creator of the Bot");
            await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            #endregion

            #region StarCommand
           globalCommand.WithName("rnd");
           globalCommand.WithDescription("Returns a random Beatmap of the desired difficulty");
           globalCommand.AddOption("stars", ApplicationCommandOptionType.Integer, "The stars of the Beatmap",
               isRequired: true, false, false, 0, 10);
           await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
           #endregion
        }
    }
}
