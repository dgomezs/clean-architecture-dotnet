namespace CleanArchitecture.TodoList.WebApi.Tests
{
    public static class ControllerTestingConstants
    {
        public static readonly string TodoListPath = "/todo-lists";
        public static readonly string UsersPath = "/users";
        public static readonly string OwnerHeader = "OwnerId";
        public static string TodoListsSearchByName = $"{TodoListPath}/search/by-name";
    }
}