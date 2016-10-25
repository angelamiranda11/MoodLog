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


        private string _iD;
        public string ID
        {
            get
            {
                return _iD;
            }
            set
            {
                _iD = value;
            }
        }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
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
            get
            {
                return _birthday;
            }
            set
            {
                _birthday = value;
            }
        }

        string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        string _entries;
        public string Entries
        {
            get
            {
                return _entries;
            }
            set
            {
                _entries = value;
            }
        }
    }
}
