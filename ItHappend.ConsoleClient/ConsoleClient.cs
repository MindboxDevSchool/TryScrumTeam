using System;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Infrastructure;

namespace ItHappend.ConsoleClient
{
    public class ConsoleClient
    {
        private IUserService _userService;

        public ConsoleClient()
        {
            var userRepository = new UserRepositoryInMemory();
            _userService = new UserService(userRepository);
        }

        private Tuple<string, string> ReadLoginAndPassword()
        {
            Console.Write("Введите логин:");
            var login = Console.ReadLine();
            Console.Write("Введите пароль:");
            var password = Console.ReadLine();
            return new Tuple<string, string>(login, password);
        }

        private void WorkWithTracks(AuthData authData)
        {
            
        }
        
        private void WorkWithUser(AuthData authData)
        {
            Console.WriteLine("Welcome to User");
            bool IsRunning = true;
            while (IsRunning)
            {
                Console.WriteLine("Выберите действие из списка:");
                Console.WriteLine("1 - открыть меню работы с треками");
                Console.WriteLine("2 - выйти из аккаунта");

                try
                {
                    var userCase = Convert.ToInt32(Console.ReadLine());
                    switch (userCase)
                    {
                        case 1:
                            WorkWithTracks(authData);
                            break;
                        case 2:
                            IsRunning = false;
                            break;
                        default:
                            Console.WriteLine("Введено несуществующее действие!");
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("Упс, что-то пошло не так");
                }
            }
        }

        public void Start()
        {
            Console.WriteLine("Welcome to ItHappend");
            bool IsRunning = true;
            while (IsRunning)
            {
                Console.WriteLine("Выберите действие из списка:");
                Console.WriteLine("1 - зарегистироваться");
                Console.WriteLine("2 - войти в аккаунт");
                Console.WriteLine("3 - выйти из программы");

                try
                {
                    var userCase = Convert.ToInt32(Console.ReadLine());
                    switch (userCase)
                    {
                        case 1:
                            var loginAndPassword = ReadLoginAndPassword();
                            var result = _userService.CreateUser(loginAndPassword.Item1, loginAndPassword.Item2);
                            if (result.IsSuccessful())
                                WorkWithUser(result.Value);
                            else
                                Console.WriteLine(result.Exception.Message);
                            break;
                        case 2:
                            loginAndPassword = ReadLoginAndPassword();
                            result = _userService.LoginUser(loginAndPassword.Item1, loginAndPassword.Item2);
                            if (result.IsSuccessful())
                                WorkWithUser(result.Value);
                            else
                                Console.WriteLine(result.Exception.Message);
                            break;
                        case 3:
                            IsRunning = false;
                            break;
                        default:
                            Console.WriteLine("Введено несуществующее действие!");
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("Упс, что-то пошло не так");
                }
            }
        }
    }
}