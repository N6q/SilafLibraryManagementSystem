using SilafLibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilafLibraryManagementSystem.Repositories.Interfaces
{
    public interface IBorrowRecordRepository
    {
        void Add(BorrowRecord record);
        void Update(BorrowRecord record);
        BorrowRecord GetById(string id);
        IEnumerable<BorrowRecord> GetAll();
        IEnumerable<BorrowRecord> GetByMemberId(string memberId);
        IEnumerable<BorrowRecord> GetByBookId(string bookId);
        BorrowRecord GetActiveBorrow(string bookId);
        void SaveChanges();
    }
}