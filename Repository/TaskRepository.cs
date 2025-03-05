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
        private static List<ClassTask> Tasks = new List<ClassTask>()
        {

        };
        public static IEnumerable<ClassTask> AllTasks => Tasks;
        public static ClassTask CurrentTask { get; private set; }
        public bool AddTask(string category, string name, string description,string date)
        {
            var newTask = new ClassTask
            {
                Category = category,
                Name = name,
                Description = description,
                IsCompleted = false,
                Date = date,
            };
            Tasks.Add(newTask);
            CurrentTask = newTask;
            return true;
        }
    }
}
