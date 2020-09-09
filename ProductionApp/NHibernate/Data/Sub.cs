using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.NHibernate.Data
{
    public class Sub
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual int Employee { get; set; }
    }
}
