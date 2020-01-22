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
    /// Interaction logic for ClientWindowUserControl.xaml
    /// </summary>
    public partial class ClientWindowUserControl : UserControl
    {
        ClientBO Client;
        public ObservableCollection<GuestRequestBO> requests;
        public ClientWindowUserControl(ClientBO clientBO)
        {
            InitializeComponent();
            Client = clientBO;
        }
    }
}
