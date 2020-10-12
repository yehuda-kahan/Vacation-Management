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
using System.Text.RegularExpressions;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControlSingUp : UserControl
    {
        static IBl bl = BlFactory.GetBL();
        PersonBO person = new PersonBO();
        internal event Action<string> OpenClientWin;


        public UserControlSingUp()
        {
            InitializeComponent();
            // Chose ID as a default
            IDType.SelectedIndex = 0; 
        }

        private void UserMail_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (Email.Text == "")
                ErorrMail.Visibility = Visibility.Collapsed;
            else if (!bl.IsValidMail(Email.Text) && Email.Text != "")
            {
                ErorrMail.Visibility = Visibility.Visible;
                Email.BorderBrush = Brushes.Red;
                CreateBut.IsEnabled = false;
            }
            else if (bl.IsValidMail(Email.Text))
            {
                ErorrMail.Visibility = Visibility.Collapsed;
                Email.BorderBrush = Brushes.Gray;
                CreateBut.IsEnabled = true;
            }
        }

        private void CreateBut_Click(object sender, RoutedEventArgs e)
        {
            // Make sure all details are filled out
            if (FirstName.Text == "" || LastName.Text == "" || Email.Text == ""
                || Phone.Text == "" || Id.Text == "" || Password.Password == "")
            {
                MessageBox.Show("נא למלא את כל הפרטים הנדרשים", "ERROR",MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            person.Id = Id.Text;
            person.IdType = (IdTypesBO)IDType.SelectedIndex;
            person.FirstName = FirstName.Text;
            person.LastName = LastName.Text;
            person.Email = Email.Text;
            if (CheckForMailInSys(Email.Text))
            {
                MessageBox.Show("המייל נמצא בשימוש ע''י משתמש אחר");
                return;
            }
            person.Password = Password.Password;
            person.PhoneNomber = Phone.Text;
            try
            {
                bl.AddPerson(person);
                OpenClientWin(person.Id);
            }
            catch (DuplicateKeyException ex) { MessageBox.Show(ex.Message); }
            catch (InvalidOperationException ex) { MessageBox.Show(ex.Message); }//TODO

        }

        private void Id_PreviewKeyUp(object sender, KeyEventArgs e)
        {
           
            if (Id.Text.Length == 9)
            {
                if (!bl.IsValidTZ(Id.Text))
                {
                    ErorrID.Visibility = Visibility.Visible;
                    Id.BorderBrush = Brushes.Red;
                    CreateBut.IsEnabled = false;
                }
                else
                {
                    ErorrID.Visibility = Visibility.Collapsed;
                    Id.BorderBrush = Brushes.Gray;
                    CreateBut.IsEnabled = true;
                }
            }
            else
            {
                ErorrID.Visibility = Visibility.Visible;
                Id.BorderBrush = Brushes.Red;
                CreateBut.IsEnabled = false;
            }
        }
        public bool CheckForMailInSys(string mail)
        {
            try
            {
                bl.GetPersonByMail(Email.Text);
                return true;
            }
            catch (MissingMemberException ex)
            {
                return false;
            }
        }

        private void Phone_PreviewKeyUp(object sender, KeyEventArgs e)
        {
           
            var regex = @"^(?<countryCode>[\+][1-9]{1}[0-9]{0,2}\s)?(?<areaCode>0?[1-9]\d{0,4})(?<number>\s[1-9][\d]{5,12})(?<extension>\sx\d{0,4})?$";

            if (Regex.IsMatch(Phone.Text, regex))
            {
                Phone.BorderBrush = Brushes.Gray;
                CreateBut.IsEnabled = true;
            }
            else
            {
                Phone.BorderBrush = Brushes.Red;
                CreateBut.IsEnabled = false;
            }
        }        
    }
}
