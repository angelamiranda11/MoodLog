using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_moodlog
{
    public static class Global
    {
        private static User s_sUser;
        public static User User
        {
            get;
            set;
        }
    }
}
