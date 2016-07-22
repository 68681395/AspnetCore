using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestConsole
{
    public interface ITodoRepository
    {
        IEnumerable<TodoItem> AllItems { get; }
        void Add(TodoItem item);
        TodoItem GetById(int id);
        bool TryDelete(int id);
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TodoRepository : ITodoRepository
    {
        readonly List<TodoItem> _items = new List<TodoItem>();

        public IEnumerable<TodoItem> AllItems
        {
            get { return _items; }
        }

        public TodoItem GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public void Add(TodoItem item)
        {
            item.Id = 1 + _items.Max(x => (int?)x.Id) ?? 0;
            _items.Add(item);
        }

        public bool TryDelete(int id)
        {
            var item = GetById(id);

            if (item == null) { return false; }

            _items.Remove(item);

            return true;
        }
    }
}
