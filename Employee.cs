using System;
using System.Collections.Generic;

namespace Question2
{
    public class Employee
    {
      


        #region class properties
        private long _salary = 0;
        private string _id, _managerId = "";

        // employee id get & setter

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        // employee manager id get & setter

        public string ManagerId
        {
            get => _managerId;
            set => _managerId = value;
        }
        // employee salary get & setter

        public long Salary
        {
            get => _salary;
            set => _salary = value;
        }

        #endregion class properties
       
        //constructor with csv data
       
        public Employee()
        {

        }
       
        public override bool Equals(object obj)
        {
            Employee emp1 = (Employee)obj;
            return (emp1.Id.ToUpper().Equals(Id.ToUpper()));
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public class EmployeeExtension
    {
        private List<Employee> employeeslist= new List<Employee>();
        public List<Employee> EmployeesList => employeeslist;

        readonly Dictionary<string, List<string>> supervisees= new Dictionary<string, List<string>>();

        public EmployeeExtension(String[] csvdata)
        {
            ExtractData(csvdata);

            foreach (var emp in employeeslist)
            {
                Insert(emp.ManagerId, emp.Id);
            }
        }

        #region class methods

        //add supervisee to list of employees under same manager
        public void Insert(string boss, string employeeId)
        {
            Insert(boss);
            Insert(employeeId);
            supervisees[boss].Add(employeeId);
        }

        //use employee id to populate supervisee list
        public void Insert(string employeeId)
        {
            //if employee already added, stop execution
            if (supervisees.ContainsKey(employeeId))
            {
                return;
            }

            supervisees.Add(employeeId, new List<string>());
        }


        //get supervisees
        public List<String> GetSupervisees(String employeeid)
        {
            return supervisees[employeeid];
        }
        public Employee CheckEmployee(string id)
        {
            foreach (Employee emp in employeeslist)
            {
                if (emp.Id.Equals(id))
                {
                    return emp;
                }
            }

            return null;
        }

        //get salary for junior employees
        public long getSalaryBudget(String leastitem)
        {
            long salary = 0;
            HashSet<String> checkeditem = new HashSet<String>();
            Stack<String> emp_stack = new Stack<String>();
            emp_stack.Push(leastitem);

            while (emp_stack.Count != 0)
            {
                String empId = emp_stack.Pop();
                if (!checkeditem.Contains(empId))
                {
                    checkeditem.Add(empId);
                    foreach (String v in GetSupervisees(empId))
                    {
                        emp_stack.Push(v);
                    }
                }
            }

            if (checkeditem.Count == 0) return salary;
            foreach (var id in checkeditem)
            {
                salary += CheckEmployee(id).Salary;
            }

            return salary;
        }
        #endregion class methods


        #region extract employees from csv data

        //Populate employee list from a csv file
        public void ExtractData(string[] csvdata)
        {

            int allceos = 0;//keep count of ceos


            foreach (var q in csvdata)
            {
                try
                {
                    //split each employee csv object
                    var employeesplit = q.Split(',');
                    long remuneration_;
                    var new_employee = new Employee();
                    new_employee.Id = employeesplit[0];

                    
                   
                    

                    if (employeesplit[1].Equals(""))
                    {
                        new_employee.ManagerId = "";
                        allceos++;                        

                        if (allceos > 1)
                        {
                            //show error there is more than one manager 
                            throw new Exception("Failed! Managers are more than one.");
                        }
                    }
                    else
                    {
                        new_employee.ManagerId = employeesplit[1];
                    }


                    var isvalid = Int64.TryParse(employeesplit[2], out remuneration_);
                    //is salary a valid number?
                    if (isvalid)
                    {
                        //check if salary is a positive number
                        if (remuneration_ > 0)
                        {
                            new_employee.Salary = remuneration_;
                        }
                        else
                        {
                            throw new Exception("Salary cannot be a negative value.");
                        }

                    }
                    else
                    {
                        throw new Exception("The value for salary is not valid.");
                    }

                    employeeslist.Add(new_employee);
                }
                catch (Exception ex)
                {
                    //clear employees
                    employeeslist.Clear();

                    Console.WriteLine(ex.Message);
                    return;
                }
               
            }
        }

        #endregion extract employees from csv data

    }
}
