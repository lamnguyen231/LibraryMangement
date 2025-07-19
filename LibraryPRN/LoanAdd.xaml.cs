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

namespace LibraryPRN
{
    /// <summary>
    /// Interaction logic for LoanDetails.xaml
    /// </summary>
    public partial class LoanAdd : Window
    {
        private readonly LibraryManagementSystemContext _context;
        public LoanAdd(LibraryManagementSystemContext context)
        {   
            InitializeComponent();
            _context = context;
            cbBook.ItemsSource = _context.Books.ToList();
            cbMember.ItemsSource = _context.Members.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbMember.SelectedValue == null || cbBook.SelectedValue == null || dpCheckoutDate.SelectedDate == null || dpDueDate.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            var checkout = new Checkout()
            {
                MemberId = (int)cbMember.SelectedValue,
                BookId = (int)cbBook.SelectedValue,
                CheckoutDate = DateOnly.FromDateTime(dpCheckoutDate.SelectedDate.Value),
                DueDate = DateOnly.FromDateTime(dpDueDate.SelectedDate.Value)
            };

            _context.Checkouts.Add(checkout);
            _context.SaveChanges();
            DialogResult = true;
            Close();
        }
    }
}
