using Discord;
using Discord.Commands;
using OsuRandomizer.DataModel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace OsuRandomizer.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BeatmapFunctions beatmapFunctions = new BeatmapFunctions();
        Stopwatch stopwatch = new Stopwatch();
        private TimeSpan _ts;

        /// <summary>
        /// Spits out a random beatmap of the wanted difficulty
        /// </summary>
        /// <param name="stars">amount of Stars that the beatmap should have</param>
        /// <returns></returns>
        [Command("rnd")]
        [Alias("random")]
        public async Task Rnd([Remainder] string stars)
        {
            int starsConverted = -1;
            try
            {
                starsConverted = Convert.ToInt32(stars);
            }
            catch (Exception e)
            { }

            EmbedBuilder embed = new EmbedBuilder();
            Emoji starEmoji = new Emoji("\U0001f31f");
            if (starsConverted < 0 || starsConverted >= 11)
            {
                embed.WithTitle("Failed " + stars + " " + starEmoji + " request")
                    .WithColor(Color.Red)
                    .WithDescription("Please enter a star amount between 0 and 10");
            }
            else
            {
                stopwatch.Start();
                string beatmapLink = beatmapFunctions.GetBeatmap(starsConverted);
                string setId = new Regex("\\/beatmapsets\\/(\\d*)", RegexOptions.IgnoreCase).Match(beatmapLink)
                    .Groups[0].Value.Remove(0, 13);
                embed.WithTitle("Your " + stars + " " + starEmoji + " request is done")
                    .WithDescription("Here's your Beatmap " + Context.User.Mention + "\n " + $"[**Click Here!**]({beatmapLink})"
                                     )
                    .WithColor(Color.Green)
                    .WithThumbnailUrl($"https://b.ppy.sh/thumb/{setId}l.jpg");

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
            SocketUser stealingMyAccount = Context.Client.GetUser(233946992185704450);
            EmbedAuthorBuilder exampleAuthor = new EmbedAuthorBuilder();
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithAuthor(exampleAuthor)
            .WithColor(Color.Green)
            .WithThumbnailUrl(stealingMyAccount.GetAvatarUrl())
            .WithDescription("[Twitter](https://twitter.com/SuchtpatientTTV)\n" +
                             "[Twitch](https://www.twitch.tv/suchtpatient)\n" +
                             "[Youtube](https://www.youtube.com/channel/UCgrmRtHzT4yL39hHgd7TULQ)\n" +
                             "[GitHub](https://github.com/de-MMXIV)\n");

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
            embed.WithTitle("Changelog (09.08.2021): ")
                .WithColor(Color.Blue)
                .WithDescription("Added thumbnails to .rnd command\n" +
                                 "Changed link to hyperlink in .rnd command\n" +
                                 "Custom error for erroneous values in .rnd\n" +
                                 "Changed .database command to use markdown\n" +
                                 "Changed .creator command to use markdown\n" +
                                 "Fixed typos");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        /// <summary>
        /// spits out the size of the database with a date
        /// </summary>
        /// <returns></returns>
        [Command("database")]
        public async Task DataBase()
        {
            int[] amountArray = new int[11];
            for (int i = 0; i < amountArray.Length; i++)
            {
                amountArray[i] = beatmapFunctions.GetBeatmapAmount(i);
            }

            int totalBeatmaps = amountArray.Sum();
            string description = $"**Total Beatmaps:** {totalBeatmaps}";

            for (int i = 0; i < amountArray.Length; i++)
            {
                description += $"\n**{i} Star{(i >= 2 ? "s" : "")}:** {amountArray[i]}";
            }
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithDescription(description);
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
                .AddField(".rnd <Star amount>", "Retrieves a random Beatmap of the chosen difficulty")
                .AddField(".database", "Shows you the amount of songs that are in the database")
                .AddField(".changelog", "Lists you the latest changes")
                .AddField(".feedback", "Ways to give feedback or report bugs")
                .AddField(".creator", "The guy who programmed the bot and some social media links");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }

        [Command("feedback")]
        public async Task Feedback()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithColor(Color.Blue)
                .WithTitle("Feedback:")
                .WithDescription("You can send me **feedback** or **bug-reports** by @ing me on [Twitter](https://twitter.com/SuchtpatientTTV)\n" +
                                 "\nOr by adding me on Discord" +
                                 "\n**Suchtpatient#8768**");
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }
    }
}
