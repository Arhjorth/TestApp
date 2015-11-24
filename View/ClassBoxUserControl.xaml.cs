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

namespace TestApp.View {
    /// <summary>
    /// Interaction logic for ClassBoxUserControl.xaml
    /// </summary>
    public partial class ClassBoxUserControl : UserControl {
        public ClassBoxUserControl() {
            InitializeComponent();
        }

        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e) {
            e.Handled = true;
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e) {
            e.Handled = true;
        }
    }
}
