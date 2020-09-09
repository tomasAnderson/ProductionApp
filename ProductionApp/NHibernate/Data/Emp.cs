using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.NHibernate.Data
{
    public class Emp
    {
        public virtual int ID { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Name { get; set; }
        public virtual string SecondName { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual Gender Genders { get; set; }
        public virtual int Subdivision { get; set; }
    }
}
