using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure.Repositories
{
    public class UserRepositoryDapper:IUserRepository
    {
        private readonly IDbConnection _connection;
        
        public UserRepositoryDapper(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
        public User TryCreate(User user)
        {
            try
            {
                var a = _connection
                    .Query<User>(@"insert into ItHappend.Users
                (Id, Login, HashedPassword) 
                VALUES (@Id, @Login, @HashedPassword)",
                        new
                        {
                            Id = user.Id,
                            Login = user.Login,
                            HashedPassword = user.HashedPassword
                        });
            }
            catch
            {
                throw new RepositoryException(RepositoryExceptionType.LoginAlreadyExists,user.Login);
            }

            return user;
        }

        public User TryGetByLogin(string login)
        {
            var result = _connection
                .Query<User>(@"select * from ItHappend.Users
                                    where Login = @Login",
            new{Login = login}).ToList();
            
            if (!result.Any())
                throw new RepositoryException(RepositoryExceptionType.UserNotFoundByLogin, login);
            
            return result.Single();
        }

        public User TryGetById(Guid id)
        {
            var result = _connection
                .Query<User>(@"select * from ItHappend.Users
                                    where Id = @Id",
                    new{Id = id}).ToList();
            if (!result.Any())
            {
                throw new RepositoryException(RepositoryExceptionType.UserNotFound, id);
            }
            return result.Single();
        }
    }
}