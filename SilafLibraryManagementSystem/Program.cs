using SilafLibraryManagementSystem.Models;
using SilafLibraryManagementSystem.Repositories.Implementations;
using SilafLibraryManagementSystem.Services;

namespace SilafLibraryManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bookRepo = new BookRepository();
            var memberRepo = new MemberRepository();
            var borrowRepo = new BorrowRecordRepository();
            var libraryService = new LibraryService(bookRepo, memberRepo, borrowRepo);

            while (true)
            {
                Console.WriteLine("\n===== 📚 Library Menu =====");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Register Member");
                Console.WriteLine("3. Borrow Book");
                Console.WriteLine("4. Return Book");
                Console.WriteLine("5. List All Books");

                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Book ID: ");
                        var bookId = Console.ReadLine();
                        Console.Write("Title: ");
                        var title = Console.ReadLine();
                        Console.Write("Author: ");
                        var author = Console.ReadLine();
                        libraryService.AddBook(new Book { Id = bookId, Title = title, Author = author });

                        break;

                    case "2":
                        Console.Write("Member National ID: ");
                        var nationalId = Console.ReadLine();
                        Console.Write("Name: ");
                        var name = Console.ReadLine();
                        libraryService.RegisterMember(new Member { Id = nationalId, Name = name });

                        break;

                    case "3":
                        Console.Write("Book ID: ");
                        string borrowBookId = Console.ReadLine();

                        Console.Write("Member National ID: ");
                        string memId = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(borrowBookId) || string.IsNullOrWhiteSpace(memId))
                        {
                            Console.WriteLine("❌ Both Book ID and Member ID are required.");
                        }
                        else
                        {
                            libraryService.BorrowBook(borrowBookId, memId);
                        }
                        break;




                    case "4":
                        Console.Write("Book ID: ");
                        var returnBookId = Console.ReadLine();
                        Console.Write("Member ID: ");
                        var returnMemId = Console.ReadLine();
                        libraryService.ReturnBook(returnBookId, returnMemId);
                        break;

                    case "5":
                        libraryService.ListAllBooks();
                        break;


                    case "0":
                        Console.WriteLine("👋 Goodbye!");
                        return;

                    default:
                        Console.WriteLine("❌ Invalid choice.");
                        break;
                }
            }
        }
    }
}