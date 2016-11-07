using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_moodlog
{
    public static class Global
    {
        public const string Path = "C:\\";
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

        public static Stream GetStreamOf(string filename, FileMode filemode)
        {
            return new FileStream(Path + filename, filemode);
        }
    }
}
