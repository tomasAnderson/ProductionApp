using FluentNHibernate.Mapping;
using ProductionApp.NHibernate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.NHibernate.MappingData
{
    class EmpMap : ClassMap<Emp>
    {
        public EmpMap() 
        {
            Id(x => x.ID).Column("id");
            Map(x => x.LastName).Column("lastName");
            Map(x => x.Name).Column("name");
            Map(x => x.SecondName).Column("secondName");
            Map(x => x.BirthDate).Column("birthDate");
            Map(x => x.Genders).Column("gender");
            Map(x => x.Subdivision).Column("subdivision");
            Table("employee");
        }
    }
}
