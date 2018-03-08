using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECABaseModel
{
    public sealed class CEC : EntityCollection
    {
        public Guid ID { get; private set; }
        internal CEC()
        {
            ID = Guid.NewGuid();
        }

        public static CEC Instance = new CEC();
    }
}
