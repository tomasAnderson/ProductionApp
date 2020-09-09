using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.NHibernate
{
    class NHibernateHelper
    {
        public NHibernateHelper()
        {
            InitializeSessionFactory();
        }

        private static ISessionFactory sessionFactory;
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                    InitializeSessionFactory();

                return sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {
            sessionFactory = Fluently.Configure().Database(
                MySQLConfiguration.Standard.ConnectionString(
                    cs => cs.Server("localhost").Database("db").Username("root").Password("123456789"))
                ).Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateHelper>()).BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
