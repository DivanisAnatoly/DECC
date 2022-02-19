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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using DECC.Infrastructure;
using Npgsql;
using DECC.Windows.Schedule;
using DECC.Windows.Teachers;
using DECC.Windows.Groups;
using DECC.Windows.Disciplines;
using DECC.Windows.Students;
using DECC.Windows.Login;

namespace DECC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new();

        public MainWindow()
        {
            InitializeComponent();
            Login login = new();
            login.Show();
            this.Close();
        }
       
    }
}
