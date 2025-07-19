using LibraryPRN.Models;
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
using System.Windows.Shapes;
using LibraryPRN.Utility;

namespace LibraryPRN
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LibraryManagementSystemContext _context;
        private Member _member;
        public Login()
        {
            InitializeComponent();
            _context = new LibraryManagementSystemContext();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string mail = txtMemberMail.Text;
            string password = txtPassword.Password;

            if (!ValidateLogin(mail, password, out string err))
            {
                MessageBox.Show(err);
                return;
            }

            else
            {
                if (_member.RoleId)
                {
                    Admin admin = new Admin(_context);
                    admin.Show();
                }
                else
                {
                    User guest = new User(_context, _member);
                    guest.Show();
                }
                this.Close();
            }
        }
        
        public bool ValidateLogin(string mail, string password, out string err)
        {
            err = "";
            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(password))
            {
                err = "Please enter both User's Mail and Password.";
                return false;
            }

            _member = _context.Members.FirstOrDefault(m => m.Email != null && m.Email == mail);
            if (_member == null)
            {
                err = "Account don't exist";
                return false;
            }

            string hashed = Hashing.HashPassword(password);

            if (_member.Passwords != hashed)
            {
                err = "Wrong password";
                return false;
            }

            return true;
        }
    }
}
