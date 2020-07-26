using DevExpress.Xpo;
using System;
using System.Data;

namespace mysqltest.DataModel
{
    [Persistent("osu_beatmaps")]
    class Beatmap : XPLiteObject
    {
        public Beatmap(Session session) : base(session)
        {
        }

        private int beatmap_id;
        [Key (true)]
        [Persistent("beatmap_id")]
        public int BeatmapId
        {
            get { return beatmap_id; }
            set { SetPropertyValue(nameof(BeatmapId), ref beatmap_id, value); }
        }

        int beatmapset_id;
        [Indexed(Unique = false)]
        [Persistent("beatmapset_id")]
        public int BeatmapsetId
        {
            get { return beatmapset_id; }
            set { SetPropertyValue(nameof(BeatmapsetId), ref beatmapset_id, value); }
        }

        int playmode;
        [Persistent("playmode")]
        public int Playmode
        {
            get { return playmode; }
            set { SetPropertyValue(nameof(playmode), ref playmode, value); }
        }

        int approved;
        [Persistent("approved")]
        public int Approved
        {
            get { return approved; }
            set { SetPropertyValue(nameof(Approved), ref approved, value); }
        }

        float difficultyrating;
        [Persistent("difficultyrating")]
        [Indexed (Unique = false)]
        public float Difficultyrating
        {
            get { return difficultyrating; }
            set { SetPropertyValue(nameof(Difficultyrating), ref difficultyrating, value); }
        }
    }
}
