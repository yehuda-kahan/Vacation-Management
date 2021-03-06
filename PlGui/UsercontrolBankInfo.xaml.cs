﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;
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
        string State;

        public Dictionary<int, string> BankNumberDictionary { set; get; }
        public Dictionary<int, string> BankBranchesDictionary { set; get; }

        public UserControlBankInfo(HostBO host, string state = "create")
        {
            InitializeComponent();
            State = state;
            BankNumberDictionary = bl.getBanknameList();
            Host = host;
            UserControlGrid.DataContext = Host;
            BankName.ItemsSource = BankNumberDictionary.Values;
        }

        private void CreatBut(object sender, RoutedEventArgs e)
        {
            if (State == "create")
            {
                Host.BankDetales.BankNumber = Convert.ToUInt16(BankNum.Text);
                Host.BankDetales.BranchAddress = BranchAddress.Text;
                Host.BankDetales.BranchCity = BranchCity.Text;
                //Host.CollectingClearance = ClearneceCB.IsEnabled;
                try { bl.AddHost(Host); }
                catch (DuplicateKeyException ex) { MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error); }
                MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
                OpenHostWin(Host.PersonalInfo.Id);
            }
            else if (State == "upd")
            {
                try { bl.UpdHost(Host); }
                catch (MissingMemberException ex) { MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error); }

                MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
                OpenHostWin(Host.PersonalInfo.Id);
            }          
        }

        private void BankName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            BankNum.Text = BankNumberDictionary.First(x => x.Value == BankName.SelectedItem.ToString()).Key.ToString();
            BankBranchesDictionary = bl.GetBranchesListForBank(Convert.ToInt32(BankNum.Text));
            BranchCity.Text = null;
            BranchAddress.Text = null;
            List<int> temp = BankBranchesDictionary.Keys.ToList();
            temp.Sort();
            BranchNumber.ItemsSource = temp;
            BranchNumber.SelectedIndex = 0;
        }


        private void BranchNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string address = BankBranchesDictionary.First(x => x.Key == Convert.ToInt32(BranchNumber.SelectedItem)).Value.ToString();
                BranchCity.Text = address.Substring(address.IndexOf('@') + 1);
                BranchAddress.Text = address.Substring(0, address.LastIndexOf('@'));
            }
            catch (Exception) { return; }

        }

    }
}

