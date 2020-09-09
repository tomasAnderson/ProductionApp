using Prism.Mvvm;
using ProductionApp.NHibernate;
using ProductionApp.NHibernate.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.Classes
{
    //Подразделения
    public class Subdivision : BindableBase
    {
        readonly ObservableCollection<Subdivision> subdivisions = new ObservableCollection<Subdivision>();
        public ReadOnlyObservableCollection<Subdivision> PublicSubdivisions;

        public Subdivision()
        {
            PublicSubdivisions = new ReadOnlyObservableCollection<Subdivision>(subdivisions);
        }

        #region public properties
        int id;
        public int ID {
            get
            {
                return id;
            }
            set
            {
                id = value;
                RaisePropertyChanged("ID");
            }
        }
        string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        Employee employee;
        public Employee Employee
        {
            get
            {
                return employee;
            }
            set
            {
                employee = value;
                RaisePropertyChanged("Employee");
            }
        }
        #endregion

        #region public methods
        public void RemoveValue(int index)
        {
            //проверка на валидность удаления из коллекции - обязанность модели
            if (index >= 0 && index < subdivisions.Count)
            {
                subdivisions.RemoveAt(index);
                RaisePropertyChanged("Name"); RaisePropertyChanged("Employee");
            }
        }
        public void RemoveValueInDB(int index, int id)
        {
            //проверка на валидность удаления из коллекции - обязанность модели
            if (index >= 0 && index < subdivisions.Count)
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var sub = session.QueryOver<Sub>().List();
                        var emp = session.QueryOver<Emp>().List();

                        foreach (var s in sub)
                        {
                            if (s.ID == id)
                                session.Delete(s);
                        }

                        foreach (var e in emp)
                        {
                            if (e.Subdivision == id)
                            {
                                e.Subdivision = 0;
                                session.Update(e);
                            }
                        }

                        transaction.Commit();
                    }
                }
            }
        }
        public void AddValue(int id, string name, Employee employee)
        {
            if (name.Trim() != "")
            {
                subdivisions.Add(new Subdivision { ID = id, Name = name, Employee = employee });
                RaisePropertyChanged("Name"); RaisePropertyChanged("Employee");
            }
        }
        public void AddValueToDB(int id, string name, Employee employee)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Sub sub = new Sub();
                    sub.Name = name;
                    sub.Employee = (employee == null) ? 0 : employee.ID;

                    session.Save(sub);

                    transaction.Commit();
                }
            }
        }
        public void ChangeValue(int index, int id, string name, Employee employee)
        {
            if (index >= 0 && index < subdivisions.Count)
            {
                subdivisions.RemoveAt(index);
                subdivisions.Insert(index, new Subdivision { ID = id, Name = name, Employee = employee });
                RaisePropertyChanged("Name"); RaisePropertyChanged("Employee");
            }
        }
        public void ChangeValueInDB(int index, int id, string name, Employee employee)
        {
            if (index >= 0 && index < subdivisions.Count)
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var sub = session.QueryOver<Sub>().List();

                        foreach (var s in sub)
                        {
                            if (s.ID == id)
                            {
                                s.ID = id;
                                s.Name = name;
                                s.Employee = (employee == null) ? 0 : employee.ID;

                                session.Update(s);
                            }
                        }

                        transaction.Commit();
                    }
                }
            }

        }
        #endregion
    }
}
