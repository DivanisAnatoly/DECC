using MaterialDesignThemes.Wpf;
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
using DECC.Windows.Schedule;
using DECC.Services;

namespace DECC.Windows.Menu
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new();

        public Menu()
        {
            InitializeComponent();

            menuItems.Children.Add(new Views.MenuItem("Schedule","Расписание", new Schedule.Schedule()));
            menuItems.Children.Add(new Views.MenuItem("Teacher","Преподаватели", new Teachers.Teachers()));
            if(!IdentityService.IsStudent()) menuItems.Children.Add(new Views.MenuItem("Person","Обучающиеся", new Students.Students()));
            menuItems.Children.Add(new Views.MenuItem("People","Группы", new Groups.Groups()));
            menuItems.Children.Add(new Views.MenuItem("Book","Дисциплины", new Disciplines.Disciplines()));
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

        private void LogOut(object sender, RoutedEventArgs e)
        {
            Login.Login login = new();
            login.Show();
            this.Close();
        }
    }
}
