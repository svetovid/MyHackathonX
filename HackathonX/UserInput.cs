using HackathonX.DB.Model;
using HackathonX.DB.Repositories;
using System.Diagnostics;
using System.Windows;

namespace HackathonX.UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UserInput : Window
    {
        MainWindow mainWin;
        UserRepository userRepository;


        public UserInput()
        {
            InitializeComponent();
        }

        private void StartPlaying_Click(object sender, RoutedEventArgs e)
        {
            string strName = txtName.Text.Trim();
            Debug.WriteLine($"player: >>>{strName}<<<");

            mainWin = new MainWindow();
            //mainWin.CurrentUser = userRepository.GetOrAddUser(strName);

            User assumeNewUserForNow = new();
            assumeNewUserForNow.Name = strName;
            mainWin.CurrentUser = assumeNewUserForNow;
            
            mainWin.Show();
            this.Hide();
        }
    }
}
