using System;
using System.Collections.Generic;
using DigitalVolunteer.Core.DataModels;

namespace DigitalVolunteer.Core.Interfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
        void Add( Task item, Guid ownerId );

        List<Task> GetMyTasks( Guid userId, TaskSelectorMode selectorMode );
    }
}