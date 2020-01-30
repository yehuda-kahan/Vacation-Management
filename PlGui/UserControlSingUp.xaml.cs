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
        internal event Action<string> OpenClientWin;


        public UserControlSingUp()
        {
            InitializeComponent();
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
                ErorrID.Visibility = Visibility.Collapsed;
                Id.BorderBrush = Brushes.Gray;
                CreateBut.IsEnabled = true;
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

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!textBox.Text.All(char.IsLetter) || textBox.Text.Length == 0)
            {
                textBox.BorderBrush = Brushes.Red;
                CreateBut.IsEnabled = false;
            }
            else
            {
                textBox.BorderBrush = Brushes.Gray;
                CreateBut.IsEnabled = true;
            }
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox passBox = sender as PasswordBox;
            if (passBox.Password.Length == 0)
            {
                passBox.BorderBrush = Brushes.Red;
                CreateBut.IsEnabled = false;
            }
            else
            {
                passBox.BorderBrush = Brushes.Gray;
                CreateBut.IsEnabled = true;
            }
        }


        private void Num_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!textBox.Text.All(char.IsDigit) || textBox.Text.Length < 10)
            {
                textBox.BorderBrush = Brushes.Red;
                CreateBut.IsEnabled = false;
            }
            else
            {
                textBox.BorderBrush = Brushes.Gray;
                CreateBut.IsEnabled = true;
            }
        }
    }
}
