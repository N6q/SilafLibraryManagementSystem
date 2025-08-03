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
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔════════════════════════════════════╗");
                Console.WriteLine("║     📚 Library Management Menu     ║");
                Console.WriteLine("╠════════════════════════════════════╣");
                Console.WriteLine("║ 1. Add Book                        ║");
                Console.WriteLine("║ 2. Register Member                 ║");
                Console.WriteLine("║ 3. Borrow Book                     ║");
                Console.WriteLine("║ 4. Return Book                     ║");
                Console.WriteLine("║ 5. List All Books                  ║");
                Console.WriteLine("║ 6. List All Members                ║");
                Console.WriteLine("║ 7. Search Book by Title            ║");
                Console.WriteLine("║ 8. View Borrow Records             ║");
                Console.WriteLine("║ 9. Show Currently Borrowed Books   ║");
                Console.WriteLine("║ 0. Exit                            ║");
                Console.WriteLine("╚════════════════════════════════════╝");
                Console.ResetColor();
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("📘 Add New Book");
                        libraryService.AddBook(new Book());
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.WriteLine("👤 Register New Member");
                        Console.Write("Member National ID: ");
                        var nationalId = Console.ReadLine();
                        Console.Write("Name: ");
                        var name = Console.ReadLine();
                        libraryService.RegisterMember(new Member { Id = nationalId, Name = name });
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.WriteLine("📖 Borrow Book");
                        Console.Write("Book ID: ");
                        string borrowBookId = Console.ReadLine();
                        Console.Write("Member National ID: ");
                        string memId = Console.ReadLine();
                        libraryService.BorrowBook(borrowBookId, memId);
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.WriteLine("📚 Return Book");
                        Console.Write("Book ID: ");
                        var returnBookId = Console.ReadLine();
                        Console.Write("Member ID: ");
                        var returnMemId = Console.ReadLine();
                        libraryService.ReturnBook(returnBookId, returnMemId);
                        Console.ReadKey();
                        break;

                    case "5":
                        Console.WriteLine("📚 List All Books");
                        libraryService.ListAllBooks();
                        Console.ReadKey();
                        break;

                    case "6":
                        Console.WriteLine("👥 List All Members");
                        libraryService.ListAllMembers();
                        Console.ReadKey();
                        break;

                    case "7":
                        Console.WriteLine("🔍 Search Book by Title");
                        Console.Write("Enter book title to search: ");
                        string titleSearch = Console.ReadLine();
                        libraryService.SearchBookByTitle(titleSearch);
                        Console.ReadKey();
                        break;

                    case "8":
                        Console.WriteLine("📜 Borrow History");
                        libraryService.ViewBorrowRecords();
                        Console.ReadKey();
                        break;

                    case "9":
                        Console.WriteLine("🚨 Currently Borrowed Books");
                        libraryService.ShowUnreturnedBooks();
                        Console.ReadKey();
                        break;

                    case "0":
                        Console.WriteLine("👋 Exiting system... Goodbye!");
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Invalid choice. Press any key to try again.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
