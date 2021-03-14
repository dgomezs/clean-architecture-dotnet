namespace Domain.ValueObjects
{
    public class TodoListId
    {
        private TodoListId(long id) =>
            Id = id;

        public long Id { get; }

        public static TodoListId Create(long value)
        {
            return new TodoListId(value);
        }
    }
}