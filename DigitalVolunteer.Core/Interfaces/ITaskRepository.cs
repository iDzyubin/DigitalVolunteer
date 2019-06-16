﻿using System;
using DigitalVolunteer.Core.DataModels;

namespace DigitalVolunteer.Core.Interfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
        void Add( Task item, Guid ownerId );
    }
}