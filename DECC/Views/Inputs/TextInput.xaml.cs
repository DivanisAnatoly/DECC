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

namespace DECC.Views.Inputs
{
    /// <summary>
    /// Логика взаимодействия для TextInput.xaml
    /// </summary>
    public partial class TextInput : UserControl, ICustomInput
    {
        public string Label { get; private set; }

        public string Value { get; set; }

        public bool Required { get; private set; }


        public TextInput(string Label, bool Required)
        {
            InitializeComponent();
            DataContext = this;
            this.Label = Label;
            this.Required = Required;
        }
    }
}
