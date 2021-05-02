namespace CleanArchitecture.TodoList.WebApi.Tests
{
    public static class ControllerTestingConstants
    {
        public static readonly string TodoListPath = "/todo-lists";
        public static readonly string UsersPath = "/users";
        public static string TodoListsSearchByName = $"{TodoListPath}/search/by-name";
        public static readonly string AuthenticationPath = "/auth";
    }
}