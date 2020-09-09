using Prism.Mvvm;
using ProductionApp.Classes;
using ProductionApp.NHibernate;
using ProductionApp.NHibernate.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp
{
    public class Employee : BindableBase
    {
        readonly ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        public ReadOnlyObservableCollection<Employee> PublicEmployees;

        public Employee()
        {
            PublicEmployees = new ReadOnlyObservableCollection<Employee>(employees);
        }

        #region public properties
        int id;
        public virtual int ID
        {
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
        string lastName;
        public virtual string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
                RaisePropertyChanged("LastName");
            }
        }
        string name;
        public virtual string Name
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
        string secondName;
        public virtual string SecondName
        {
            get
            {
                return secondName;
            }
            set
            {
                secondName = value;
                RaisePropertyChanged("SecondName");
            }
        }
        DateTime birthDate;
        public virtual DateTime BirthDate
        {
            get
            {
                return birthDate;
            }
            set
            {
                birthDate = value;
                RaisePropertyChanged("BirthDate");
            }
        }
        Gender gender;
        public virtual Gender Genders
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
                RaisePropertyChanged("Genders");
            }
        }

        Subdivision subdivision;
        public Subdivision Subdivision
        {
            get
            {
                return subdivision;
            }
            set
            {
                subdivision = value;
                RaisePropertyChanged("Subdivision");
            }
        }
        #endregion

        #region public methods
        public void RemoveValue(int index)
        {
            if (index >= 0 && index < employees.Count)
            {
                employees.RemoveAt(index);
                RaisePropertyChanged("Name");
            }
        }
        public void RemoveValueInDB(int index, int id)
        {
            if (index >= 0 && index < employees.Count)
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var emp = session.QueryOver<Emp>().List();
                        var sub = session.QueryOver<Sub>().List();

                        foreach (var e in emp)
                        {
                            if (e.ID == id)
                                session.Delete(e);
                        }

                        foreach (var s in sub) 
                        {
                            if (s.Employee == id) 
                            {
                                s.Employee = 0;
                                session.Update(s);
                            }    
                        }

                        transaction.Commit();
                    }
                }
            }
        }
        public void AddValue(int id, string name, string lastName, string secondName, DateTime birthDate, Gender gender, Subdivision subdivision)
        {
            employees.Add(new Employee
            {
                ID = id,
                Name = name,
                LastName = lastName,
                SecondName = secondName,
                BirthDate = birthDate,
                Genders = gender,
                Subdivision = subdivision
            });

            RaisePropertyChanged("Name");
        }

        public void AddValueToDB(string name, string lastName, string secondName, DateTime birthDate, Gender gender, Subdivision subdivision)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Emp emp = new Emp();
                    emp.Name = name;
                    emp.LastName = lastName;
                    emp.SecondName = secondName;
                    emp.BirthDate = birthDate;
                    emp.Genders = gender;
                    emp.Subdivision = (subdivision == null) ? 0 : subdivision.ID;

                    session.Save(emp);

                    transaction.Commit();
                }
            }
        }
        public void ChangeValue(int index, int id, string name, string lastName, string secondName, DateTime birthDate, Gender gender, Subdivision subdivision)
        {
            if (index >= 0 && index < employees.Count)
            {
                employees.RemoveAt(index);
                employees.Insert(index, new Employee
                {
                    ID = id,
                    Name = name,
                    LastName = lastName,
                    SecondName = secondName,
                    BirthDate = birthDate,
                    Genders = gender,
                    Subdivision = subdivision
                });
                RaisePropertyChanged("Name");
            }
        }
        public void ChangeValueInDB(int index, int id, string name, string lastName, string secondName, DateTime birthDate, Gender gender, Subdivision subdivision)
        {
            if (index >= 0 && index < employees.Count)
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var emp = session.QueryOver<Emp>().List();

                        foreach (var e in emp)
                        {
                            if (e.ID == id)
                            {
                                e.ID = id;
                                e.Name = name;
                                e.LastName = lastName;
                                e.SecondName = secondName;
                                e.BirthDate = birthDate;
                                e.Genders = gender;
                                e.Subdivision = (subdivision == null) ? 0 : subdivision.ID;

                                session.Update(e);
                            }
                        }

                        transaction.Commit();
                    }
                }

                RaisePropertyChanged("Name");
            }
        }
        #endregion


    }
}
