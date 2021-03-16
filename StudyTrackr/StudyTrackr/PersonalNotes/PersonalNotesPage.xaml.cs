using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;  
using System.Text;
using System.Threading.Tasks;
using SQLite;
using StudyTrackr.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyTrackr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonalNotesPage : ContentPage
    {
        //public static Notes currentNotes = new Notes();
        //private Notes currentNotes = Globals.CurrentNote;
        private CourseNotes currentCourseNotes = Globals.CurrentCourseNotes;
        private SQLiteAsyncConnection _notesConnection;
        private SQLiteAsyncConnection _courseNotesConnection;
        private ObservableCollection<PersonalNotes> _notes;

        private ObservableCollection<CourseNotes> _courseNotes;
        // private Course currentCourse = Globals.CurrentCourse;
        private readonly User currentUser = Globals.CurrentUser;

        public PersonalNotesPage()
        {
            InitializeComponent();

            _notesConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
            _courseNotesConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            // noteLabel.Text = currentCourse.CourseName;

            await _notesConnection.CreateTableAsync<PersonalNotes>();
            await _courseNotesConnection.CreateTableAsync<CourseNotes>();
            //var addNoteMessage = new Notes()
            //{
            //    NotesId = -1,
            //    NotesTitle = "Create a note!",
            //    Note = "Create a note!"
            //};

            var notesList = await _notesConnection.Table<PersonalNotes>()
                .Where(n => n.UserId.Equals(currentUser.UserId))
                .ToListAsync();

            var courseNotesList = await _courseNotesConnection.Table<CourseNotes>()
                .Where(c => c.UserId.Equals(currentUser.UserId))
                .ToListAsync();


            _notes = new ObservableCollection<PersonalNotes>(notesList);

            if (notesList.Count <= 0)
            {
                notesView.ItemsSource = null;
            }
            else
            {
                notesView.ItemsSource = _notes;
            }

            _courseNotes = new ObservableCollection<CourseNotes>(courseNotesList);

            if (courseNotesList.Count <= 0)
            {
                courseNotesView.ItemsSource = null;
            }
            else
            {
                courseNotesView.ItemsSource = _courseNotes;
            }



            base.OnAppearing();
        }

        private async void ToolbarItem_OnActivated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPersonalNotes());
        }

        private async void NotesView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Globals.CurrentPersonalNotes = (PersonalNotes) e.SelectedItem;

            await Navigation.PushAsync(new DisplayPersonalNotes());
        }

        private async void CourseNotesView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Globals.CurrentCourseNotes = (CourseNotes) e.SelectedItem;

            await Navigation.PushAsync(new DisplayCourseNotes());
        }
    }
}