using System.Collections.Generic;

namespace WebApi.Auth.Scopes
{
    public static class Scopes
    {
        public const string ReadTodoListsScope = "read:todo-lists";
        public const string CreateTodoListsScope = "create:todo-lists";
        public const string ManageTodoListsScope = "manage:todo-lists";


        public static readonly List<string> All = new()
            {ReadTodoListsScope, CreateTodoListsScope, ManageTodoListsScope};
    }
}