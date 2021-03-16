using System.ComponentModel;
using SQLite;

namespace StudyTrackr.Database
{
    public class PersonalNotes : Notes
    {

        public int UserId { get; set; }


        private string _type { get; set; }

        public string Type
        {
            get { return _type; }
            set { _type = "Personal Note"; }
        }
    }

}