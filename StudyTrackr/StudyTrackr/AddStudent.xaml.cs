using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyTrackr.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyTrackr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStudent : ContentPage
    {
        
        public AddStudent()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {

            base.OnAppearing();
        }

        private async void AddStudentSaveButton_OnClicked(object sender, EventArgs e)
        {

            var addUserConnection = DependencyService.Get<ISQLiteDb>().GetConnection();

            await addUserConnection.CreateTableAsync<User>();


            if ((Validation.IsFieldNull(StudentIdEntry.Text)
                 && (Validation.IsFieldNull(FirstNameEntry.Text)
                     && (Validation.IsFieldNull(PasswordEntry.Text)
                         && (Validation.IsFieldNull(LastNameEntry.Text)
                             && (Validation.IsFieldNull(StudentEmailEntry.Text)))))))
            {
                if (Validation.IsEmailValid(StudentEmailEntry.Text))
                {
                    if (int.TryParse(StudentIdEntry.Text, out int studentNumResult))
                    {

                        try
                        {
                            var user = new User()
                            {
                                StudentId = Convert.ToInt32(StudentIdEntry.Text),
                                Password = PasswordEntry.Text,
                                StudentFirstName = FirstNameEntry.Text,
                                StudentLastName = LastNameEntry.Text,
                                StudentEmail = StudentEmailEntry.Text,
    
                            };

                            await addUserConnection.InsertAsync(user);

                            await Navigation.PopAsync();
                        }
                        catch (Exception exception)
                        {
                            await DisplayAlert("ERROR", $"{exception}","Ok");
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