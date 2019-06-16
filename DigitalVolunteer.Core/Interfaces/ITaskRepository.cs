using System;
using System.Collections.Generic;
using DigitalVolunteer.Core.DataModels;

namespace DigitalVolunteer.Core.Interfaces
{
    public interface ITaskRepository : IRepository<DigitalTask>
    {
        List<DigitalTask> GetUserTasks( Guid userId );
        List<DigitalTask> GetUserTasks( Guid userId, Func<DigitalTask, bool> filter );
    }
}
