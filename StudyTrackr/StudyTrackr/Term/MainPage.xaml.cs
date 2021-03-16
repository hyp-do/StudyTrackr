using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotifications;
using SQLite;
using StudyTrackr.Helpers;
using StudyTrackr.Database;
using Xamarin.Forms;

namespace StudyTrackr
{

    public partial class MainPage : ContentPage
    {
        private readonly User currentUser = Globals.CurrentUser;
        private SQLiteAsyncConnection _termConnection;
        // private SQLiteAsyncConnection _notificationConnection;
        // private SQLiteAsyncConnection _termSearchConnection;
        private ObservableCollection<Term> _terms;


        public MainPage()
        {
            InitializeComponent();

            _termConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
            // _notificationConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
            // _termSearchConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {

            await _termConnection.CreateTableAsync<Term>();


                var termList = await _termConnection.Table<Term>()
                    .Where(u => u.UserId.Equals(currentUser.UserId))
                    .ToListAsync();

                _terms = new ObservableCollection<Term>(termList);

                termView.ItemsSource = _terms;


            // await _notificationConnection.CreateTableAsync<Course>();
            // await _notificationConnection.CreateTableAsync<Assessment>();
            // var courseList = await _notificationConnection.Table<Course>()
            //     .Where(u => u.UserId.Equals(currentUser.UserId))
            //     .ToListAsync();
            // var assessmentList = await _notificationConnection.Table<Assessment>()
            //     .Where(a => a.UserId.Equals(currentUser.UserId))
            //         .ToListAsync();

            // await _termConnection.CreateTableAsync<Course>();
            // await _termConnection.CreateTableAsync<Assessment>();

            var courseList = await _termConnection.Table<Course>()
                .Where(u => u.UserId.Equals(currentUser.UserId))
                .ToListAsync();
            var assessmentList = await _termConnection.Table<Assessment>()
                .Where(a => a.UserId.Equals(currentUser.UserId))
                .ToListAsync();

            int courseNotificationId = 0;
            int assessmentNotificationId = 0;

            foreach (Course course in courseList)
            {
                courseNotificationId++;
                if (course.CourseNotification)
                {
                    if (course.CourseStartDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Reminder", $"Your course {course.CourseName} starts today!", courseNotificationId);
                    }

                    if (course.CourseEndDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Reminder", $"Your Course {course.CourseName} ends today!", courseNotificationId);
                    }
                }
            }

            foreach (Assessment assessment in assessmentList)
            {
                assessmentNotificationId++;
                if (assessment.AssessmentNotification)
                {
                    if (assessment.AssessmentStartDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Reminder", $"Assessment {assessment.AssessmentTitle} starts today!", assessmentNotificationId);
                    }

                    if (assessment.AssessmentEndDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Reminder", $"Assessment {assessment.AssessmentTitle} ends today!", assessmentNotificationId);
                    }
                }
            }


            base.OnAppearing();
        }

        private async void AddTerm_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTermPage());
        }

        private async void Notes_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PersonalNotesPage());
        }

        private void TermView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Globals.CurrentTerm = (Term)e.Item;
        }

        private async void TermView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Globals.CurrentTerm = (Term)e.SelectedItem;

            await Navigation.PushAsync(new TermPage());

        }

        private async void TermSearchEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // await _termSearchConnection.CreateTableAsync<Term>();

            // var termSearchList = await _termSearchConnection.Table<Term>()
            //     .Where(u => u.UserId.Equals(currentUser.UserId))
            //     .ToListAsync();

            var termSearchList = await _termConnection.Table<Term>()
                .Where(u => u.UserId.Equals(currentUser.UserId))
                .ToListAsync();

            var termFoundList = new List<Term>();

            bool termFound = false;

            if (!String.IsNullOrEmpty(TermSearchEntry.Text))
            {
                foreach (Term term in termSearchList)
                {
                    if (term.TermTitle.ToUpper().Contains(TermSearchEntry.Text.ToUpper()))
                    {
                        termFoundList.Add(term);
                        termFound = true;
                    }
                }
            }

            if (termFound)
            {
                termView.ItemsSource = termFoundList;
            }
            if (!(termFound) && !(String.IsNullOrEmpty(TermSearchEntry.Text)))
            {
                termView.ItemsSource = null;
            }
            else if (String.IsNullOrEmpty(TermSearchEntry.Text))
            {
                termView.ItemsSource = _terms;
            }
        }
    }
}
