using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taskLibrary;

namespace lab.Repository
{
    public class TaskRepository
    {
        private static List<Todo> Tasks = new List<Todo>()
        {

        };
        public static IEnumerable<Todo> AllTasks => Tasks;
        public static Todo CurrentTask { get; private set; }
        public bool AddTask(string category, string name, string description,int date)
        {
            var newTask = new Todo
            {
                category = category,
                title = name,
                description = description,
                isCompleted = false,
                date = date,
            };
            Tasks.Add(newTask);
            CurrentTask = newTask;
            return true;
        }
    }
}
