using SilafLibraryManagementSystem.Data;
using SilafLibraryManagementSystem.Models;
using SilafLibraryManagementSystem.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilafLibraryManagementSystem.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly string filePath = "books.json";
        private List<Book> books;

        public BookRepository()
        {
            books = FileContext.LoadFromFile<Book>(filePath);
        }

        public void Add(Book book)
        {
            books.Add(book);
            SaveChanges();
        }

        public void Update(Book book)
        {
            var index = books.FindIndex(b => b.Id == book.Id);
            if (index != -1)
            {
                books[index] = book;
                SaveChanges();
            }
        }

        public void Delete(string id)
        {
            books.RemoveAll(b => b.Id == id);
            SaveChanges();
        }

        public Book GetById(string id)
        {
            return books.FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Book> GetAll()
        {
            return books;
        }

        public IEnumerable<Book> SearchByTitle(string title)
        {
            return books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveChanges()
        {
            FileContext.SaveToFile(filePath, books);
        }
    }
}