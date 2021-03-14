namespace Domain.ValueObjects
{
    public class TodoListName
    {
        public string Name { get; }
        private TodoListName(string name) => Name = name;

        public static TodoListName Create(string name) =>
            new TodoListName(name);
    }
}