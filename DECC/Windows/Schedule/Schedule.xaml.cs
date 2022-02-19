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
using System.Data;
using DECC.Infrastructure;
using Npgsql;
using System.IO;
using MaterialDesignThemes.Wpf;

namespace DECC.Windows.Schedule
{
    /// <summary>
    /// Логика взаимодействия для Schedule.xaml
    /// </summary>
    public partial class Schedule : Window
    {
        private NpgsqlDataAdapter dataAdapter = null;
        private DataTable table = null;
        private DataSet dataSet = null;
        private string queriesPath = @"C:\Users\anato\source\repos\DECC\DECC\Queries\";

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new();

        public Schedule()
        {
            InitializeComponent();

            table = new DataTable();
            dataSet = new DataSet();

            string query = File.ReadAllText(queriesPath + "ShowSchedule.txt");

            DisplayQueryData(query);

            searchColums.ItemsSource = ScheduleOptions.SearchCategories;
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

            sheduleDataGrid.ItemsSource = table.DefaultView;
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


            SheduleSearchCategories category = ScheduleOptions.StringToEnum(selectedItem);
            string query = File.ReadAllText(queriesPath + "ShowSchedule.txt");
            

            switch (category)
            {
                case SheduleSearchCategories.Discipline:
                    query += $" AND \"Disciplines\".\"Name\" = '{searchText}'";
                    break;
                case SheduleSearchCategories.Group:
                    query += $"  AND \"Groups\".\"Name\" = '{searchText}'";
                    break;
                case SheduleSearchCategories.Teacher:
                    query += $"  AND (\"Users\".\"Surname\" = '{searchText}' OR \"Users\".\"Name\" = '{searchText}' OR \"Users\".\"Fathername\" = '{searchText}')";
                    break;
            }
            
            DisplayQueryData(query);
        }

        private void ResetFilters(object sender, RoutedEventArgs e)
        {
            searchInput.Clear();
            searchColums.SelectedIndex = -1;
            string query = File.ReadAllText(queriesPath + "ShowSchedule.txt");
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
