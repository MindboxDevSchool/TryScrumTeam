using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Infrastructure;

namespace ItHappend.ConsoleClient
{
    public class ConsoleClient
    {
        private IUserService _userService;
        private ITracksService _tracksService;
        private IEventService _eventService;

        public ConsoleClient()
        {
            var userRepository = new UserRepositoryInMemory();
            var trackRepository = new TrackRepositoryInMemory();
            var eventRepository = new EventRepositoryInMemory();
            _userService = new UserService(userRepository);
            _tracksService = new TracksService(trackRepository, eventRepository);
            _eventService = new EventService(eventRepository, trackRepository);
        }

        private Tuple<string, string> ReadLoginAndPassword()
        {
            Console.Write("Введите логин:");
            var login = Console.ReadLine();
            Console.Write("Введите пароль:");
            var password = Console.ReadLine();
            return new Tuple<string, string>(login, password);
        }

        private Tuple<string, DateTime, IEnumerable<CustomizationType>> ReadTrackDto()
        {
            Console.Write("Введите название трека:");
            var name = Console.ReadLine();
            return new Tuple<string, DateTime, IEnumerable<CustomizationType>>(name, DateTime.Now, new List<CustomizationType>());
        }

        private Dictionary<int, TrackDto> PrintTrackList(UserDto userDto)
        {
            var result = _tracksService.GetTracks(userDto.Id);
            var tracks = new Dictionary<int, TrackDto>();
            if (!result.Any())
                Console.WriteLine("Список треков пока пуст");
            else
            {
                Console.WriteLine("{0,-3} {1,-20} {2,-30} {3,-10}", "№", "Name", "CreatedAt", "Customizations");
                int i = 0;
                foreach (var track in result)
                {
                    i++;
                    tracks[i] = track;
                    Console.Write("{0,-3} {1,-20} {2,-30}",
                        i,
                        track.Name,
                        track.CreatedAt.ToString("g", CultureInfo.CreateSpecificCulture("de-DE")));
                    foreach (var customizationType in track.AllowedCustomizations)
                    {
                        Console.Write(customizationType.ToString("G"), ", ");
                    }

                    Console.WriteLine();
                }
            }

            return tracks;
        }

        private Dictionary<int, EventDto> PrintEventList(UserDto userDto, TrackDto trackDto)
        {
            var result = _eventService.GetEvents(userDto.Id, trackDto.Id); 
            var events = new Dictionary<int, EventDto>();
            if (!result.Any())
                Console.WriteLine("Список событий пока пуст");
            else
            {
                Console.WriteLine("{0,-3} {1,-30} {2,-10}", "№", "CreatedAt", "Customizations");
                int i = 0;
                foreach (var @event in result)
                {
                    i++;
                    events[i] = @event;
                    Console.Write("{0,-3} {1,-20}",
                        i,
                        @event.CreatedAt.ToString("g", CultureInfo.CreateSpecificCulture("de-DE")));
                    Console.WriteLine();
                }
            }

            return events;
        }

        private void WorkWithEvents(UserDto userDto, TrackDto track)
        {
            Console.WriteLine("Welcome to Event Menu");
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Текущие события:");
                var events = PrintEventList(userDto, track);
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Выберите действие из списка:");
 
                Console.WriteLine("1 - посмотреть список событий");
                Console.WriteLine("2 - создать событие");
                //Console.WriteLine("3 - редактировать событие №");
                Console.WriteLine("4 - удалить событие №");
                Console.WriteLine("5 - выйти из меню событий");

                try
                {
                    var userCase = Convert.ToInt32(Console.ReadLine());
                    switch (userCase)
                    {
                        case 1:
                            events = PrintEventList(userDto, track);
                            break;
                        case 2:
                            _eventService.CreateEvent(userDto.Id, track.Id, DateTime.Now, new Customizations());
                            break;
                        case 4:
                            Console.Write("Введите номер событие:");
                            var num = Convert.ToInt32(Console.ReadLine());
                            _eventService.DeleteEvent(userDto.Id, events[num].Id);
                            break;
                        case 5:
                            isRunning = false;
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

        private void WorkWithTracks(UserDto userDto)
        {
            Console.WriteLine("Welcome to Track Menu");
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Текущие треки:");
                var tracks = PrintTrackList(userDto);
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Выберите действие из списка:");
 
                Console.WriteLine("1 - посмотреть список треков");
                Console.WriteLine("2 - создать трек");
                Console.WriteLine("3 - открыть трек №");
                Console.WriteLine("4 - редактировать трек №");
                Console.WriteLine("5 - удалить трек №");
                Console.WriteLine("6 - выйти из меню треков");

                try
                {
                    var userCase = Convert.ToInt32(Console.ReadLine());
                    switch (userCase)
                    {
                        case 1:
                            tracks = PrintTrackList(userDto);
                            break;
                        case 2:
                            var trackTuple = ReadTrackDto();
                            _tracksService.CreateTrack(userDto.Id, trackTuple.Item1, trackTuple.Item2, trackTuple.Item3);
                            break;
                        case 3:
                            Console.Write("Введите номер трека:");
                            var num = Convert.ToInt32(Console.ReadLine());
                            WorkWithEvents(userDto, tracks[num]);
                            break;
                        case 4:
                            Console.Write("Введите номер трека:");
                            num = Convert.ToInt32(Console.ReadLine());
                            trackTuple = ReadTrackDto();
                            var trackDto = tracks[num];
                            var editedTrackDto = new TrackDto(trackDto.Id, 
                                trackTuple.Item1, 
                                trackDto.CreatedAt, 
                                trackDto.CreatorId,
                                trackTuple.Item3);
                            _tracksService.EditTrack(userDto.Id, editedTrackDto);
                            break;
                        case 5:
                            Console.Write("Введите номер трека:");
                            num = Convert.ToInt32(Console.ReadLine());
                            _tracksService.DeleteTrack(userDto.Id, tracks[num].Id);
                            break;
                        case 6:
                            isRunning = false;
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
        
        private void WorkWithUser(UserDto userDto)
        {
            Console.WriteLine("Welcome to User");
            bool isRunning = true;
            while (isRunning)
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
                            WorkWithTracks(userDto);
                            break;
                        case 2:
                            isRunning = false;
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
            bool isRunning = true;
            while (isRunning)
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
                            isRunning = false;
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