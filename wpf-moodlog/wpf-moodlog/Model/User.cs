using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

namespace wpf_moodlog
{
    public enum Gender { Male, Female };

    public static class GenderExtensions
    {
        public static string GetName(this Gender e)
        {
            return Enum.GetName(typeof(Gender), e);
        }
    }
    public class User
    {
        public User(string ID,
                    string FirstName,
                    string LastName,
                    string GenderCode,
                    string Birthday,
                    string Username,
                    string Password,
                    string Entries) 

        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.GenderCode = GenderCode;
            this.Birthday = Birthday;
            this.Username = Username;
            this.Password = Password;
            this.Entries = Entries;
        }


        string _iD;
        public string ID
        {
            get;
            set;
        }

        string _firstName;
        public string FirstName
        {
            get;
            set;
        }

        string _lastName;
        public string LastName
        {
            get;
            set;
        }

        Gender _genderCode;
        public string GenderCode
        {
            get
            {
                return this._genderCode.GetName();
            }
            set
            {
                if (value == "M")
                {
                    this._genderCode = Gender.Male;
                }
                else
                {
                    this._genderCode = Gender.Female;
                }
            }
        }

        string _birthday;
        public string Birthday
        {
            get;
            set;
        }

        string _username;
        public string Username
        {
            get;
            set;
        }

        string _password;
        public string Password
        {
            get;
            set;
        }

        string _entries;
        public string Entries
        {
            get;
            set;
        }

        public Stream GetEntriesStream()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("wpf_moodlog.Data." + this._entries + ".csv");
        }
    }
}
