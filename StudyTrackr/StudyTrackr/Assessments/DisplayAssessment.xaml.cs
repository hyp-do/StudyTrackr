﻿using System;
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
    public partial class DisplayAssessment : ContentPage
    {
        private readonly Assessment _currentAssessment = Globals.CurrentAssessment;
        private readonly SQLiteAsyncConnection _deleteAssessmentConnection;
        private readonly SQLiteAsyncConnection _displayAssessmentConnection;
        public DisplayAssessment()
        {
            InitializeComponent();

            _deleteAssessmentConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
            _displayAssessmentConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await _displayAssessmentConnection.CreateTableAsync<Assessment>();

            var assessment = await _displayAssessmentConnection.Table<Assessment>()
                .Where(a => a.AssessmentId.Equals(_currentAssessment.AssessmentId))
                .FirstAsync();

            AssessmentLabel.Text = assessment.AssessmentTitle;

            if (assessment.Type == "PA")
            {
                AssessmentTypeLabel.Text = "Performance Assessment";
            }

            if (assessment.Type == "OA")
            {
                AssessmentTypeLabel.Text = "Objective Assessment";
            }

            assessmentStartDate.Text = assessment.AssessmentStartDate.ToString("MM/dd/yyyy");
            assessmentEndDate.Text = assessment.AssessmentEndDate.ToString("MM/dd/yyyy");
            assessmentNotifications.IsToggled = assessment.AssessmentNotification;

            
            base.OnAppearing();
        }

        private void ToolbarItem_OnActivated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditAssessment());
        }

        private async void AssessmentDelete_OnClicked(object sender, EventArgs e)
        {
            var deleteAssessment  = await DisplayAlert("Warning", "Do you want to delete this assessment?", "Yes", "No");
            if (deleteAssessment)
            {
                await _deleteAssessmentConnection.DeleteAsync(_currentAssessment);
                await Navigation.PopAsync();
            }
        }
    }
}