using SilafLibraryManagementSystem.Models;
using SilafLibraryManagementSystem.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilafLibraryManagementSystem.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository bookRepo;
        private readonly IMemberRepository memberRepo;
        private readonly IBorrowRecordRepository borrowRepo;

        public LibraryService(
            IBookRepository bookRepo,
            IMemberRepository memberRepo,
            IBorrowRecordRepository borrowRepo)
        {
            this.bookRepo = bookRepo;
            this.memberRepo = memberRepo;
            this.borrowRepo = borrowRepo;
        }

        public void AddBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Id))
            {
                Console.WriteLine("❌ Book ID is required.");
                return;
            }

            if (bookRepo.GetById(book.Id) != null)
            {
                Console.WriteLine("⚠️ Book with this ID already exists.");
                return;
            }

            bookRepo.Add(book);
            Console.WriteLine("✅ Book added.");
        }


        public void RegisterMember(Member member)
        {
            if (string.IsNullOrWhiteSpace(member.Id))
            {
                Console.WriteLine("❌ National ID is required.");
                return;
            }

            if (memberRepo.GetById(member.Id) != null)
            {
                Console.WriteLine("⚠️ Member with this ID already exists.");
                return;
            }

            memberRepo.Add(member);
            Console.WriteLine("✅ Member registered.");
        }


        public void BorrowBook(string bookId, string memberId)
        {
            var book = bookRepo.GetById(bookId);
            var member = memberRepo.GetById(memberId);

            if (book == null || member == null)
            {
                Console.WriteLine("❌ Book or member not found.");
                return;
            }

            if (!book.IsAvailable)
            {
                Console.WriteLine("⚠️ Book is not available.");
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
            Console.WriteLine("📚 Book borrowed.");
        }

        public void ReturnBook(string bookId, string memberId)
        {
            var book = bookRepo.GetById(bookId);
            var member = memberRepo.GetById(memberId);

            if (book == null || member == null)
            {
                Console.WriteLine("❌ Book or member not found.");
                return;
            }

            var borrow = borrowRepo.GetActiveBorrow(bookId);

            if (borrow == null || borrow.MemberId != memberId)
            {
                Console.WriteLine("⚠️ No active borrow found for this book by this member.");
                return;
            }

            borrow.ReturnDate = DateTime.Now;
            borrowRepo.Update(borrow);

            book.IsAvailable = true;
            bookRepo.Update(book);

            Console.WriteLine("✅ Book returned.");
        }
        public void ListAllBooks()
        {
            var books = bookRepo.GetAll();
            if (!books.Any())
            {
                Console.WriteLine("📭 No books found.");
                return;
            }

            Console.WriteLine("\n📚 All Books:");
            foreach (var b in books)
            {
                string status = b.IsAvailable ? "Available" : "Borrowed";
                Console.WriteLine($"ID: {b.Id}, Title: {b.Title}, Author: {b.Author}, Status: {status}");
            }
        }


    }
}