using DECC.Views.Inputs;
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

namespace DECC.Windows.Dialogs.AddRow
{
    /// <summary>
    /// Логика взаимодействия для AddRow.xaml
    /// </summary>
    public partial class AddRow : Window
    {
        public List<ICustomInput> Inputs { get; set; }

        public string DialogTitle { get; private set; }


        public AddRow(string DialogTitle, List<ICustomInput> Inputs)
        {
            InitializeComponent();
            DataContext = this;
            this.Inputs = Inputs;
            foreach(var input in Inputs)
            {
                inputStack.Children.Add((UIElement)input);
            }
            this.DialogTitle = DialogTitle;
        }

        private void AcceptButton_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (var input in Inputs)
            {
                if (input.Required && String.IsNullOrEmpty(input.Value))
                {
                    return;
                }
            }
            DialogResult = true;
            Close();
        }
    }
}
