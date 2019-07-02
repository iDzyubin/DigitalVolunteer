using System.ComponentModel.DataAnnotations;

namespace DigitalVolunteer.Core.DataModels
{
    public enum UserStatus
    {
        Unknown = 0,
        Unconfirmed = 1,
        Confirmed = 2,
        Deleted = 3
    }

    public enum DigitalTaskStatus
    {
        Open = 0,
        Completed = 1,
        Canceled = 2
    }

    public enum DigitalTaskFormat
    {
        [Display( Name = "Удаленная работа" )]
        Freelance = 0,
        [Display( Name = "Присутствие на месте" )]
        UpWork = 1
    }

    public enum TaskSelectorMode
    {
        [Display( Name = "Все задания" )]
        All = 0,
        [Display( Name = "Я исполнитель" )]
        Executor = 1,
        [Display( Name = "Я заказчик" )]
        Owner = 2
    }


    /// <summary>
    /// Состояние задачи.
    /// Является основным жизненным циклом.
    /// </summary>
    public enum DigitalTaskState
    {
        /// <summary>
        /// Задача создана.
        /// Когда пользователь создал задачу и исполнителя ещё нет.
        /// </summary>
        [Display( Name = "Задание создано" )]
        Created = 0,
        /// <summary>
        /// Задача не подтверждена.
        /// Когда исполнитель решил взять задачу,
        /// но заказчик ещё не подтвердил, что согласен.
        /// </summary>
        [Display( Name = "Ожидает подтверждения" )]
        Unconfirmed = 1,
        /// Задача не подтверждена заказчиком.
        /// Когда исполнитель решил взять задачу,
        /// но заказчик ещё не подтвердил, что согласен.
        /// </summary>
        [Display( Name = "Ожидает подтверждения заказчика" )]
        WaitingOwnerConfirmation = 1,
        /// <summary>
        /// Задача подтверждена.
        /// Заказчик подтвердил исполнителя
        /// </summary>
        [Display( Name = "Задание подтверждено" )]
        Confirmed = 2,
        /// <summary>
        /// Задача выполнена.
        /// Исполнитель передал заказчику работу,
        /// заказчик подтвердил выполнение.
        /// </summary>
        [Display( Name = "Задание выполнено" )]
        Completed = 3,
        /// <summary>
        /// Задание завершено.
        /// Заказчик подтвердил, что задача выполнена успешно.
        /// </summary>
        [Display( Name = "Задание завершено" )]
        Closed = 4,
        /// <summary>
        /// Задача не подтверждена исполнителем.
        /// Когда заказчик решил предложить задачу,
        /// но исполнитель ещё не подтвердил, что согласен.
        /// </summary>
        [Display( Name = "Ожидает подтверждения исполнителя" )]
        WaitingExecutorConfirmation = 5
    }
}
