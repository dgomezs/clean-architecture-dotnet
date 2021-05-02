using System.Collections.Generic;

namespace WebApi.Authorization
{
    public static class Scopes
    {
        public static readonly string ReadTodoListsScope = "read:todo-lists";
        public static readonly string CreateTodoListsScope = "create:todo-lists";
        public static readonly string ManageTodoListsScope = "manage:todo-lists";


        public static List<string> All = new List<string>
            {ReadTodoListsScope, CreateTodoListsScope, ManageTodoListsScope};
    }
}