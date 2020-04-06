﻿using System;
using System.Diagnostics;
using Discord;
using Discord.WebSocket;
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
            _client = new DiscordSocketClient();
            log.Info("Logging in...");
            
            await _client.LoginAsync(TokenType.Bot, "");
            log.Info("Logged in");
            await _client.StartAsync();
            _handler = new CommandHandler(_client);
            stopwatch.Stop();
            log.Info("Bot is Running after " + stopwatch.ElapsedMilliseconds + "ms of bootup");
            await Task.Delay(-1);
        }
    }
}
