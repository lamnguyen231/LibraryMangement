USE [LibraryManagementSystem]
GO

DROP TABLE IF EXISTS [dbo].[Returns];
DROP TABLE IF EXISTS [dbo].[Checkouts];
DROP TABLE IF EXISTS [dbo].[Books];
DROP TABLE IF EXISTS [dbo].[Authors];
DROP TABLE IF EXISTS [dbo].[Members];
GO

CREATE TABLE [dbo].[Authors](
	[Author_ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] VARCHAR(255),
    [Birth_Year] INT,
    [Nationality] VARCHAR(100)
);

CREATE TABLE [dbo].[Books](
	[Book_ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] VARCHAR(255),
    [Author_ID] INT,
    [Genre] VARCHAR(100),
    [Publication_Year] INT,
    [ISBN] VARCHAR(13),
    FOREIGN KEY (Author_ID) REFERENCES Authors(Author_ID)
);

CREATE TABLE [dbo].[Members](
	[Member_ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] VARCHAR(255),
    [Join_Date] DATE,
    [Email] VARCHAR(255),
    [RoleID] BIT,
    [Passwords] NVARCHAR(255)
);

CREATE TABLE [dbo].[Checkouts](
	[Checkout_ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Member_ID] INT,
    [Book_ID] INT,
    [Checkout_Date] DATE,
    [Due_Date] DATE,
    FOREIGN KEY (Member_ID) REFERENCES Members(Member_ID),
    FOREIGN KEY (Book_ID) REFERENCES Books(Book_ID)
);

CREATE TABLE [dbo].[Returns](
	[Return_ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Checkout_ID] INT,
    [Return_Date] DATE,
    FOREIGN KEY (Checkout_ID) REFERENCES Checkouts(Checkout_ID)
);

-- Insert into Authors
INSERT INTO Authors (Name, Birth_Year, Nationality) VALUES
('F. Scott Fitzgerald', 1896, 'American'),
('George Orwell', 1903, 'British'),
('Harper Lee', 1926, 'American'),
('J.D. Salinger', 1919, 'American'),
('Herman Melville', 1819, 'American'),
('Jane Austen', 1775, 'British'),
('J.R.R. Tolkien', 1892, 'British'),
('Leo Tolstoy', 1828, 'Russian'),
('Homer', 800, 'Greek'),
('Fyodor Dostoevsky', 1821, 'Russian'),
('Dante Alighieri', 1265, 'Italian'),
('Victor Hugo', 1802, 'French'),
('Miguel de Cervantes', 1547, 'Spanish'),
('Emily Brontë', 1818, 'British'),
('Oscar Wilde', 1854, 'Irish');
GO

-- Insert into Members (including RoleID and Passwords)
INSERT INTO Members (Name, Join_Date, Email, RoleID, Passwords) VALUES
('Admin User', '2025-07-18', 'admin@example.com', 1, '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918'), //admin
('Alice Johnson', '2023-01-15', 'alice.johnson@example.com', 0, '13dc8554575637802eec3c0117f41591a990e1a2d37160018c48c9125063838a'), //alicepassword
('Bob Smith', '2022-06-10', 'bob.smith@example.com', 0, 'da7655b5bf67039c3e76a99d8e6fb6969370bbc0fa440cae699cf1a3e2f1e0a1'),
('Charlie Brown', '2021-09-30', 'charlie.brown@example.com', 0, 'ce44586fec739a984eebda6a96cf897a4f9908a6f0812bdd9bbc55315203d833'),
('David Williams', '2022-03-20', 'david.williams@example.com', 0, '3f7c7801a5b04b65c25e672222d08ff03742adf249caa4b10187daceb5c2d856'),
('Eve Carter', '2023-05-25', 'eve.carter@example.com', 0, '6ff063e2be77308b0c4782900838183d527fce72cd0b8fe48bb752b4848028f2'),
('Frank Moore', '2021-08-12', 'frank.moore@example.com', 0, '5320c12c41444a6c8ff42288f7b0265ca3efaed18dd1545b45666db55d632a2a'),
('Grace Lee', '2022-11-10', 'grace.lee@example.com', 0, 'f688efc24c2c7799478078fdc0f6d874467799cbd65bcbb8e2ada33b45a49cf7'),
('Hannah Davis', '2023-04-18', 'hannah.davis@example.com', 0, 'ba48fcd45c0c70424ab6300cbf0ccad8417072e68d2850802667278f7a6806cd'),
('Ivy Harris', '2021-02-25', 'ivy.harris@example.com', 0, 'bc54ebcd4c46d6b2ed5a043289418123218b5af7ff38868678914052e3a058f9'),
('Jackie Kim', '2023-06-03', 'jackie.kim@example.com', 0, '619c1d01b0c829f0b233f9ccb2bf0445da18761aa3ee3df06305cd611edae391'),
('Liam Turner', '2022-09-08', 'liam.turner@example.com', 0, 'dcfca7ed3941004490c77f13de31bf8921639985960c58e627c4a16c6c8ad22d'),
('Mona Scott', '2021-11-22', 'mona.scott@example.com', 0, 'c5e061c8d932d4d322d82613e0a89644db53e830fdf822936e169ac1f4def9f6'),
('Nathan Green', '2022-07-19', 'nathan.green@example.com', 0, '8b68c062ded06197b6c7fcdf39a4c0d141f3645f6e61db8c851c17fd0edc31cf'),
('Olivia Taylor', '2023-02-14', 'olivia.taylor@example.com', 0, '203d53614c56261b89b2ef5e086df12f1d5208f903f0b6099813d1def7e46518'),
('Paul Walker', '2021-12-05', 'paul.walker@example.com', 0, '9423a2f7f90dfbb4ce03e90ae6eadf66808605f759139413a27a7468d45a015d');

GO

-- Insert into Books
INSERT INTO Books (Title, Author_ID, Genre, Publication_Year, ISBN) VALUES
('The Great Gatsby', 1, 'Fiction', 1925, '9780743273565'),
('1984', 2, 'Dystopian', 1949, '9780451524935'),
('To Kill a Mockingbird', 3, 'Fiction', 1960, '9780061120084'),
('The Catcher in the Rye', 4, 'Fiction', 1951, '9780316769488'),
('Moby Dick', 5, 'Adventure', 1851, '9781853260087'),
('Pride and Prejudice', 6, 'Romance', 1813, '9781503290563'),
('The Hobbit', 7, 'Fantasy', 1937, '9780345339683'),
('War and Peace', 8, 'Historical Fiction', 1869, '9780307388803'),
('The Odyssey', 9, 'Epic', 8, '9780140268867'),
('Crime and Punishment', 10, 'Philosophical Fiction', 1866, '9780140449136'),
('Brave New World', 2, 'Dystopian', 1932, '9780060850524'),
('The Divine Comedy', 11, 'Poetry', 1320, '9780140448955'),
('Les Misérables', 12, 'Historical Fiction', 1862, '9780451419439'),
('Don Quixote', 13, 'Novel', 1605, '9780060934347'),
('The Brothers Karamazov', 10, 'Philosophical Fiction', 1880, '9780140449242'),
('Wuthering Heights', 14, 'Romance', 1847, '9780141439556'),
('Jane Eyre', 14, 'Gothic Fiction', 1847, '9780141441146'),
('The Iliad', 9, 'Epic', 750, '9780140275360'),
('Anna Karenina', 8, 'Realist Fiction', 1877, '9780140449174'),
('The Picture of Dorian Gray', 15, 'Philosophical Fiction', 1890, '9780141439570');
GO

-- Insert into Checkouts
-- Note: Member_IDs shifted by 1 because of Admin member inserted at position 1
INSERT INTO Checkouts (Member_ID, Book_ID, Checkout_Date, Due_Date) VALUES
(2, 1, '2023-01-20', '2023-02-20'),
(3, 2, '2022-06-15', '2022-07-15'),
(4, 3, '2021-09-10', '2021-10-10'),
(5, 4, '2022-03-22', '2022-04-22'),
(6, 5, '2023-05-30', '2023-06-30'),
(7, 6, '2021-08-01', '2021-08-31'),
(8, 7, '2022-11-01', '2022-12-01'),
(9, 8, '2023-04-10', '2023-05-10'),
(10, 9, '2021-02-28', '2021-03-28'),
(11, 10, '2023-06-07', '2023-07-07'),
(12, 11, '2022-09-12', '2022-10-12'),
(13, 12, '2021-11-30', '2021-12-30'),
(14, 13, '2022-07-22', '2022-08-22'),
(15, 14, '2023-02-18', '2023-03-18'),
(16, 15, '2021-12-08', '2022-01-08');
GO

-- Insert into Returns
INSERT INTO Returns (Checkout_ID, Return_Date) VALUES
(1, '2023-02-15'),
(2, '2022-07-10'),
(3, '2021-10-05'),
(4, '2022-04-01'),
(5, '2023-06-10'),
(6, '2021-08-25'),
(7, '2022-11-15'),
(8, '2023-05-05'),
(9, '2021-03-05'),
(10, '2023-06-25'),
(11, '2022-10-08'),
(12, '2021-12-20'),
(13, '2022-08-02'),
(14, '2023-03-05'),
(15, '2022-01-05');
GO
