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
    public partial class EditUserPage : ContentPage
    {
        private readonly User currentUser = Globals.CurrentUser;
        private SQLiteAsyncConnection _userConnection;
        private ObservableCollection<User> _users;

        public EditUserPage()
        {
            InitializeComponent();

            _userConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await _userConnection.CreateTableAsync<User>();

            var currentStudent = await _userConnection.Table<User>()
                .Where(u => u.UserId.Equals(currentUser.UserId))
                .FirstAsync();


            StudentIdEntry.Text = currentStudent.StudentId.ToString();
            FirstNameEntry.Text = currentStudent.StudentFirstName;
            LastNameEntry.Text = currentStudent.StudentLastName;
            StudentEmailEntry.Text = currentStudent.StudentEmail;

            base.OnAppearing();
        }

        private async void EditStudentSaveButton_OnClicked(object sender, EventArgs e)
        {
            var editUserConnection = DependencyService.Get<ISQLiteDb>().GetConnection();

            await editUserConnection.CreateTableAsync<User>();

           bool isNumber = int.TryParse(StudentIdEntry.Text, out int studentNumResult);


            if ((Validation.IsFieldNull(StudentIdEntry.Text)
                 && (Validation.IsFieldNull(FirstNameEntry.Text)
                     && (Validation.IsFieldNull(LastNameEntry.Text)
                         && (Validation.IsFieldNull(StudentEmailEntry.Text))))))
            {
                if (Validation.IsEmailValid(StudentEmailEntry.Text))
                {
                    if (isNumber)
                    {
                        try
                        {
                            var user = new User()
                            {
                                UserId = currentUser.UserId,
                                StudentId = currentUser.StudentId, //TODO: Make this editable?
                                Password = PasswordEntry.Text,
                                StudentFirstName = FirstNameEntry.Text,
                                StudentLastName = LastNameEntry.Text,
                                StudentEmail = StudentEmailEntry.Text,

                            };

                            await editUserConnection.UpdateAsync(user);

                            await Navigation.PopAsync();
                        }
                        catch (Exception exception)
                        {
                            await DisplayAlert("ERROR", $"{exception}", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("ERROR", "The Student ID must be a number.",
                            "OK");
                    }
                }
                else
                {
                    await DisplayAlert("ERROR", "The email address is not valid, please check it.", "OK");
                }

            }
            else
            {
                await DisplayAlert("ERROR", "There is an empty field, check all fields before submitting.", "OK");
            }
        }
    }
}