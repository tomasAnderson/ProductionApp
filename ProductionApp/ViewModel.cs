using Prism.Commands;
using Prism.Mvvm;
using ProductionApp.Classes;
using ProductionApp.NHibernate;
using ProductionApp.NHibernate.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProductionApp
{
    class ViewModel : BindableBase
    {
        readonly Subdivision subdivision = new Subdivision();
        readonly Employee employee = new Employee();

        public ViewModel() {
            addDataEMP();

            AddCommandSubdivision = new DelegateCommand<List<object>>(i =>
            {
                int id = (i[0].ToString() != "") ? Convert.ToInt32(i[0]) : 1;
                string name = i[1].ToString();
                Employee employee = (i[2] != null) ? (Employee)i[2] : null;

                if (name.ToString().Trim() != "")
                {
                    subdivision.AddValueToDB(id, name, employee);
                    subdivision.AddValue(id, name, employee);
                    addDataEMP();
                }
            });
            RemoveCommandSubdivision = new DelegateCommand<List<object>>(i =>
            {
                if ((int)i[0] != -1)
                {
                    int index = (int)i[0];
                    int id = Convert.ToInt32(i[1]);

                    subdivision.RemoveValueInDB(index, id);
                    subdivision.RemoveValue(index);
                    addDataEMP();
                }
            });
            ChangeCommandSubdivision = new DelegateCommand<List<object>>(i =>
            {
                if ((int)i[0] != -1) {
                    int index = (int)i[0];
                    int id = Convert.ToInt32(i[1]);
                    string name = (i[2].ToString().Trim() != "") ? i[2].ToString() : "";
                    Employee employee = (i[3] != null) ? (Employee)i[3] : null;

                    if (name.Trim() != "")
                    {
                        subdivision.ChangeValueInDB(index, id, name, employee);
                        subdivision.ChangeValue(index, id, name, employee);
                        addDataEMP();
                    }
                }
            });



            RemoveCommandEmployee = new DelegateCommand<List<object>>(i =>
            {
                if ((int)i[0] != -1)
                {
                    int index = (int)i[0];
                    int id = Convert.ToInt32(i[1]);

                    employee.RemoveValueInDB(index, id);
                    employee.RemoveValue(index);
                    addDataEMP();
                }
            });
            ChangeCommandEmployee = new DelegateCommand<List<object>>(i =>
            {
                if ((int)i[0] != -1)
                {
                    int index = (int)i[0];
                    int id = Convert.ToInt32(i[1]);
                    string name = (i[2].ToString().Trim() != "") ? i[2].ToString() : "";
                    string lastName = (i[3].ToString().Trim() != "") ? i[3].ToString() : "";
                    string secondName = (i[4].ToString().Trim() != "") ? i[4].ToString() : "";
                    DateTime birthDate = (i[5] != null) ? (DateTime)i[5] : DateTime.MinValue;
                    Enum gender = (i[6] != null) ? (Gender)i[6] : (Enum)None.none;
                    Subdivision subdivision = (i[7] != null) ? (Subdivision)i[7] : null;

                    if (name.Trim() != "" && lastName.Trim() != "" && secondName.Trim() != "" && birthDate != DateTime.MinValue && subdivision != null)
                    {
                        employee.ChangeValueInDB(index, id, name, lastName, secondName, birthDate, (Gender)gender, subdivision);
                        employee.ChangeValue(index, id, name, lastName, secondName, birthDate, (Gender)gender, subdivision);
                        addDataEMP();
                    }
                }
            });
            AddCommandEmployee = new DelegateCommand<List<object>>(i =>
            {
                int id = (i[0].ToString() != "") ? Convert.ToInt32(i[0]) : 1;
                string name = (i[1].ToString().Trim() != "") ? i[1].ToString() : "";
                string lastName = (i[2].ToString().Trim() != "") ? i[2].ToString() : "";
                string secondName = (i[3].ToString().Trim() != "") ? i[3].ToString() : "";
                DateTime birthDate = (i[4] != null) ? (DateTime)i[4] : DateTime.MinValue;
                Enum gender = (i[5] != null) ? (Gender)i[5] : (Enum)None.none;
                Subdivision subdivision = (i[6] != null) ? (Subdivision)i[6] : null;

                if (name.Trim() != "" &&
                lastName.Trim() != "" &&
                secondName.Trim() != "" &&
                birthDate != DateTime.MinValue &&
                //Enum.IsDefined(typeof(Gender), i[4]) &&
                subdivision != null)
                {
                    employee.AddValueToDB(name, lastName, secondName, birthDate, (Gender)gender, subdivision);
                    employee.AddValue(id, name, lastName, secondName, birthDate, (Gender)gender, subdivision);
                    addDataEMP();
                }
            });
        }

        public DelegateCommand<List<object>> AddCommandSubdivision { get; }
        public DelegateCommand<List<object>> RemoveCommandSubdivision { get; }
        public DelegateCommand<List<object>> ChangeCommandSubdivision { get; }

        public DelegateCommand<List<object>> AddCommandEmployee { get; }
        public DelegateCommand<List<object>> RemoveCommandEmployee { get; }
        public DelegateCommand<List<object>> ChangeCommandEmployee { get; }

        public ReadOnlyObservableCollection<Subdivision> PublicSubdivisions { get; set; }
        public ReadOnlyObservableCollection<Employee> PublicEmployees { get; set; }

        ReadOnlyObservableCollection<Subdivision> refreshSubdivision() 
        {
            return subdivision.PublicSubdivisions;
        }

        ReadOnlyObservableCollection<Employee> refreshEmployee()
        {
            return employee.PublicEmployees;
        }

        int index;

        // создаю/выгружаю данные
        public void addDataEMP()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var sub = session.QueryOver<Sub>().List();
                    var emp = session.QueryOver<Emp>().List();

                    PublicSubdivisions = refreshSubdivision();
                    PublicEmployees = refreshEmployee();

                    if (sub.Count != 0)
                    {
                        for (int i = 0; i <= PublicSubdivisions.Count + 5; i++)
                            subdivision.RemoveValue(0);

                        foreach (var s in sub)
                            subdivision.AddValue(s.ID, s.Name, null);
                    }

                    #region Employee
                    index = -1;
                    if (emp.Count != 0)
                    {
                        for (int i = 0; i <= PublicEmployees.Count + 5; i++)
                            employee.RemoveValue(0);


                        foreach (var e in emp)
                        {
                            int n = 0;

                            while (PublicSubdivisions.Count - n != 0)
                            {
                                if (PublicSubdivisions[n].ID == e.Subdivision)
                                {
                                    index = n;
                                    break;
                                }
                                n++;
                                index = -1;
                            }

                            if(index == -1)
                                employee.AddValue(e.ID, e.Name, e.LastName, e.SecondName, e.BirthDate, e.Genders, null);
                            else
                                employee.AddValue(e.ID, e.Name, e.LastName, e.SecondName, e.BirthDate, e.Genders, PublicSubdivisions[index]);
                        }
                    }
                    #endregion

                    #region Subdivision
                    index = -1;
                    if (emp.Count != 0)
                    {
                        for (int i = 0; i <= PublicSubdivisions.Count + 5; i++)
                            subdivision.RemoveValue(0);


                        foreach (var s in sub)
                        {
                            int n = 0;

                            while (PublicEmployees.Count - n != 0)
                            {
                                if (PublicEmployees[n].ID == s.Employee)
                                {
                                    index = n;
                                    break;
                                }
                                n++;
                                index = -1;
                            }

                            if (index == -1)
                                subdivision.AddValue(s.ID, s.Name, null);
                            else
                                subdivision.AddValue(s.ID, s.Name, PublicEmployees[index]);
                        }
                    }
                    #endregion

                    #region Employee
                    index = -1;
                    if (emp.Count != 0)
                    {
                        for (int i = 0; i <= PublicEmployees.Count + 5; i++)
                            employee.RemoveValue(0);


                        foreach (var e in emp)
                        {
                            int n = 0;

                            while (PublicSubdivisions.Count - n != 0)
                            {
                                if (PublicSubdivisions[n].ID == e.Subdivision)
                                {
                                    index = n;
                                    break;
                                }
                                n++;
                                index = -1;
                            }

                            if (index == -1)
                                employee.AddValue(e.ID, e.Name, e.LastName, e.SecondName, e.BirthDate, e.Genders, null);
                            else
                                employee.AddValue(e.ID, e.Name, e.LastName, e.SecondName, e.BirthDate, e.Genders, PublicSubdivisions[index]);
                        }
                    }
                    #endregion


                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception) { }
                }
            }
        }

        public void addDataSUB()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var sub = session.QueryOver<Sub>().List();
                    var emp = session.QueryOver<Emp>().List();

                    PublicSubdivisions = refreshSubdivision();
                    PublicEmployees = refreshEmployee();

                    if (sub.Count != 0)
                    {
                        for (int i = 0; i <= PublicSubdivisions.Count + 5; i++)
                            subdivision.RemoveValue(0);

                        foreach (var s in sub)
                            subdivision.AddValue(s.ID, s.Name, null);
                    }

                    #region Employee
                    index = -1;
                    if (emp.Count != 0)
                    {
                        for (int i = 0; i <= PublicEmployees.Count + 5; i++)
                            employee.RemoveValue(0);


                        foreach (var e in emp)
                        {
                            int n = 0;

                            while (PublicSubdivisions.Count - n != 0)
                            {
                                if (PublicSubdivisions[n].ID == e.Subdivision)
                                {
                                    index = n;
                                    break;
                                }
                                n++;
                                index = -1;
                            }

                            if (index == -1)
                                employee.AddValue(e.ID, e.Name, e.LastName, e.SecondName, e.BirthDate, e.Genders, null);
                            else
                                employee.AddValue(e.ID, e.Name, e.LastName, e.SecondName, e.BirthDate, e.Genders, PublicSubdivisions[index]);
                        }
                    }
                    #endregion

                    #region Subdivision
                    index = -1;
                    if (emp.Count != 0)
                    {
                        for (int i = 0; i <= PublicSubdivisions.Count + 5; i++)
                            subdivision.RemoveValue(0);


                        foreach (var s in sub)
                        {
                            int n = 0;

                            while (PublicEmployees.Count - n != 0)
                            {
                                if (PublicEmployees[n].ID == s.Employee)
                                {
                                    index = n;
                                    break;
                                }
                                n++;
                                index = -1;
                            }

                            if (index == -1)
                                subdivision.AddValue(s.ID, s.Name, null);
                            else
                                subdivision.AddValue(s.ID, s.Name, PublicEmployees[index]);
                        }
                    }
                    #endregion


                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception) { }
                }
            }
        }


    }

    enum None {
        none
    }
}
