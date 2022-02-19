using System;
using System.Collections;
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
    /// Логика взаимодействия для SelectInput.xaml
    /// </summary>
    public partial class SelectInput : UserControl, ICustomInput
    {
        public string Label { get; private set; }
        public string Value { get; init; }
        public IEnumerable ItemsSource { get; init; }
        public string DisplayMemberPath { get; init; }
        public string SelectedValuePath { get; init; }

        public bool Required { get; private set; }

        public SelectInput(string Label, ComboBox comboBox, bool Required)
        {
            InitializeComponent();
            DataContext = this;

            this.ItemsSource = comboBox.ItemsSource;
            this.DisplayMemberPath = comboBox.DisplayMemberPath;
            this.SelectedValuePath = comboBox.SelectedValuePath;
            this.Label = Label;
            this.Required = Required;
        }
    }
}
