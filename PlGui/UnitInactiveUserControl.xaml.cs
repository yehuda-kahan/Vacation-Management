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
    /// Interaction logic for UnitInactiveUserControl.xaml
    /// </summary>
    public partial class UnitInactiveUserControl : UserControl
    {
        HostingUnitBO myUnit;
        static IBl bl = BlFactory.GetBL();
        public UnitInactiveUserControl(HostingUnitBO unit)
        {
            InitializeComponent();
            myUnit = unit;
        }

        private void ActiveUnit_Click(object sender, RoutedEventArgs e)
        {
            myUnit.Status = StatusBO.ACTIVE;
            bl.UpdUnit(myUnit);
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
