using System.Windows.Controls;
using System.Windows.Input;

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
