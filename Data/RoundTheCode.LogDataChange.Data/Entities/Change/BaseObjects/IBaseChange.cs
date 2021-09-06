using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Entities.Change.BaseObjects
{
    public partial interface IBaseChange<TChangeEntity>
        where TChangeEntity : class, IChangeEntity<TChangeEntity>, new()
    {
        int Id { get; set; }

        int ReferenceId { get; set; }

        ChangeData ChangeData { get; set; }

        DateTimeOffset Created { get; set; }
    }
}
