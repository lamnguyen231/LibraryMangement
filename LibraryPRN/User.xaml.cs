using LibraryPRN.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for Guest.xaml
    /// </summary>
    public partial class User : Window
    {
        private readonly LibraryManagementSystemContext _context;
        private Member _member;
        private readonly bool _notAdmin = false;
        public User(LibraryManagementSystemContext context, Member member)
        {
            InitializeComponent();
            _context = context;
            _member = member;
            LoadProjects();
        }

        private void LoadProjects(string typeFilter = null)
        {
            BookGrid.ItemsSource = _context.Books.Include(b => b.Checkouts).ThenInclude(c => c.Returns).ToList().Where(b => b.Status.Equals("Available"));
            List<Author> authors = _context.Authors.ToList();
            BookGenreFilter.ItemsSource = _context.Books.Select(p => p.Genre).Distinct().OrderBy(t => t).ToList();
            CheckoutGrid.ItemsSource = _context.Checkouts.Include(c => c.Returns).ToList().Where(c => c.MemberId == _member.MemberId);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            string keyword = BookSearchBox.Text.ToUpper();
            var filtered = _context.Books.AsQueryable().Where(b => !string.IsNullOrEmpty(b.Title) && b.Title.ToUpper().Contains(keyword)
                                                              || !string.IsNullOrEmpty(b.Author.Name) && b.Author.Name.ToUpper().Contains(keyword))
                                                       .ToList();
            BookGrid.ItemsSource = filtered;
        }

        private void BookGenreFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookGrid.ItemsSource = _context.Books.AsQueryable().Where(b => b.Genre == BookGenreFilter.Text).ToList();
        }

        private void BtnBookReset_Click(object sender, RoutedEventArgs e)
        {
            BookSearchBox.Clear();
            BookGenreFilter.SelectedIndex = -1;
            BookGrid.ItemsSource = _context.Books.ToList();
        }

        private void BtnMemberEdit_Click(object sender, RoutedEventArgs e)
        {
            MemberDetails memberDetails = new MemberDetails(_context, _notAdmin, _member);
            memberDetails.ShowDialog();
            LoadProjects();
        }

        private void BookGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnLoanBook_Click(object sender, RoutedEventArgs e)
        {
            if (BookGrid.SelectedItem is not Book book)
            {
                MessageBox.Show("Please select a book for loan");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to loan this book?",
                                                      "Confirm Loan",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var checkout = new Checkout()
                {
                    MemberId = _member.MemberId,
                    BookId = book.BookId,
                    CheckoutDate = DateOnly.FromDateTime(DateTime.Now),
                    DueDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                };
                _context.Checkouts.Add(checkout);
                _context.SaveChanges();
                MessageBox.Show("Book loaned successfully.");
            }
            else
            {
                MessageBox.Show("Loan canceled.");
            }
            LoadProjects();
        }

        private void BtnLoanExtend_Click(object sender, RoutedEventArgs e)
        {
            if (CheckoutGrid.SelectedItem is not Checkout checkout)
            {
                MessageBox.Show("Please select a loan");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to extend this loan for (2 weeks)?",
                                                      "Confirm extend",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                checkout.DueDate = checkout.DueDate?.AddDays(14);
                _context.SaveChanges();
                MessageBox.Show("Loan extended successfully.");
            }
            else
            {
                MessageBox.Show("Extension failed");
            }
            LoadProjects();
        }

        private void BtnLoanReturn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckoutGrid.SelectedItem is not Checkout checkout)
            {
                MessageBox.Show("Please select a loan");
                return;
            }

            Return return2 = new Return()
            {
                CheckoutId = checkout.CheckoutId,
                ReturnDate = DateOnly.FromDateTime(DateTime.Now),
                Checkout = checkout
            };
            _context.Returns.Add(return2);
            _context.SaveChanges();
            LoadProjects();
            LoadProjects();
        }

        private void BtnLoanRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadProjects();
        }

        private void LoanGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
