using SilafLibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilafLibraryManagementSystem.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        void Add(Member member);
        void Update(Member member);
        void Delete(string id);
        Member GetById(string id);
        IEnumerable<Member> GetAll();
        IEnumerable<Member> SearchByName(string name);
        void SaveChanges();
    }
}