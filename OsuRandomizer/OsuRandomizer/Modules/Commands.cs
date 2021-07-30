using Discord;
using Discord.Commands;
using OsuRandomizer.DataModel;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OsuRandomizer.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BeatmapFunctions beatmapFunctions = new BeatmapFunctions();
        Regex reg = new Regex("https?:\\/\\/osu.ppy.sh\\/beatmapsets\\/[0123456789]+\\#osu(\\/[0123456789]+)");
        Stopwatch stopwatch = new Stopwatch();
        private TimeSpan _ts;

        /// <summary>
        /// Spits out a random beatmap of the wanted difficulty
        /// </summary>
        /// <param name="stars">amount of Stars that the beatmap should have</param>
        /// <returns></returns>
        [Command("rnd")]
        public async Task Rnd(int stars)
        {
            EmbedBuilder embed = new EmbedBuilder();
            Emoji starEmoji = new Emoji("\U0001f31f");
            if (stars < 0 || stars >= 11)
            {
                embed.WithTitle("Failed " + stars + " " + starEmoji + " Request")
                    .WithColor(Color.Red)
                    .WithDescription("Please enter a Star amount between 0 and 10");
            }
            else
            {
                stopwatch.Start();

                embed.WithTitle("Your " + stars + " " + starEmoji + " Request is done")
                    .WithDescription("Heres your Beatmap " + Context.User.Mention + "\n " + beatmapFunctions.GetBeatmap(stars))
                    .WithColor(Color.Green);

                stopwatch.Stop();
                _ts = stopwatch.Elapsed;
                log.Info($"Download at {DateTime.Now} | Timing: " + _ts.Milliseconds + "ms");
            }

            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        /// <summary>
        /// Spits out some links of me ;)
        /// </summary>
        /// <returns></returns>
        [Command("creator")]
        public async Task Creator()
        {
            EmbedAuthorBuilder exampleAuthor = new EmbedAuthorBuilder()
                .WithName("Suchtpatient")
                .WithIconUrl("https://cdn.discordapp.com/avatars/233946992185704450/58bad2c3c32ec804d803edc85bdb29f4.png?size=2048");
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithAuthor(exampleAuthor)
            .WithColor(Color.Green)
            .WithDescription("Twitter: https://twitter.com/SuchtpatientTTV " +
                             "\nTwitch: https://www.twitch.tv/suchtpatient " +
                             "\nYoutube: https://www.youtube.com/channel/UCgrmRtHzT4yL39hHgd7TULQ " +
                             "\nGithub: https://github.com/de-MMXIV");

            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }
        /// <summary>
        /// spits out the changes of the week
        /// </summary>
        /// <returns></returns>
        [Command("changelog")]
        public async Task Changelog()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Changelog (03.12.2020): ")
                .WithColor(Color.Blue)
                .WithDescription("Changed Names in .creator\n" +
                                 "Changed Links in .creator\n" +
                                 "Changed Names in .feedback\n" +
                                 "Changed Link in .feedback");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        /// <summary>
        /// spits out the size of the database with a date
        /// </summary>
        /// <returns></returns>
        [Command("database")]
        public async Task DataBase()
        {
            int totalBeatmaps = beatmapFunctions.GetBeatmapAmount(0) +
                                beatmapFunctions.GetBeatmapAmount(1) +
                                beatmapFunctions.GetBeatmapAmount(2) +
                                beatmapFunctions.GetBeatmapAmount(3) +
                                beatmapFunctions.GetBeatmapAmount(4) +
                                beatmapFunctions.GetBeatmapAmount(5) +
                                beatmapFunctions.GetBeatmapAmount(6) +
                                beatmapFunctions.GetBeatmapAmount(7) +
                                beatmapFunctions.GetBeatmapAmount(8) +
                                beatmapFunctions.GetBeatmapAmount(9) +
                                beatmapFunctions.GetBeatmapAmount(10);
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithDescription("Total Beatmaps: " + totalBeatmaps +
                                 "\n0 Star: " + beatmapFunctions.GetBeatmapAmount(0) +
                                 "\n1 Star: " + beatmapFunctions.GetBeatmapAmount(1) +
                                 "\n2 Star: " + beatmapFunctions.GetBeatmapAmount(2) +
                                 "\n3 Star: " + beatmapFunctions.GetBeatmapAmount(3) +
                                 "\n4 Star: " + beatmapFunctions.GetBeatmapAmount(4) +
                                 "\n5 Star: " + beatmapFunctions.GetBeatmapAmount(5) +
                                 "\n6 Star: " + beatmapFunctions.GetBeatmapAmount(6) +
                                 "\n7 Star: " + beatmapFunctions.GetBeatmapAmount(7) +
                                 "\n8 Star: " + beatmapFunctions.GetBeatmapAmount(8) +
                                 "\n9 Star: " + beatmapFunctions.GetBeatmapAmount(9) +
                                 "\n10 Star: " + beatmapFunctions.GetBeatmapAmount(10));
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        /// <summary>
        /// spits out all the commands there are
        /// </summary>
        /// <returns></returns>
        [Command("commands")]
        [Alias("help")]
        public async Task UserCommands()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Commands: ")
                .WithColor(Color.Blue)
                .AddField(".rnd <Star amount>", "Gets you a random Beatmap of the chosen Difficulty")
                .AddField(".database", "Shows you the Amount of Songs that are in the DataBase")
                .AddField(".changelog", "Gets you the latest Changes")
                .AddField(".feedback", "Ways to give Feedback or report Bugs")
                .AddField(".creator", "The Guy who programmed the bot and some social media links");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        [Command("feedback")]
        public async Task Feedback()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithColor(Color.Blue)
                .WithTitle("Feedback:")
                .WithDescription("You can send me feedback or bugreports by @ing me on twitter \n" +
                                 "https://twitter.com/SuchtpatientTTV" +
                                 "\nOr by adding me on Discord" +
                                 "\nSuchtpatient#8768");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }
    }
}
