
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace StudyTrackr.Database
{
    public class CourseNotes : Notes
    {

        public int CourseId { get; set; }

        public int UserId { get; set; }

        private string _type { get; set; }

        public string Type
        {
            get { return _type;}
            set { _type = "Course Note"; }
        }
    }

}