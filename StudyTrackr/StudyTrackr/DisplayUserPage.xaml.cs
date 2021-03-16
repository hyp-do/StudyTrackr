using System;
using System.Collections.Generic;
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
    public partial class DisplayUserPage : ContentPage
    {
        private readonly User currentUser = Globals.CurrentUser;
        private readonly SQLiteAsyncConnection _displayUserConnection;
        public DisplayUserPage()
        {
            InitializeComponent();

            _displayUserConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await _displayUserConnection.CreateTableAsync<User>();

            var currentStudent = await _displayUserConnection.Table<User>()
                .Where(u => u.UserId.Equals(currentUser.UserId))
                .FirstAsync();

            StudentIdLabel.Text = currentStudent.StudentId.ToString();
            FirstNameLabel.Text = currentStudent.StudentFirstName;
            LastNameLabel.Text = currentStudent.StudentLastName;
            StudentEmailLabel.Text = currentStudent.StudentEmail;


            base.OnAppearing();
        }

        private async void EditStudentButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditUserPage());
        }


        private async void DeleteStudentButton_OnClicked(object sender, EventArgs e)
        {
            var deleteUser = await DisplayAlert("Warning", "Do you want to delete this student?", "Yes", "No");

            if (deleteUser)
            {
                var deleteUserConnection = DependencyService.Get<ISQLiteDb>().GetConnection();

                await deleteUserConnection.DeleteAsync(currentUser);
                await Navigation.PopAsync();
            }
        }
    }
}