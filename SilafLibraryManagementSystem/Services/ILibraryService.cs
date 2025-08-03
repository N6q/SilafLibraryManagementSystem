using SilafLibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilafLibraryManagementSystem.Services
{
    public interface ILibraryService
    {
        void AddBook(Book book);
        void RegisterMember(Member member);
        void BorrowBook(string bookId, string memberId);
        void ReturnBook(string bookId, string memberId);
    }
}
