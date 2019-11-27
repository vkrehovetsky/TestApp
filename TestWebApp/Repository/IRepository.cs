using System.Collections.Generic;

namespace TestWebApp.Models
{
    public interface IRepository
    {
        IEnumerable<Assignment> GetAll();
        Assignment Get(int id);
        Assignment Add(Assignment user);
        Assignment Update(Assignment user);
        Assignment SetPriority(int id, int priority);
        Assignment Up(int id);
        Assignment Down(int id);
        Assignment Delete(int id);
    }
}
