using System;
using System.Collections.Generic;
using DigitalVolunteer.Core.DomainModels;

namespace DigitalVolunteer.Core.Repositories
{
    public class MoqTaskRepository : ITaskRepository
    {
        public void Add( Task item )
        {
            throw new NotImplementedException();
        }

        public void Remove( Guid id )
        {
            throw new NotImplementedException();
        }

        public void Update( Task item )
        {
            throw new NotImplementedException();
        }

        public Task Get( Guid id ) => new Task
        {
            Id    = Guid.NewGuid(),
            Title = "Создание Web приложения",
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, "           +
                          "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                          "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris " +
                          "nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in "  +
                          "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
            EndDate             = DateTime.Now,
            StartDate           = new DateTime(2019, 6, 7),
            ContactPhone        = "89005553535",
            HasPushNotification = true,
            IsOnlyForExecutors  = false,
            TaskFormat          = TaskFormat.Freelance,
            Owner               = new Account{Id = Guid.NewGuid(), FirstName = "Tim",  LastName = "Cook",      Description = GetDescription(), Rating = {LikeCount = 100, DislikeCount = 10}},
            Executor            = new Account{Id = Guid.NewGuid(), FirstName = "John", LastName = "Appleseed", Description = GetDescription(), Rating = {LikeCount = 50,  DislikeCount = 70}}
        };

        public List<Task> GetAll()
        {
            var item = new Task
            {
                Id    = Guid.NewGuid(),
                Title = "Создание Web приложения",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, "           +
                              "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                              "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris " +
                              "nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in "  +
                              "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
                EndDate             = DateTime.Now,
                StartDate           = DateTime.Now,
                ContactPhone        = "89005553535",
                HasPushNotification = true,
                IsOnlyForExecutors  = false,
                TaskFormat          = TaskFormat.Freelance,
                Owner = new Account{Id = Guid.NewGuid(), FirstName = "Tim",  LastName = "Cook",      Description = GetDescription(), Rating = {LikeCount = 100, DislikeCount = 10}},
                Executor = new Account{Id = Guid.NewGuid(), FirstName = "John", LastName = "Appleseed", Description = GetDescription(), Rating = {LikeCount = 50,  DislikeCount = 70}}
            };
            var result = new List<Task>();
            for( int i = 0; i < 100; i++ )
            {
                result.Add(item);
            }

            return result;
        }
        
        public static string GetDescription() =>
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do "         +
            "eiusmod tempor incididunt ut labore et dolore magna aliqua. "             +
            "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi " +
            "ut aliquip ex ea commodo consequat.";
    }
}