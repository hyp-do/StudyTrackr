using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using StudyTrackr.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PropertyChangingEventArgs = Xamarin.Forms.PropertyChangingEventArgs;

namespace StudyTrackr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPersonalNotes : ContentPage
    {
        //private Course currentCourse = Globals.CurrentCourse;
        private User currentUser = Globals.CurrentUser;
        private SQLiteAsyncConnection _courseConnection;

        public AddPersonalNotes()
        {
            InitializeComponent();

            _courseConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await _courseConnection.CreateTableAsync<Course>();

            PersonalLabel.Text = currentUser.StudentId + " Notes";

            var courseList = await _courseConnection.Table<Course>()
                .Where(c => c.UserId.Equals(currentUser.UserId))
                .ToListAsync();

            var courseNameList = new List<String>();

            foreach (Course course in courseList)
            {
                courseNameList.Add(course.CourseName);
            }

            NoteTypePicker.SelectedIndex = 0;
            CoursePicker.ItemsSource = courseNameList;

            base.OnAppearing();
        }

        private async void NoteSaveButton_OnClicked(object sender, EventArgs e)
        {


            if ((Validation.IsFieldNull(NoteTitleEntry.Text)) && (Validation.IsFieldNull(NoteEditor.Text)))
            {

                var addNoteConnection = DependencyService.Get<ISQLiteDb>().GetConnection();


                if (NoteTypePicker.SelectedItem.ToString() == "Personal")
                {
                    await addNoteConnection.CreateTableAsync<PersonalNotes>();

                    var note = new PersonalNotes()
                    {
                        UserId = currentUser.UserId,
                        NotesTitle = NoteTitleEntry.Text,
                        Note = NoteEditor.Text,
                    };

                    await addNoteConnection.InsertAsync(note);

                    await Navigation.PopAsync();
                }

                if (NoteTypePicker.SelectedItem.ToString() == "Course")
                {
                    try
                    {
                        var selectedCourse = CoursePicker.SelectedItem.ToString();

                        var courseId = await _courseConnection.Table<Course>()
                            .Where(c => c.CourseName.Equals(selectedCourse))
                            .FirstAsync();

                        var note = new CourseNotes()
                        {
                            UserId = currentUser.UserId,
                            CourseId = courseId.CourseId,
                            NotesTitle = NoteTitleEntry.Text,
                            Note = NoteEditor.Text,
                        };

                        await addNoteConnection.CreateTableAsync<CourseNotes>();

                        await addNoteConnection.InsertAsync(note);

                        await Navigation.PopAsync();
                    }
                    catch (NullReferenceException ex)
                    {
                        await DisplayAlert("ERROR", "You must choose a course to save a course note to!", "Ok");
                    }

                }
            }
            else
            {
                await DisplayAlert("ERROR", "There is an empty field, check all fields before submitting.", "OK");
            }
        }


        // private void CoursePicker_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        // {
        //     
        // }
        private void NoteTypePicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (NoteTypePicker.SelectedItem.ToString() == "Personal")
            {
                CoursePicker.IsEnabled = false;
                CoursePicker.IsVisible = false;
                CourseLabel.IsVisible = false;
                CourseLabel.IsEnabled = false;

            }
            else if (NoteTypePicker.SelectedItem.ToString() == "Course")
            {
                CoursePicker.IsEnabled = true;
                CoursePicker.IsVisible = true;
                CourseLabel.IsVisible = true;
                CourseLabel.IsEnabled = true;
            }
        }
    }
}