using DECC.Infrastructure;
using DECC.Services;
using MaterialDesignThemes.Wpf;
using Npgsql;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace DECC.Windows.Teachers
{
    /// <summary>
    /// Логика взаимодействия для Teachers.xaml
    /// </summary>
    public partial class Teachers : Window
    {
        private NpgsqlDataAdapter dataAdapter = null;
        private DataTable table = null;
        private DataSet dataSet = null;
        private string queriesPath = @"C:\Users\anato\source\repos\DECC\DECC\Queries\";
        private string dataQueryFile;

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new();

        public Teachers()
        {
            InitializeComponent();

            table = new DataTable();
            dataSet = new DataSet();

            if (IdentityService.IsStudent()) {
                dataQueryFile = "ShowTeachers_Student.txt";
            }
            else
            {
                dataQueryFile = "ShowTeachers.txt";
            }

            string query = File.ReadAllText(queriesPath + dataQueryFile);

            DisplayQueryData(query);

            searchColums.ItemsSource = TeachersOptions.SearchCategories;
        }

        private void DisplayQueryData(string query)
        {
            using (var con = DBContext.GetConnection())
            {
                con.Open();
                NpgsqlCommand command = new(query, con);
                dataAdapter = new NpgsqlDataAdapter(command);

                dataSet.Reset();
                dataAdapter.Fill(dataSet);
                table = dataSet.Tables[0];

                con.Close();
            }

            dataGrid.ItemsSource = table.DefaultView;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            string searchText = searchInput.Text;
            var selectedItem = searchColums?.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedItem) || string.IsNullOrEmpty(searchText)) return;


            TeacherSearchCategories category = TeachersOptions.StringToEnum(selectedItem);
            string query = File.ReadAllText(queriesPath + dataQueryFile);

            switch (category)
            {
                case TeacherSearchCategories.Teacher:
                    query += $"  AND (\"Users\".\"Surname\" = '{searchText}' OR \"Users\".\"Name\" = '{searchText}' OR \"Users\".\"Fathername\" = '{searchText}')";
                    break;
                case TeacherSearchCategories.Phone:
                    query += $"  AND \"Users\".\"Phone\" = '{searchText}'";
                    break;
                case TeacherSearchCategories.Position:
                    query += $"  AND \"Positions\".\"Name\" = '{searchText}'";
                    break;
            }

            DisplayQueryData(query);
        }

        private void ResetFilters(object sender, RoutedEventArgs e)
        {
            searchInput.Clear();
            searchColums.SelectedIndex = -1;
            string query = File.ReadAllText(queriesPath + dataQueryFile);
            DisplayQueryData(query);
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

        private void GoToMenu(object sender, RoutedEventArgs e)
        {
            Menu.Menu menu = new();
            menu.Show();
            this.Close();
        }
    }
}
