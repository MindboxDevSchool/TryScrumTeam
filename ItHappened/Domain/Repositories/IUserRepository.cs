﻿using System;

namespace ItHappened.Domain.Repositories
{
    public interface IUserRepository
    {
        Result<User> TryCreate(User user);
        Result<User> TryGetByLogin(string login);
        Result<User> TryGetByToken(string token);
        Result<User> TryGetById(Guid id);
    }
}