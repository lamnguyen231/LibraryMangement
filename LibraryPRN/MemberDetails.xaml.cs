using LibraryPRN.Models;
using LibraryPRN.Utility;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LibraryPRN
{
    /// <summary>
    /// Interaction logic for MemberDetails.xaml
    /// </summary>
    public partial class MemberDetails : Window
    {
        private readonly LibraryManagementSystemContext _context;
        private Member? _member;
        public MemberDetails(LibraryManagementSystemContext context, bool isAdmin, Member? member = null)
        {
            InitializeComponent();
            _context = context;
            _member = member;
            if (isAdmin == false) RolePanel.Visibility = Visibility.Collapsed;

            if (_member != null)
            {
                MemberId.Text = _member.MemberId.ToString();
                MemberName.Text = _member.Name;
                MemberJoinDate.SelectedDate = DateTime.Parse(_member.JoinDate.ToString()); ;
                MemberEmail.Text = _member.Email;
                
                RoleAdmin.IsChecked = _member.RoleId;
                RoleUser.IsChecked = !_member.RoleId;
            }
        }

        private void BtnMemberSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateMemberInput(out string error))
            {
                MessageBox.Show(error);
                return;
            }

            if (_member == null)
            {
                _member = new Member()
                {
                    Name = MemberName.Text,
                    JoinDate = DateOnly.FromDateTime(MemberJoinDate.SelectedDate.Value),
                    Email = MemberEmail.Text,
                    Passwords = Hashing.HashPassword(MemberPassword.Password),
                    RoleId = RoleAdmin.IsChecked == true
                };
                _context.Members.Add(_member);
            }

            else
            {
                _member.Name = MemberName.Text;
                _member.Email = MemberEmail.Text;
                _member.JoinDate = DateOnly.FromDateTime(MemberJoinDate.SelectedDate.Value);
                if (MemberPassword.Password.IsNullOrEmpty()
                && MemberConfirmPassword.Password.IsNullOrEmpty())
                {
                    _member.Passwords = _member.Passwords;
                }
                else _member.Passwords = Hashing.HashPassword(MemberPassword.Password);
                _member.RoleId = RoleAdmin.IsChecked == true;               
            }

            _context.SaveChanges();
            this.Close();
        }

        private bool ValidateMemberInput(out string error)
        {
            error = "";

            if (string.IsNullOrEmpty(MemberName.Text))
            {
                error = "Name must not be empty";
                return false;
            }

            MemberName.Text = char.ToUpper(MemberName.Text[0]) + MemberName.Text.Substring(1);

            if (MemberJoinDate.SelectedDate == null)
            {
                error = "Must select a date";
                return false;
            }

            if (_member != null)
            {
                if (Hashing.HashPassword(MemberPassword.Password) == _member.Passwords)
                {
                    error = "Same password as before";
                    return false;
                }
            }

            if (MemberPassword.Equals(MemberConfirmPassword))
            {
                error = "Must have same password";
                return false;
            }

            try { _ = new System.Net.Mail.MailAddress(MemberEmail.Text); }
            catch { error = "Invalid email format"; }

            return true;
        }

        private void MemberRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RoleAdmin_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RoleUser_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
