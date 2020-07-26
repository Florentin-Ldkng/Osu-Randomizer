using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using mysqltest.DataModel;

namespace OsuRandomizer.DataModel
{
    class BeatmapFunctions
    {
        Random rnd = new Random();
        public string GetBeatmap(int star)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                IQueryable<Beatmap> beatmap = uow.Query<Beatmap>()
                    .Where(reference => reference.Difficultyrating >= star && reference.Difficultyrating < star+1);
                var beatmaps = beatmap.ToArray();
                int randomMap = rnd.Next(0, beatmaps.Length);
                string output = "https://osu.ppy.sh/beatmapsets/" + beatmaps[randomMap].BeatmapsetId + "#osu/" + beatmaps[randomMap].BeatmapId;
                return output;
            }
        }

        public int GetBeatmapAmount(int star)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                IQueryable<Beatmap> beatmap = uow.Query<Beatmap>()
                    .Where(reference => reference.Difficultyrating >= star && reference.Difficultyrating < star + 1);
                var beatmaps = beatmap.ToArray();
                
                return beatmaps.Length;
            }
        }
    }
}
