using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace StudyTrackr.Database
{
    public class User
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }

        public int StudentId { get; set; }

        public string Password { get; set; }


        private string _studentFirstName;

        public string StudentFirstName
        {
            get { return _studentFirstName; }

            set
            {
                if (_studentFirstName == value)
                {
                    return;
                }
                else
                {
                    _studentFirstName = value;
                }

                OnPropertyChanged(nameof(StudentFirstName));
            }

        }

        private string _studentLastName;


        public string StudentLastName
        {
            get { return _studentLastName; }

            set
            {
                if (_studentLastName == value)
                {
                    return;
                }
                else
                {
                    _studentLastName = value;
                }

                OnPropertyChanged(nameof(StudentLastName));
            }

        }

        private string _studentEmail;

        public string StudentEmail
        {
            get { return _studentEmail; }

            set
            {
                if (_studentEmail == value)
                {
                    return;
                }
                else
                {
                    _studentEmail = value;
                }

                OnPropertyChanged(nameof(StudentEmail));
            }

        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}