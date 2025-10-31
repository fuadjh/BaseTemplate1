using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public abstract class BaseEntity<Tid> :IEntity<Tid>
    {
        public Tid Id { get; set; }
    }
}
