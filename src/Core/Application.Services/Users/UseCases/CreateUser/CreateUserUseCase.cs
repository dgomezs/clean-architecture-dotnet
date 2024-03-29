﻿using System.Threading.Tasks;
using Application.Services.Users.Errors;
using Application.Services.Users.Repositories;
using Domain.Shared.Errors;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace Application.Services.Users.UseCases.CreateUser
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public CreateUserUseCase(IUserRepository userRepository) =>
            _userRepository = userRepository;

        public async Task<UserId> Invoke(CreateUserCommand createUserCommand)
        {
            var existingUser = await _userRepository.GetByEmail(createUserCommand.Email);
            if (existingUser is not null)
                throw new DomainException(new UserAlreadyExistsError(createUserCommand.Email));

            var user = new User(createUserCommand.Name, createUserCommand.Email);
            await _userRepository.Save(user);
            return user.Id;
        }
    }
}