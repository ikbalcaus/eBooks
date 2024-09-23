-- Create the database
CREATE DATABASE eBooks;
GO

USE eBooks;
GO

-- Table for roles
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) -- 'user', 'publisher', 'admin'
);
GO

-- Table for users
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    UserName NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE,
    PasswordHash NVARCHAR(255),
    RoleId INT FOREIGN KEY REFERENCES Roles(RoleId),
    RegistrationDate DATETIME DEFAULT GETDATE()
);
GO

-- Table for genres
CREATE TABLE Genres (
    GenreId INT PRIMARY KEY IDENTITY(1,1),
    GenreName NVARCHAR(100)
);
GO

-- Table for books
CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255),
    Description NVARCHAR(MAX),
    GenreId INT FOREIGN KEY REFERENCES Genres(GenreId),
    Price DECIMAL(10,2),
    TotalPages INT,
    PDFPath NVARCHAR(255),
    PublisherId INT FOREIGN KEY REFERENCES Users(UserId),
    AddedDate DATETIME DEFAULT GETDATE()
);
GO

-- Table for authTokens
CREATE TABLE AuthTokens (
    AuthTokenId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Token NVARCHAR(255),
    LoggedInDate DATETIME DEFAULT GETDATE(),
    ExpirationDate DATETIME
);
GO

-- New table for storing multiple images for a book
CREATE TABLE BookImages (
    ImageId INT PRIMARY KEY IDENTITY(1,1),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    ImagePath NVARCHAR(255), -- Path to the image file
    AddedDate DATETIME DEFAULT GETDATE()
);
GO

-- Table for purchases
CREATE TABLE Purchases (
    PurchaseId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    PurchaseDate DATETIME DEFAULT GETDATE(),
    TotalPrice DECIMAL(10,2)
);
GO

-- Table for reviews
CREATE TABLE Reviews (
    ReviewId INT PRIMARY KEY IDENTITY(1,1),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(MAX),
    ReviewDate DATETIME DEFAULT GETDATE()
);
GO

-- Table for wishlists
CREATE TABLE Wishlists (
    WishlistId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    AddedDate DATETIME DEFAULT GETDATE()
);
GO

-- Table for cart
CREATE TABLE Cart (
    CartId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    AddedDate DATETIME DEFAULT GETDATE()
);
GO

-- Table for authors
CREATE TABLE Authors (
    AuthorId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Biography NVARCHAR(MAX)
);
GO

-- Table for book authors (many-to-many relationship)
CREATE TABLE BookAuthors (
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    AuthorId INT FOREIGN KEY REFERENCES Authors(AuthorId),
    PRIMARY KEY (BookId, AuthorId)
);
GO

-- Table for access rights (age restrictions)
CREATE TABLE AccessRights (
    AccessRightId INT PRIMARY KEY IDENTITY(1,1),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    MinimumAge INT
);
GO

-- Table for reading progress
CREATE TABLE ReadingProgress (
    ProgressId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    CurrentPage INT,
    LastReadDate DATETIME DEFAULT GETDATE()
);
GO

-- Table for favorites
CREATE TABLE Favorites (
    FavoriteId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    AddedDate DATETIME DEFAULT GETDATE()
);
GO

-- Table for publisher verification
CREATE TABLE PublisherVerification (
    VerificationId INT PRIMARY KEY IDENTITY(1,1),
    PublisherId INT FOREIGN KEY REFERENCES Users(UserId),
    Verified BIT DEFAULT 0,
    VerificationDate DATETIME,
    AdminId INT FOREIGN KEY REFERENCES Users(UserId)
);
GO

-- Table for following publishers
CREATE TABLE PublisherFollows (
    FollowId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    PublisherId INT FOREIGN KEY REFERENCES Users(UserId), -- Publisher is also a user
    FollowDate DATETIME DEFAULT GETDATE()
);
GO

-- Table for following books
CREATE TABLE BookFollows (
    FollowId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    FollowDate DATETIME DEFAULT GETDATE()
);
GO
