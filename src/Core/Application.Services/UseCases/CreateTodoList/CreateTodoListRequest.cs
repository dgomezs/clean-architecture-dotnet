using System.Collections.Generic;
using Domain.Errors;
using Domain.ValueObjects;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace Application.Services.UseCases.CreateTodoList
{
    public class CreateTodoListRequest
    {
        private CreateTodoListRequest(TodoListName todoListName, TodoListName todoListName2)
        {
            TodoListName = todoListName;
            TodoListName2 = todoListName2;
        }

        public TodoListName TodoListName { get; }
        public TodoListName TodoListName2 { get; }

        public static Validation<DomainValidationException, CreateTodoListRequest> CreateWithErrors(string name1,
            string name2)
        {
            var n1 = TodoListName.CreateWithErrors(name1).MapFail(e => e.PrefixErrors("TodoListName"));
            var n2 = TodoListName.CreateWithErrors(name2).MapFail(e => e.PrefixErrors("TodoListName2"));
            return (n1 | n2)
                .Map(_ => BuildRequest(n1, n2));
        }

        public static CreateTodoListRequest Create(string name1,
            string name2)
        {
            return CreateWithErrors(name1, name2)
                .IfFail(ex => throw new DomainValidationException("ddd", "ddd", ConcatErrors(ex)));
        }

        private static CreateTodoListRequest BuildRequest(Validation<DomainValidationException, TodoListName> n1,
            Validation<DomainValidationException, TodoListName> n2)
        {
            var todoListName = n1.ToOption().ValueUnsafe();
            var todoListName1 = n2.ToOption().ValueUnsafe();
            return new CreateTodoListRequest(todoListName, todoListName1);
        }

        private static IEnumerable<DomainValidationFailure> ConcatErrors(
            Seq<DomainValidationException> domainValidationExceptions)
        {
            return domainValidationExceptions.Select(x => x.Errors).Flatten();
        }
    }
}