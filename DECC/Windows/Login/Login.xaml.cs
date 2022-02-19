using DECC.Infrastructure;
using MaterialDesignThemes.Wpf;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DECC.Windows.Login
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new();

        public Login()
        {
            InitializeComponent();
        }

        private void ToggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();

            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void LoginUser(object sender, RoutedEventArgs e)
        {
            String email = txtUserEmail.Text;
            String password = txtPassword.Password;

            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Поля не должны быть пустыми!");
                return;
            }

            using var con = DBContext.GetConnection();
            con.Open();
            NpgsqlCommand command = new($"SELECT * FROM public.\"Users\" WHERE \"Email\" = '{email}' AND \"Password\" = '{password}'", con);
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                Properties.Settings.Default.Name = reader["Name"].ToString();
                Properties.Settings.Default.Surname = reader["Surname"].ToString();
                Properties.Settings.Default.Fathername = reader["Fathername"].ToString();
                Properties.Settings.Default.Id = (int)reader["Id"];
                Properties.Settings.Default.Role = (int)reader["UserRoleId"];
                Properties.Settings.Default.Save();
                Menu.Menu menu = new();
                menu.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильный пароль или почта!");
            }
            reader.Close();
            con.Close();
        }
    }
}
