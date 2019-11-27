using System.Collections.Generic;
using System.Linq;

namespace TestWebApp.Models
{
    public class Repository : IRepository
    {
        LinkedList<Assignment> _assignments = new LinkedList<Assignment>();

        public Repository()
        {
            if (!_assignments.Any())
            {
                _assignments.AddLast(new Assignment { Id = 1, Name = "Task1" });
                _assignments.AddLast(new Assignment { Id = 2, Name = "Task2" });
                _assignments.AddLast(new Assignment { Id = 3, Name = "Task3" });
                _assignments.AddLast(new Assignment { Id = 4, Name = "Task4" });
            }
        }

        public IEnumerable<Assignment> GetAll()
        {
            return _assignments;
        }

        public Assignment Get(int id)
        {
            return _assignments.FirstOrDefault(x => x.Id == id);
        }

        public Assignment Add(Assignment assignment)
        {
            _assignments.AddLast(assignment);
            return assignment;
        }

        public Assignment Update(Assignment assignment)
        {
            if (!_assignments.Any(x => x.Id == assignment.Id))
            {
                return null;
            }
            _assignments.Where(x => x.Id == assignment.Id)
                .Select(t =>
                {
                    t.Name = assignment.Name;
                    return t;
                }).ToList();
            return assignment;
        }
        public Assignment SetPriority(int id, int priority)
        {
            Assignment current = _assignments.FirstOrDefault(x => x.Id == id);
            Assignment item = _assignments.Skip(priority - 1).FirstOrDefault();

            if (current == null || item == null)
                return null;

            var withSequence = _assignments
                        .Select((a, index) => new
                        {
                            a.Id,
                            a.Name,
                            Sequence = index + 1
                        }).FirstOrDefault(x => x.Id == id);

            LinkedListNode<Assignment> newNode = _assignments.Find(item);
            _assignments.Remove(current);

            if (withSequence.Sequence > priority)
                _assignments.AddBefore(newNode, current);
            else
                _assignments.AddAfter(newNode, current);

            return current;
        }

        public Assignment Up(int id)
        {
            Assignment current = _assignments.FirstOrDefault(x => x.Id == id);

            if (current == null)
                return null;

            if (current != _assignments.FirstOrDefault())
            {
                Assignment prev = GetPrevious(_assignments, id);
                LinkedListNode<Assignment> newNode = _assignments.Find(prev);

                _assignments.Remove(current);
                _assignments.AddBefore(newNode, current);
            }
            return current;
        }

        public Assignment Down(int id)
        {
            Assignment current = _assignments.FirstOrDefault(x => x.Id == id);

            if (current == null)
                return null;

            if (current != _assignments.LastOrDefault())
            {
                Assignment next = GetNext(_assignments, id);
                LinkedListNode<Assignment> newNode = _assignments.Find(next);

                _assignments.Remove(current);
                _assignments.AddAfter(newNode, current);
            }
            return current;
        }

        public Assignment Delete(int id)
        {
            Assignment assignment = _assignments.FirstOrDefault(x => x.Id == id);

            if (assignment == null)
                return null;

            _assignments.Remove(assignment);
            return assignment;
        }

        private static Assignment GetNext(IEnumerable<Assignment> list, int id)
        {
            return list.SkipWhile(x => !x.Id.Equals(id)).Skip(1).First();
        }

        private static Assignment GetPrevious(IEnumerable<Assignment> list, int id)
        {
            return list.TakeWhile(x => !x.Id.Equals(id)).Last();
        }
    }
}
