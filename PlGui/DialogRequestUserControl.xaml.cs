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
    /// Interaction logic for DialogRequestUserControl.xaml
    /// </summary>
    public partial class DialogRequestUserControl : UserControl
    {
        static IBl bl = BlFactory.GetBL();
        private GuestRequestBO request;

        public DialogRequestUserControl(GuestRequestBO guestRequest)
        {
            InitializeComponent();
            request = guestRequest;
            UserControlGrid.DataContext = request;
            comboArea.SelectedIndex = (int)request.Area;
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void comboArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            request.Area = (AreaLocationBO)comboArea.SelectedIndex;
        }
    }
}
