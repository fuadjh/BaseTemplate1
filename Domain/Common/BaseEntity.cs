using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public abstract class BaseEntity<TId> : IEntity<TId>
    {
        public TId Id { get; protected set; }

        protected BaseEntity() { }

        protected BaseEntity(TId id)
        {
            Id = id;
        }
    }

}
