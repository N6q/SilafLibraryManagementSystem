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
    public class MemberRepository : IMemberRepository
    {
        private readonly string filePath = "members.json";
        private List<Member> members;

        public MemberRepository()
        {
            members = FileContext.LoadFromFile<Member>(filePath);
        }

        public void Add(Member member)
        {
            members.Add(member);
            SaveChanges();
        }

        public void Update(Member member)
        {
            var index = members.FindIndex(m => m.Id == member.Id);
            if (index != -1)
            {
                members[index] = member;
                SaveChanges();
            }
        }

        public void Delete(string id)
        {
            members.RemoveAll(m => m.Id == id);
            SaveChanges();
        }

        public Member GetById(string id)
        {
            return members.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Member> GetAll()
        {
            return members;
        }

        public IEnumerable<Member> SearchByName(string name)
        {
            return members.Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveChanges()
        {
            FileContext.SaveToFile(filePath, members);
        }
    }
}