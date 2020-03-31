﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using OsuRandomizerTool;

namespace OsuRandomizer.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private DataBase _jsonDataBase = JsonConvert.DeserializeObject<DataBase>(File.ReadAllText(@"D:\Osu! Randomizer\DataBase.json"));
        Regex reg = new Regex("https?:\\/\\/osu.ppy.sh\\/beatmapsets\\/[0123456789]+\\#osu(\\/[0123456789]+)");

        /// <summary>
        /// Spits out a random beatmap of the wanted difficulty
        /// </summary>
        /// <param name="stars">amount of Stars that the beatmap should have</param>
        /// <returns></returns>
        [Command("rnd")]
        public async Task Rnd(int stars)
        {
            Serialize();
             _jsonDataBase.Downloads++;
             Serialize();
             var starEmoji = new Emoji("\U0001f31f");
             var embed = new EmbedBuilder( );
             embed.WithTitle("Your " + stars +" "+ starEmoji +" Request is done")
                 .WithDescription("Heres your Beatmap " + Context.User.Mention + "\n " + _jsonDataBase.GetRandomMap(stars))
                 .WithColor(Color.Green);
             await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }
        
        /// <summary>
        /// Allows users to request levels
        /// </summary>
        /// <param name="request">the link</param>
        /// <returns></returns>
        [Command("request")]
        public async Task Request(string request)
        {
            MatchCollection mc = reg.Matches(request);

            if (mc.Count > 0)
            {
                _jsonDataBase.UserRequest(mc[0].Value);
                Serialize();
                var embed = new EmbedBuilder();
                embed.WithColor(Color.Green)
                    .WithTitle("Request Send")
                    .WithDescription("Then you for your Request " + Context.User.Mention + "!\n" +
                                     "It was send to a Moderator\n" +
                                     "After it gets approved it will be in the Data Base");
                await Context.Channel.SendMessageAsync(null, false, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Color.Red)
                    .WithTitle("Request NOT Send")
                    .WithDescription("Please only submit valid links to osu! beatmaps");
                await Context.Channel.SendMessageAsync(null, false, embed.Build());
            }
            
                    
           
        }

        /// <summary>
        /// Spits out some links of me ;)
        /// </summary>
        /// <returns></returns>
        [Command("creator")]
        public async Task Creator()
        {
            var exampleAuthor = new EmbedAuthorBuilder()
                .WithName("MMXIV")
                .WithIconUrl("https://cdn.discordapp.com/avatars/233946992185704450/11d04a3f77879f2f4c61cd75b29c1d43.png?size=128");
            var embed = new EmbedBuilder();
                embed.WithAuthor(exampleAuthor)
                .WithColor(Color.Green)
                .WithDescription("Twitter: https://twitter.com/de_mmxiv " +
                                 "\nTwitch: https://www.twitch.tv/de_mmxiv " +
                                 "\nYoutube: https://www.youtube.com/channel/UCgrmRtHzT4yL39hHgd7TULQ");

            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        /// <summary>
        /// Spits out the maintenance times
        /// </summary>
        /// <returns></returns>
        [Command("downtime")]
        public async Task Downtime()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Maintenance Times: ")
                .WithColor(Color.Red)
                .WithDescription("Everyday at 12pm for 10 minutes\n" +
                                 "Every Sunday at 6pm for 30 minutes\n" +
                                 "During these times the bot may go offline\n" +
                                 "UTC+1");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        /// <summary>
        /// spits out the changes of the week
        /// </summary>
        /// <returns></returns>
        [Command("changelog")]
        public async Task Changelog()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Changelog ("+ System.DateTime.Today.ToShortDateString()+"): ")
                .WithColor(Color.Blue)
                .WithDescription("Created the Bot\n" +
                                 "Added changelog Command\n" +
                                 "Added downtime Command\n" +
                                 "Added creator Command\n" +
                                 "Added request Command\n" +
                                 "Added rng Command\n" +
                                 "Started The Database");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        /// <summary>
        /// spits out the size of the database with a date
        /// </summary>
        /// <returns></returns>
        [Command("database")]
        public async Task DataBase()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Database(" + System.DateTime.Today.ToShortDateString() +")")
                .WithColor(Color.Blue)
                .WithDescription("1 Star : " + _jsonDataBase.Star1.Count + "\n" +
                                 "2 Star : " + _jsonDataBase.Star2.Count + "\n" +
                                 "3 Star : " + _jsonDataBase.Star3.Count + "\n" +
                                 "4 Star : " + _jsonDataBase.Star4.Count + "\n" +
                                 "5 Star : " + _jsonDataBase.Star5.Count + "\n" +
                                 "6 Star : " + _jsonDataBase.Star6.Count + "\n" +
                                 "7 Star : " + _jsonDataBase.Star7.Count + "\n" +
                                 "8 Star : " + _jsonDataBase.Star8.Count + "\n" +
                                 "9 Star : " + _jsonDataBase.Star9.Count + "\n" +
                                 "10 Star : " + _jsonDataBase.Star10.Count + "\n");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        /// <summary>
        /// spits out all the commands there are
        /// </summary>
        /// <returns></returns>
        [Command("commands")]
        public async Task UserCommands()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Commands: ")
                .WithColor(Color.Blue)
                .AddField(".rnd <Star amount>", "Gets you a random Beatmap of the chosen Difficulty")
                .AddField(".request <link>", "Request a Beatmap you want to see in the Database to the mod")
                .AddField(".database", "Shows you the Amount of Songs that are in the DataBase")
                .AddField(".downtime", "Shows you when the Bot will be down because of Maintenance")
                .AddField(".changelog", "Gets you the latest Changes")
                .AddField(".creator", "The Guy programmed the bot and some social media links");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }
        /// <summary>
        /// Serializes the database.json to a file
        /// </summary>
        private void Serialize()
        {
            File.WriteAllText(@"D:\Osu! Randomizer\DataBase.json", JsonConvert.SerializeObject(_jsonDataBase));
        }
    }
}