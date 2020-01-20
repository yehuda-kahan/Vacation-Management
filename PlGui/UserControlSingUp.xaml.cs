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
using System.Data.Linq;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControlSingUp : UserControl
    {
        static IBl bl = BlFactory.GetBL();
        PersonBO person = new PersonBO();
        public delegate void OpenClientWin(string id);
        public UserControlSingUp()
        {
            InitializeComponent();


        }

        private void UserMail_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (Email.Text == "")
                ErorrMail.Visibility = Visibility.Collapsed;
            if (!bl.IsValidMail(Email.Text) && Email.Text != "")
                ErorrMail.Visibility = Visibility.Visible;
            if (bl.IsValidMail(Email.Text))
                ErorrMail.Visibility = Visibility.Hidden;
        }

        private void CreateBut_Click(object sender, RoutedEventArgs e)
        {
            person.Id = Id.Text;
            person.IdType = (IdTypesBO)IDType.SelectedIndex;
            person.FirstName = FirstName.Text;
            person.LastName = LastName.Text;
            person.Email = Email.Text;
            person.Password = Password.Password;
            person.PhoneNomber = Phone.Text;
            try { bl.AddPerson(person); }
            catch (DuplicateKeyException ex) { MessageBox.Show("here1"); }
            catch (InvalidOperationException ex) { MessageBox.Show("here2"); }
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
            //OpenClientWin temp = new OpenClientWin(this.Parent.);

        }
    }
}
