using System;
using SilafLibraryManagementSystem.Data;
using SilafLibraryManagementSystem.Models;
using SilafLibraryManagementSystem.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilafLibraryManagementSystem.Repositories.Implementations

    {
        public class BorrowRecordRepository : IBorrowRecordRepository
        {
            private readonly string filePath = "borrowRecords.json";
            private List<BorrowRecord> records;

            public BorrowRecordRepository()
            {
                records = FileContext.LoadFromFile<BorrowRecord>(filePath);
            }

            public void Add(BorrowRecord record)
            {
                records.Add(record);
                SaveChanges();
            }

            public void Update(BorrowRecord record)
            {
                var index = records.FindIndex(r => r.Id == record.Id);
                if (index != -1)
                {
                    records[index] = record;
                    SaveChanges();
                }
            }

            public BorrowRecord GetById(string id)
            {
                return records.FirstOrDefault(r => r.Id == id);
            }

            public IEnumerable<BorrowRecord> GetAll()
            {
                return records;
            }

            public IEnumerable<BorrowRecord> GetByMemberId(string memberId)
            {
                return records.Where(r => r.MemberId == memberId);
            }

            public IEnumerable<BorrowRecord> GetByBookId(string bookId)
            {
                return records.Where(r => r.BookId == bookId);
            }

            public BorrowRecord GetActiveBorrow(string bookId)
            {
                return records.FirstOrDefault(r => r.BookId == bookId && r.ReturnDate == null);
            }

            public void SaveChanges()
            {
                FileContext.SaveToFile(filePath, records);
            }
        }
    }
