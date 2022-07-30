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
    public class Commands
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BeatmapFunctions beatmapFunctions = new BeatmapFunctions();
        Stopwatch stopwatch = new Stopwatch();
        private TimeSpan _ts;
        public async Task CreatorCommand(SocketSlashCommand command)
        {
            EmbedBuilder embed = new EmbedBuilder()
            .WithColor(Color.Green)
            .WithThumbnailUrl("https://cdn.discordapp.com/attachments/539440828477734962/1002930789043937331/pfp.png")
            .WithDescription("Created by Florentin Lüdeking | Florentin#8768\n" +
                             "[Twitter](https://twitter.com/Florentin_Ldkng)\n" +
                             "[Youtube](https://www.youtube.com/channel/UCaGaEo6MJHLGlpvd2TD1VZA)\n" +
                             "[GitHub](https://github.com/Florentin-Ldkng)\n");
                
            await command.RespondAsync(null, null, false, false, null, null, embed.Build());
        }
        
        public async Task RndCommand(SocketSlashCommand command)
        {
            EmbedBuilder embed = new EmbedBuilder();
            Emoji starEmoji = new Emoji("\U0001f31f");
            stopwatch.Start();
            int stars = Convert.ToInt32(command.Data.Options.First().Value);
            string beatmapLink = beatmapFunctions.GetBeatmap(stars);
            string setId = new Regex("\\/beatmapsets\\/(\\d*)", RegexOptions.IgnoreCase).Match(beatmapLink)
                .Groups[0].Value.Remove(0, 13);
            embed.WithTitle("Your " + stars + $" {starEmoji} request is done")
                .WithDescription("Here's your Beatmap " + command.User.Mention + "\n " + $"[**Click Here!**]({beatmapLink})")
                .WithColor(Color.Green)
                .WithThumbnailUrl($"https://b.ppy.sh/thumb/{setId}l.jpg");
            stopwatch.Stop();
            _ts = stopwatch.Elapsed;
            log.Info($"Download at {DateTime.Now} | Timing: " + _ts.Milliseconds + "ms");
            await command.RespondAsync(null, null, false, false, null, null, embed.Build());
        }
        
        
        public async Task DataBaseCommand(SocketSlashCommand command)
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

            await command.RespondAsync(null, null, false, false, null, null, embed.Build());
        }
    }
}
