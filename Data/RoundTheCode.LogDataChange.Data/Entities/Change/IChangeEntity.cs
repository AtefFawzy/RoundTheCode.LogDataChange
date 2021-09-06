using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Entities.Change
{
    public partial interface IChangeEntity<TEntity>
        where TEntity : class, IChangeEntity<TEntity>, new()
    {
    }
}
