using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotifications;
using StudyTrackr.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyTrackr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCourse : ContentPage
    {
        private readonly Term _currentTerm = Globals.CurrentTerm;
        public AddCourse()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            courseStatus.SelectedIndex = 0;

            base.OnAppearing();
        }

        private async void CourseSave_OnClicked(object sender, EventArgs e)
        {


            if ((Validation.IsFieldNull(courseName.Text)
                 && (Validation.IsFieldNull(instructorName.Text)
                     && (Validation.IsFieldNull(instructorEmail.Text)
                         && (Validation.IsFieldNull(courseStatus.SelectedItem.ToString())
                             && (Validation.IsFieldNull(instructorPhone.Text)))))))
            {
                if (Validation.IsEmailValid(instructorEmail.Text))
                {
                    if (Validation.IsPhoneNumberValid(instructorPhone.Text))
                    {

                        if (courseStartDate.Date < courseEndDate.Date)
                        {
                            try
                            {
                                var addCourseConnection = DependencyService.Get<ISQLiteDb>().GetConnection();

                                await addCourseConnection.CreateTableAsync<Course>();

                                var course = new Course()
                                {
                                    CourseName = courseName.Text,
                                    CourseStartDate = courseStartDate.Date,
                                    CourseEndDate = courseEndDate.Date,
                                    UserId = _currentTerm.UserId,
                                    TermId = _currentTerm.TermId,
                                    CourseStatus = courseStatus.SelectedItem.ToString(),
                                    InstructorName = instructorName.Text,
                                    InstructorEmail = instructorEmail.Text,
                                    InstructorPhone = instructorPhone.Text,
                                    CourseNotification = courseNotification.IsToggled
                                };

                                await addCourseConnection.InsertAsync(course);

                                await Navigation.PopAsync();
                            }
                            catch (Exception exception)
                            {
                                await DisplayAlert("ERROR", $"{exception}", "OK");
                            }

                        }
                        else
                        {
                            await DisplayAlert("ERROR", "The Start Date must be before the End Date", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("ERROR", "The phone number is not in a valid format. Please try again.",
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