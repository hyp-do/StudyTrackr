using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using StudyTrackr.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyTrackr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAssessment : ContentPage
    {
        private string _assessmentType;
        private Assessment _currentAssessment = Globals.CurrentAssessment;
        private Course _currentCourse = Globals.CurrentCourse;

        public EditAssessment()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {

            assessmentTitle.Text = _currentAssessment.AssessmentTitle;
            assessmentStartDate.Date = _currentAssessment.AssessmentStartDate.Date;
            assessmentEndDate.Date = _currentAssessment.AssessmentEndDate;

            pASwitch.IsEnabled = false;
            oASwitch.IsEnabled = false;

            if (_currentAssessment.Type.Contains("OA"))
            {
                oASwitch.IsToggled = true;

            }
            else if (_currentAssessment.Type.Contains("PA"))
            {

                pASwitch.IsToggled = true;
                

            }

            assessmentNotifications.IsToggled = _currentAssessment.AssessmentNotification;


            base.OnAppearing();
        }

        private async void EditAssessmentSaveButton_OnClicked(object sender, EventArgs e)
        {
            var assessment = new Assessment()
            {
                AssessmentId = _currentAssessment.AssessmentId,
                CourseId = _currentAssessment.CourseId,
                UserId = _currentAssessment.UserId,
                AssessmentTitle = assessmentTitle.Text,
                AssessmentStartDate = assessmentStartDate.Date,
                AssessmentEndDate = assessmentEndDate.Date,
                Type = _assessmentType,
                AssessmentNotification = assessmentNotifications.IsToggled

            };

            var editAssessmentConnection = DependencyService.Get<ISQLiteDb>().GetConnection();

            var assessmentList = await editAssessmentConnection.Table<Assessment>()
                .Where(a => a.CourseId.Equals(_currentCourse.CourseId))
                .ToListAsync();

            var oACounter = 0;

            var pACounter = 0;

            foreach (Assessment assessmentInList in assessmentList)
            {
                if (assessmentInList.Type == "PA")
                {
                    pACounter++;
                }

                if (assessmentInList.Type == "OA")
                {
                    oACounter++;
                }
            }

            if (!((pACounter > 0) && (assessment.Type == "PA")))
            {
                if (!((oACounter > 0) && (assessment.Type == "OA")))
                {

                    if (oASwitch.IsToggled || pASwitch.IsToggled)
                    {
                        if (Validation.IsFieldNull(assessment.AssessmentTitle))
                        {
                            if (assessment.AssessmentStartDate < assessment.AssessmentEndDate)
                            {
                                await editAssessmentConnection.UpdateAsync(assessment);

                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("ERROR", "The Start Date must be before the End Date", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("ERROR",
                                "There is an empty field, check all fields before submitting.",
                                "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("ERROR", "An Assessment Type must be selected.", "OK");
                    }
                }
                else
                {

                    
                        await DisplayAlert("ERROR",
                            "An Objective Assessment already exists. Please delete or edit an existing assessment.", "OK");
                    
                    
                }
            }
            else
            {
             
                    await DisplayAlert("ERROR",
                        "A Performance Assessment already exists. Please delete or edit an existing assessment.", "OK");

            }
        }

        

        private void OASwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            if (oASwitch.IsToggled)
            {
                pASwitch.IsEnabled = false;
                _assessmentType = "OA";
            }

            if (oASwitch.IsToggled == false)
            {
                pASwitch.IsEnabled = true;
            }
        }

        private void PASwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            if (pASwitch.IsToggled)
            {
                oASwitch.IsEnabled = false;
                _assessmentType = "PA";
            }

            if (pASwitch.IsToggled == false)
            {
                oASwitch.IsEnabled = true;
            }
        }
    }
}