using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DECC.Views
{
    /// <summary>
    /// Логика взаимодействия для MenuItem.xaml
    /// </summary>
    public partial class MenuItem : System.Windows.Controls.UserControl
    {
        public string Kind { get; init; }
        public string Text { get; init; }

        public Window Window { get; init; }

        public MenuItem(string Kind, string Text, Window Window)
        {
            InitializeComponent();
            DataContext = this;
            this.Kind = Kind;
            this.Text = Text;
            this.Window = Window;
        }

        private void OpenWindow(object sender, MouseButtonEventArgs e)
        {
            Window.Show();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
    }
}
