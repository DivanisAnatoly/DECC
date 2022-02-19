using DECC.Infrastructure;
using DECC.Services;
using DECC.Views.Inputs;
using DECC.Windows.Dialogs.AddRow;
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

namespace DECC.Windows.Disciplines
{
    /// <summary>
    /// Логика взаимодействия для Disciplines.xaml
    /// </summary>
    public partial class Disciplines : Window
    {
        private NpgsqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        private string queriesPath = @"C:\Users\anato\source\repos\DECC\DECC\Queries\";

        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new();

        public Disciplines()
        {
            InitializeComponent();          
        }

        public void InitData()
        {
            dataSet = new DataSet();
            string query = File.ReadAllText(queriesPath + "ShowDisciplines.txt");
            DisplayQueryData(query);
            searchColums.ItemsSource = DisciplinesOptions.SearchCategories;
            if (!IdentityService.IsAdmin()) {
                var columns = dataGrid.Columns;
                var deleteColumn = columns.FirstOrDefault(c => c.Header.ToString() == "Удалить");
                var updateColumn = columns.FirstOrDefault(c => c.Header.ToString() == "Редактировать");
                columns[columns.IndexOf(deleteColumn)].Visibility = Visibility.Hidden;
                columns[columns.IndexOf(updateColumn)].Visibility = Visibility.Hidden;
                add_row_btn.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitData();
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

                con.Close();
            }

            dataGrid.ItemsSource = dataSet.Tables[0].DefaultView;

            if (dataGrid.Columns.Count>1)
            {
                var targetColumn = dataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Id");
                var index = dataGrid.Columns.IndexOf(targetColumn);
                dataGrid.Columns[index].Visibility = Visibility.Hidden;
            }
        }

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);
        //    DragMove();
        //}

        private void Search(object sender, RoutedEventArgs e)
        {
            string searchText = searchInput.Text;
            var selectedItem = searchColums?.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedItem) || string.IsNullOrEmpty(searchText)) return;


            DisciplineSearchCategories category = DisciplinesOptions.StringToEnum(selectedItem);
            string query = File.ReadAllText(queriesPath + "ShowDisciplines.txt");

            switch (category)
            {
                case DisciplineSearchCategories.Discipline:
                    query += $"  AND \"Disciplines\".\"Name\" = '{searchText}'";
                    break;
                case DisciplineSearchCategories.LeadTeacher:
                    query += $"  AND (\"Users\".\"Surname\" = '{searchText}' OR \"Users\".\"Name\" = '{searchText}' OR \"Users\".\"Fathername\" = '{searchText}')";
                    break;
            }

            DisplayQueryData(query);
        }

        private void ResetFilters(object sender, RoutedEventArgs e)
        {
            searchInput.Clear();
            searchColums.SelectedIndex = -1;
            string query = File.ReadAllText(queriesPath + "ShowDisciplines.txt");
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

        private void DataGrid_AddRow(object sender, MouseButtonEventArgs e)
        {
            DataSet dataSet = new();

            List<ICustomInput> inputs=new();
            TextInput discipline = new("Дисциплина",true);
            ComboBox teachers = new();
            
            string query = File.ReadAllText(queriesPath + "TeachersList.txt");

            using (var con = DBContext.GetConnection())
            {
                con.Open();
                NpgsqlCommand command = new(query, con);
                dataAdapter = new NpgsqlDataAdapter(command);
                dataAdapter.Fill(dataSet);
                con.Close();
            }

            teachers.ItemsSource = dataSet.Tables[0].DefaultView;
            teachers.DisplayMemberPath = dataSet.Tables[0].Columns[1].ToString();
            teachers.SelectedValuePath = dataSet.Tables[0].Columns[0].ToString();

            SelectInput teachersSelect = new("Преподаватели",teachers,true);
            inputs.Add(discipline);
            inputs.Add(teachersSelect);

            AddRow addRow = new("Добавить данные",inputs);

            if (addRow.ShowDialog() == true)
            {
                using var con = DBContext.GetConnection();
                con.Open();
                NpgsqlCommand command = new($"INSERT INTO public.\"Disciplines\"(\"Name\",\"LeadTeacherId\") VALUES('{discipline.Value}',{teachersSelect.Value})", con);
                command.ExecuteNonQuery();
                con.Close();

                DisplayQueryData(File.ReadAllText(queriesPath + "ShowDisciplines.txt"));
                //MessageBox.Show(discipline.Value+' '+ teachersSelect.Value);
            }
        }

        private void Datagrid_RemoveRow(object sender, RoutedEventArgs e)
        {
            DataRowView obj = ((FrameworkElement)sender).DataContext as DataRowView;
            var idColumnIndex = obj.Row.Table.Columns.IndexOf("Id");
            string Id = obj.Row.ItemArray[idColumnIndex].ToString();
            
            using var con = DBContext.GetConnection();
            con.Open();
            NpgsqlCommand command = new($"DELETE FROM public.\"Disciplines\" WHERE \"Disciplines\".\"Id\" = '{Id}'", con);
            command.ExecuteNonQuery();
            con.Close();
            DisplayQueryData(File.ReadAllText(queriesPath + "ShowDisciplines.txt")); 
        }


        private void GoToMenu(object sender, RoutedEventArgs e)
        {
            Menu.Menu menu = new();
            menu.Show();
            this.Close();
        }

        private void Datagrid_UpdateRow(object sender, RoutedEventArgs e)
        {
            DataSet dataSet = new();

            DataRowView obj = ((FrameworkElement)sender).DataContext as DataRowView;
            var cells = obj.Row.Table.Columns;

            string id = obj.Row.ItemArray[cells.IndexOf("Id")].ToString();
            string updDiscipline = obj.Row.ItemArray[cells.IndexOf("Предмет")].ToString();
            string updLeadTeacher = obj.Row.ItemArray[cells.IndexOf("Ведущий преподаватель")].ToString();

            List<ICustomInput> inputs = new();
            TextInput discipline = new("Дисциплина", true);
            ComboBox teachers = new();

            string query = File.ReadAllText(queriesPath + "TeachersList.txt");

            using (var con = DBContext.GetConnection())
            {
                con.Open();
                NpgsqlCommand command = new(query, con);
                dataAdapter = new NpgsqlDataAdapter(command);
                dataAdapter.Fill(dataSet);
                con.Close();
            }

            teachers.ItemsSource = dataSet.Tables[0].DefaultView;
            teachers.DisplayMemberPath = dataSet.Tables[0].Columns[1].ToString();
            teachers.SelectedValuePath = dataSet.Tables[0].Columns[0].ToString();

            SelectInput teachersSelect = new("Преподаватели", teachers, true);

            discipline.Value = updDiscipline;


            inputs.Add(discipline);
            inputs.Add(teachersSelect);

            AddRow addRow = new("Обновить данные",inputs);
            if (addRow.ShowDialog() == true)
            {
                if (discipline.Value==updDiscipline && teachersSelect.Value == updLeadTeacher) return;
                using var con = DBContext.GetConnection();
                con.Open();
                NpgsqlCommand command = new($"UPDATE public.\"Disciplines\" SET \"Name\"='{discipline.Value}', \"LeadTeacherId\"='{teachersSelect.Value}' WHERE \"Id\"='{id}'", con);


                command.ExecuteNonQuery();
                con.Close();

                DisplayQueryData(File.ReadAllText(queriesPath + "ShowDisciplines.txt"));
                //MessageBox.Show(discipline.Value+' '+ teachersSelect.Value);
            }
        }
    }
}
