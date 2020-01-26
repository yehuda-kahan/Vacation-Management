using BlApi;
using BO;
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

namespace PlGui
{
    /// <summary>
    /// Interaction logic for AddUnitUserControl.xaml
    /// </summary>
    public partial class AddUnitUserControl : UserControl
    {

        HostingUnitBO myUnit;
        static IBl bl = BlFactory.GetBL();
        string hostId;
        public event Action AddUnitEv;

        public AddUnitUserControl(string Id)
        {
            InitializeComponent();
            hostId = Id;
        }

        private void AddUnit_Click(object sender, RoutedEventArgs e)
        {
            if (UnitName.Text == "") { MessageBox.Show("Must ener a name"); return; }

            else if (comArea.SelectedIndex == -1)
            {
                MessageBox.Show("Must enter an area"); return; //TODO
            }

            myUnit = new HostingUnitBO();
            myUnit.Area = (AreaLocationBO)comArea.SelectedIndex;
            myUnit.HostingUnitName = UnitName.Text;
            myUnit.Owner = hostId;
            myUnit.Diary = new bool[12, 31];
            myUnit.Key = bl.AddUnit(myUnit);
            
            AddUnitEv();

        }
    }
}