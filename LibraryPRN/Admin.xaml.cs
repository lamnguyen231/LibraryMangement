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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private readonly LibraryManagementSystemContext _context;
        private readonly bool _admin = true;
        public Admin(LibraryManagementSystemContext context)
        {
            InitializeComponent();
            _context = context;
            LoadProjects();
        }

        private void LoadProjects(string typeFilter = null)
        {
            BookGrid.ItemsSource = _context.Books.Include(b => b.Checkouts).ThenInclude(c => c.Returns).ToList();
            MemberGrid.ItemsSource = _context.Members.ToList().Where(m => m.RoleId == false);
            AdminGrid.ItemsSource = _context.Members.ToList().Where(m => m.RoleId == true);
            List<Author> authors = _context.Authors.ToList();
            CheckoutGrid.ItemsSource = _context.Checkouts.Include(c => c.Returns).ToList();
            BookGenreFilter.ItemsSource = _context.Books.Select(p => p.Genre).Distinct().OrderBy(t => t).ToList();
        }

        private void BookGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MemberGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LoanGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnMemberAdd_Click(object sender, RoutedEventArgs e)
        {
            MemberDetails memberDetails = new MemberDetails(_context, _admin);
            memberDetails.ShowDialog();
            LoadProjects();
        }

        private void BtnMemberEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = MemberGrid.SelectedItem ?? AdminGrid.SelectedItem;

            if (selected is not Member member)
            {
                MessageBox.Show("Please select a member to edit.");
                return;
            }

            MemberDetails memberDetails = new MemberDetails(_context, _admin, member);
            memberDetails.ShowDialog();
            LoadProjects();
        }

        private void BtnMemberDel_Click(object sender, RoutedEventArgs e)
        {
            if (MemberGrid.SelectedItem is not Member member)
            {
                MessageBox.Show("Please select a member to delete.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this member?", "Confirm", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes) return;

            if (member == null) return;

            _context.Members.Remove(member);
            _context.SaveChanges();
            LoadProjects();
        }

        private void BtnBookAdd_Click(object sender, RoutedEventArgs e)
        {
            BookDetails bookAdd = new BookDetails(_context);
            bookAdd.ShowDialog();
            LoadProjects();
        }

        private void BtnBookEdit_Click(object sender, RoutedEventArgs e)
        {
            if (BookGrid.SelectedItem is not Book book)
            {
                MessageBox.Show("Please select a book to edit.");
                return;
            }

            BookDetails bookDetails = new BookDetails(_context, book);
            bookDetails.ShowDialog();
            LoadProjects();
        }

        private void BtnBookDel_Click(object sender, RoutedEventArgs e)
        {
            if (BookGrid.SelectedItem is not Book book)
            {
                MessageBox.Show("Please select a book to edit.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this book?", "Confirm", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes) return;

            if (book == null) return;

            _context.Books.Remove(book);
            _context.SaveChanges();
            LoadProjects();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selectedTab = MainTabControl.SelectedItem as TabItem;
            if (selectedTab != null && selectedTab.Header.ToString() == "Book")
            {
                string keyword = BookSearchBox.Text.ToUpper();
                var filtered = _context.Books.AsQueryable().Where(b => !string.IsNullOrEmpty(b.Title) && b.Title.ToUpper().Contains(keyword)
                                                              || !string.IsNullOrEmpty(b.Author.Name) && b.Author.Name.ToUpper().Contains(keyword))
                                                      .ToList();
                BookGrid.ItemsSource = filtered;
            }
            else if (selectedTab != null && selectedTab.Header.ToString() == "Member")
            {
                string keyword = MemberSearchBox.Text.ToUpper();
                var filtered = _context.Members.ToList().Where(m => !string.IsNullOrEmpty(m.Name) && m.Name.ToUpper().Contains(keyword))
                                                        .ToList();
                MemberGrid.ItemsSource = filtered;
            }
        }

        private void BookGenreFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookGrid.ItemsSource = _context.Books.AsQueryable().Where(b => b.Genre == BookGenreFilter.Text).ToList();
        }

        private void BtnLoanReturn_Click(object sender, RoutedEventArgs e)
        {
            if (ReturnDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select a return date.");
                return;
            }

            if (CheckoutGrid.SelectedItem is not Checkout checkout)
            {
                MessageBox.Show("Please select a loan");
                return;
            }

            Return return2 = new Return()
            {
                CheckoutId = checkout.CheckoutId,
                ReturnDate = DateOnly.FromDateTime(ReturnDatePicker.SelectedDate.Value),
                Checkout = checkout
            };
            _context.Returns.Add(return2);
            _context.SaveChanges();
            LoadProjects();
        }

        private void BtnLoanRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadProjects();
        }

        private void BtnLoanAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new LoanAdd(_context);
            bool? result = addWindow.ShowDialog();
            LoadProjects();
        }

        private void BtnBookReset_Click(object sender, RoutedEventArgs e)
        {
            BookSearchBox.Clear();
            BookGenreFilter.SelectedIndex = -1;
            BookGrid.ItemsSource = _context.Books.ToList();
        }
    }
}
