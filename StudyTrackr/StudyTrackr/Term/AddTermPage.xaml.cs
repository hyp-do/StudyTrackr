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
    public partial class AddTermPage : ContentPage
    {
        private SQLiteAsyncConnection _addTermConnection;
        private readonly User currentUser = Globals.CurrentUser;

        public AddTermPage()
        {
            InitializeComponent();

            _addTermConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        private async void TermSave_OnClicked(object sender, EventArgs e)
        {

            Term term = new Term()
            {
                TermTitle = termTitle.Text,
                UserId = currentUser.UserId,
                TermStartDate = termBeginDate.Date,
                TermEndDate = termEndDate.Date
            };

            await _addTermConnection.CreateTableAsync<Term>();


            if (Validation.IsFieldNull(termTitle.Text))
            {
                if (termBeginDate.Date < termEndDate.Date)
                {
                    await _addTermConnection.InsertAsync(term);

                    await Navigation.PopAsync();

                }
                else
                {
                    await DisplayAlert("ERROR", "The Start Date must be before the End Date", "OK");
                }
            }
            else
            {
                await DisplayAlert("ERROR", "There is an empty field, check all fields before submitting.", "OK");
            }

        }
    }
}