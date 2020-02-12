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
using BO;
using BlApi;
namespace PlGui
{
    /// <summary>
    /// Interaction logic for UpdConfig.xaml
    /// </summary>
    public partial class UpdConfig : UserControl
    {
        static IBl bl = BlFactory.GetBL();

        public UpdConfig()
        {
            InitializeComponent();
            Fee.Text = bl.GetConfigByName("OrderFee").ToString();
            DaysToExp.Text = bl.GetConfigByName("NumDaysToExpire").ToString();
            AdminPass.Text = bl.GetConfigByName("AdministratorPass").ToString();
        }

        private void Fee_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!textBox.Text.All(char.IsDigit) || textBox.Text.Length == 0)
            {
                textBox.BorderBrush = Brushes.Red;
                FeeBtn.IsEnabled = false;
            }
            else
            {
                textBox.BorderBrush = Brushes.Gray;
                FeeBtn.IsEnabled = true;
            }
        }

        private void DaysToExp_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!textBox.Text.All(char.IsDigit) || textBox.Text.Length == 0)
            {
                textBox.BorderBrush = Brushes.Red;
                DaysExpBtn.IsEnabled = false;
            }
            else
            {
                textBox.BorderBrush = Brushes.Gray;
                DaysExpBtn.IsEnabled = true;
            }

        }

        private void AdminPass_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!textBox.Text.All(char.IsDigit) || textBox.Text.Length == 0)
            {
                textBox.BorderBrush = Brushes.Red;
                AdminPassBtn.IsEnabled = false;
            }
            else
            {
                textBox.BorderBrush = Brushes.Gray;
                AdminPassBtn.IsEnabled = true;
            }
        }

        private void DaysExpBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SetConfigByName("NumDaysToExpire", Convert.ToInt32(DaysToExp.Text));
            }
            catch (AccessViolationException ex) { MessageBox.Show("העידכון לא הצליח אנא נסה שנית"); }
            catch (KeyNotFoundException ex) { MessageBox.Show("העידכון לא הצליח אנא נסה שנית"); }
        }

        private void AdninPassBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SetConfigByName("AdministratorPass", Convert.ToInt32(AdminPass.Text));
            }
            catch (AccessViolationException ex) { MessageBox.Show("העידכון לא הצליח אנא נסה שנית"); }
            catch (KeyNotFoundException ex) { MessageBox.Show("העידכון לא הצליח אנא נסה שנית"); }
        }

        private void FeeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SetConfigByName("OrderFee", Convert.ToInt32(Fee.Text));
            }
            catch (AccessViolationException ex) { MessageBox.Show("העידכון לא הצליח אנא נסה שנית"); }
            catch (KeyNotFoundException ex) { MessageBox.Show("העידכון לא הצליח אנא נסה שנית"); }
        }
    }
}
