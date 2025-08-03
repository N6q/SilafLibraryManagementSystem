using SilafLibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilafLibraryManagementSystem.Repositories.Interfaces
{
    public interface IBookRepository
    {
        void Add(Book book);
        void Update(Book book);
        void Delete(string id);
        Book GetById(string id);
        IEnumerable<Book> GetAll();
        IEnumerable<Book> SearchByTitle(string title);
        void SaveChanges();
    }
}
