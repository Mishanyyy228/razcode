using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using taskLibrary;

namespace lab.Repository
{
    public interface ITodoRepository
    {
        public  Task<List<Todo>?> GetTodosAsync();

        public Task<string> NewTodos(Todo todo);

        public Task<string> DeleteTodos(Todo todo, ListBox Task_List);

        public Task<string> TodosIsReady(Todo todo, ListBox Task_List);

    }
}
