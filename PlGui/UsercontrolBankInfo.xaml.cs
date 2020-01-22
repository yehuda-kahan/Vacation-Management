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
using BlApi;
using BO;
namespace PlGui
{

    /// <summary>
    /// Interaction logic for BankInfo.xaml
    /// </summary>
    public partial class UserControlBankInfo : UserControl
    {
        static IBl bl = BlFactory.GetBL();
        internal event Action<string> OpenHostWin;
        HostBO Host;
        public UserControlBankInfo(HostBO host)
        {
            InitializeComponent();
            Host = host;
            UserControlGrid.DataContext = host;
        }

        private void CreatBut(object sender, RoutedEventArgs e)
        {
            bl.AddHost(Host);
            MessageBox.Show(Host.BankDetales.ToString());
            OpenHostWin(Host.PersonalInfo.Id);
        }

        private void BankName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BankName.SelectedIndex == 0)
                BankNum.Text = (10).ToString();
            if (BankName.SelectedIndex == 1)
                BankNum.Text = (12).ToString();
        }
    }
}
