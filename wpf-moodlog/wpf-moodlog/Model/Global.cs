using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_moodlog
{
    public static class Global
    {
        public static User User
        {
            get;
            set;
        }
        public static MoodLogEntriesPage EntriesPage
        {
            get;
            set;
        }
        public static MoodLogProfilePage ProfilePage
        {
            get;
            set;
        }

        public static MoodLogStatsPage StatsPage
        {
            get;
            set;
        }
    }
}
