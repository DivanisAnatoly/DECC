using DECC.Infrastructure;
using MaterialDesignThemes.Wpf;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace DECC.Windows.Groups
{
    /// <summary>
    /// Логика взаимодействия для Groups.xaml
    /// </summary>
    public partial class Groups : Window
    {
        private NpgsqlDataAdapter dataAdapter = null;
        private DataTable table = null;
        private DataSet dataSet = null;
        private string queriesPath = @"C:\Users\anato\source\repos\DECC\DECC\Queries\";

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new();

        public Groups()
        {
            InitializeComponent();

            table = new DataTable();
            dataSet = new DataSet();

            string query = File.ReadAllText(queriesPath + "ShowGroups.txt");

            DisplayQueryData(query);

            searchColums.ItemsSource = GroupsOptions.SearchCategories;
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


            GroupSearchCategories category = GroupsOptions.StringToEnum(selectedItem);
            string query = File.ReadAllText(queriesPath + "ShowGroups.txt");

            switch (category)
            {
                case GroupSearchCategories.Group:
                    query += $"  AND \"Groups\".\"Name\" = '{searchText}'";
                    break;
                case GroupSearchCategories.Curator:
                    query += $"  AND (\"Users\".\"Surname\" = '{searchText}' OR \"Users\".\"Name\" = '{searchText}' OR \"Users\".\"Fathername\" = '{searchText}')";
                    break;
                case GroupSearchCategories.Profile:
                    query += $"  AND \"Profiles\".\"Name\" = '{searchText}'";
                    break;
            }

            DisplayQueryData(query);
        }

        private void ResetFilters(object sender, RoutedEventArgs e)
        {
            searchInput.Clear();
            searchColums.SelectedIndex = -1;
            string query = File.ReadAllText(queriesPath + "ShowGroups.txt");
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
