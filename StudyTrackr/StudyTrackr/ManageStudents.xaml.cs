using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using StudyTrackr.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyTrackr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageStudents : ContentPage
    {
        private readonly User currentUser = Globals.CurrentUser;
        private SQLiteAsyncConnection _userConnection;
        private ObservableCollection<User> _users;
        public ManageStudents()
        {
            InitializeComponent();

            _userConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await _userConnection.CreateTableAsync<User>();

            var studentsList = await _userConnection.Table<User>()
                .ToListAsync();

            _users = new ObservableCollection<User>(studentsList);

            if (studentsList.Count <= 0)
            {
                userView.ItemsSource = null;
            }
            else
            {
                userView.ItemsSource = _users;
            }
            base.OnAppearing();
        }

        private async void UserView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Globals.CurrentUser = (User) e.SelectedItem;

            await Navigation.PushAsync(new DisplayUserPage());
        }
    }
}