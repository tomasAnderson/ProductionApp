using FluentNHibernate.Mapping;
using ProductionApp.NHibernate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.NHibernate.MappingData
{
    public class SubMap : ClassMap<Sub>
    {
        public SubMap() 
        {
            Id(x => x.ID).Column("id");
            Map(x => x.Name).Column("name");
            Map(x => x.Employee).Column("employee");
            Table("subdivision");
        }
    }
}
