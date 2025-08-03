using SilafLibraryManagementSystem.Models;
using SilafLibraryManagementSystem.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SilafLibraryManagementSystem.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository bookRepo;
        private readonly IMemberRepository memberRepo;
        private readonly IBorrowRecordRepository borrowRepo;

        public LibraryService(IBookRepository bookRepo, IMemberRepository memberRepo, IBorrowRecordRepository borrowRepo)
        {
            this.bookRepo = bookRepo;
            this.memberRepo = memberRepo;
            this.borrowRepo = borrowRepo;
        }

        public void AddBook(Book book)
        {
            Console.Clear();
            string bookId;

            while (true)
            {
                Console.Write("Enter Book ID: ");
                bookId = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(bookId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Book ID cannot be empty.");
                    Console.ResetColor();
                    continue;
                }

                if (bookRepo.GetById(bookId) != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("⚠️ Book with this ID already exists. Please enter a different one.");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Author: ");
            string author = Console.ReadLine();

            bookRepo.Add(new Book
            {
                Id = bookId,
                Title = title,
                Author = author,
                IsAvailable = true
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ Book added successfully.");
            Console.ResetColor();
        }

        public void RegisterMember(Member member)
        {
            Console.Clear();

            if (string.IsNullOrWhiteSpace(member.Id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ National ID is required.");
                Console.ResetColor();
                return;
            }

            if (memberRepo.GetById(member.Id) != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("⚠️ Member with this ID already exists.");
                Console.ResetColor();
                return;
            }

            memberRepo.Add(member);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ Member registered.");
            Console.ResetColor();
        }

        public void BorrowBook(string bookId, string memberId)
        {
            Console.Clear();
            if (string.IsNullOrWhiteSpace(bookId) || string.IsNullOrWhiteSpace(memberId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Book ID and Member ID are required.");
                Console.ResetColor();
                return;
            }

            var book = bookRepo.GetById(bookId);
            var member = memberRepo.GetById(memberId);

            if (book == null || member == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Book or member not found.");
                Console.ResetColor();
                return;
            }

            if (!book.IsAvailable)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("⚠️ Book is not available.");
                Console.ResetColor();
                return;
            }

            book.IsAvailable = false;
            bookRepo.Update(book);

            var borrowRecord = new BorrowRecord
            {
                Id = Guid.NewGuid().ToString(),
                BookId = book.Id,
                MemberId = member.Id,
                BorrowDate = DateTime.Now,
                ReturnDate = null
            };

            borrowRepo.Add(borrowRecord);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("📚 Book borrowed successfully.");
            Console.ResetColor();
        }

        public void ReturnBook(string bookId, string memberId)
        {
            Console.Clear();
            if (string.IsNullOrWhiteSpace(bookId) || string.IsNullOrWhiteSpace(memberId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Book ID and Member ID are required.");
                Console.ResetColor();
                return;
            }

            var book = bookRepo.GetById(bookId);
            var member = memberRepo.GetById(memberId);

            if (book == null || member == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Book or member not found.");
                Console.ResetColor();
                return;
            }

            var borrow = borrowRepo.GetActiveBorrow(bookId);

            if (borrow == null || borrow.MemberId != memberId)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("⚠️ No active borrow found for this book by this member.");
                Console.ResetColor();
                return;
            }

            borrow.ReturnDate = DateTime.Now;
            borrowRepo.Update(borrow);

            book.IsAvailable = true;
            bookRepo.Update(book);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ Book returned successfully.");
            Console.ResetColor();
        }

        public void ListAllBooks()
        {
            Console.Clear();
            var books = bookRepo.GetAll();

            if (!books.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📭 No books found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                        📚 All Books                               ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ ID         │ Title                    │ Author       │ Status     ║");
            Console.WriteLine("╟────────────┼──────────────────────────┼──────────────┼────────────╢");
            Console.ResetColor();

            foreach (var b in books)
            {
                string status = b.IsAvailable ? "Available" : "Borrowed";
                Console.WriteLine($"║ {b.Id,-10} │ {b.Title,-24} │ {b.Author,-12} │ {status,-10} ║");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void ListAllMembers()
        {
            Console.Clear();
            var members = memberRepo.GetAll();

            if (!members.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📭 No members found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║                👥 All Members                ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║ National ID           │ Name                 ║");
            Console.WriteLine("╟───────────────────────┼──────────────────────╢");
            Console.ResetColor();

            foreach (var m in members)
            {
                Console.WriteLine($"║ {m.Id,-21} │ {m.Name,-20} ║");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void SearchBookByTitle(string title)
        {
            Console.Clear();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Title cannot be empty.");
                Console.ResetColor();
                return;
            }

            var matches = bookRepo.SearchByTitle(title);
            if (!matches.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("🔍 No matching books found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                  🔎 Matching Books by Title                      ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ ID         │ Title                    │ Author       │ Status     ║");
            Console.WriteLine("╟────────────┼──────────────────────────┼──────────────┼────────────╢");
            Console.ResetColor();

            foreach (var b in matches)
            {
                string status = b.IsAvailable ? "Available" : "Borrowed";
                Console.WriteLine($"║ {b.Id,-10} │ {b.Title,-24} │ {b.Author,-12} │ {status,-10} ║");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void ViewBorrowRecords()
        {
            Console.Clear();
            var records = borrowRepo.GetAll();

            if (!records.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📭 No borrow records found.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                              📜 Borrow Records                                     ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Record ID │ Book ID │ Member ID │ Borrow Date        │ Return Date                ║");
            Console.WriteLine("╟───────────┼─────────┼───────────┼────────────────────┼────────────────────────────╢");
            Console.ResetColor();

            foreach (var r in records)
            {
                string returnStatus = r.ReturnDate.HasValue ? r.ReturnDate.Value.ToString("g") : "Not Returned";
                Console.WriteLine($"║ {r.Id[..8],-9} │ {r.BookId,-7} │ {r.MemberId,-9} │ {r.BorrowDate:g,-18} │ {returnStatus,-26} ║");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void ShowUnreturnedBooks()
        {
            Console.Clear();
            var records = borrowRepo.GetAll().Where(r => r.ReturnDate == null);

            if (!records.Any())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ All books are returned.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    🚨 Currently Borrowed Books                       ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Book ID │ Title                    │ Member ID   │ Borrow Date       ║");
            Console.WriteLine("╟─────────┼──────────────────────────┼─────────────┼───────────────────╢");
            Console.ResetColor();

            foreach (var r in records)
            {
                var book = bookRepo.GetById(r.BookId);
                Console.WriteLine($"║ {r.BookId,-8} │ {book?.Title,-24} │ {r.MemberId,-11} │ {r.BorrowDate:g,-17} ║");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }
    }
}
