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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class BookDetails : Window
    {
        private readonly LibraryManagementSystemContext _context;
        private Book? _book;
        public BookDetails(LibraryManagementSystemContext context, Book? book = null)
        {
            InitializeComponent();
            _context = context;
            _book = book;
            BookGenre.ItemsSource = _context.Books.Select(p => p.Genre).Distinct().OrderBy(t => t).ToList();

            if (_book != null)
            {
                BookID.Text = _book.BookId.ToString();
                BookTitle.Text = _book.Title;
                AuthorID.Text = _book.AuthorId.ToString();
                AuthorName.Text = _book.Author?.Name;
                AuthorBirthYear.Text = _book.Author?.BirthYear.ToString();
                AuthorNationality.Text = _book.Author?.Nationality;
                BookGenre.Text = _book.Genre;
                BookPublicationYear.Text = _book.PublicationYear.ToString();
                BookISBN.Text = _book.Isbn;
            }
        }
        private void BtnBookSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateBookInput(out string error))
            {
                MessageBox.Show(error);
                return;
            }

            if (_book == null)
            {
                var author = new Author
                {
                    Name = AuthorName.Text,
                    BirthYear = int.Parse(AuthorBirthYear.Text),
                    Nationality = AuthorNationality.Text,
                };

                _book = new Book
                {
                    Title = BookTitle.Text,
                    Author = author,
                    Genre = BookGenre.Text,
                    PublicationYear = int.Parse(BookPublicationYear.Text),
                    Isbn = BookISBN.Text,
                };

                _context.Books.Add(_book);
                _context.SaveChanges();
            }

            else
            {
                var author = _context.Authors.Find(int.Parse(AuthorID.Text));

                _book.Title = BookTitle.Text;
                _book.Author = author;
                _book.Genre = BookGenre.Text;
                _book.PublicationYear = int.Parse(BookPublicationYear.Text);
                _book.Isbn = BookISBN.Text;

                _context.SaveChanges();
            }

            this.Close();
        }

        private bool ValidateBookInput(out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(BookTitle.Text))
            {

                error = "Title must not be empty.";
                return false;
            }

            BookTitle.Text = char.ToUpper(BookTitle.Text[0]) + BookTitle.Text.Substring(1);

            if (string.IsNullOrWhiteSpace(AuthorName.Text))
            {
                error = "Authors must not be empty.";
                return false;
            }

            AuthorName.Text = char.ToUpper(AuthorName.Text[0]) + AuthorName.Text.Substring(1);

            if (string.IsNullOrWhiteSpace(AuthorBirthYear.Text))
            {
                error = "Birth Year must not be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(AuthorNationality.Text))
            {
                error = "Nationality must not be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(BookGenre.Text))
            {
                error = "Genre must be selected.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(BookPublicationYear.Text))
            {
                error = "Publication Year must not be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(BookISBN.Text))
            {
                error = "ISBN must not be empty.";
                return false;
            }

            return true;
        }

        private void BookGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
