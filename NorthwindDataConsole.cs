using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using yournamespace2;
using yournamespace3;

// Presentation Layer

namespace yournamespace1
{
    public class Program
    {
        static void Main(string[] args)
        {
            string serverName = GetInput("Server Name: ");
            string databaseName = GetInput("Database Name: ");

            string connectionString = $"Server={serverName};Database={databaseName};Integrated Security=true;";

            BusinessLayer businessLayer = new BusinessLayer(connectionString);
            
            Console.WriteLine("Number of customers: " + businessLayer.GetNumberOfCustomers());
            Console.WriteLine("Customer contact names: ");
            foreach(string contactName in businessLayer.GetCustomerContactNames())
            {
                Console.WriteLine(contactName);
            }

            Console.WriteLine("Number of employees: " + businessLayer.GetNumberOfEmployees());
            Console.WriteLine("Employee names: ");
            foreach(string employeeName in businessLayer.GetEmployeeNames())
            {
                Console.WriteLine(employeeName);
            }

            Console.WriteLine("Number of orders: " + businessLayer.GetNumberOfOrders());
            Console.WriteLine("Order IDs: ");
            foreach(int orderId in businessLayer.GetOrderIds())
            {
                Console.WriteLine(orderId);
            }

            Console.ReadLine();
        }

        static string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}

// Business Logic Layer (BLL)

namespace yournamespace2
{
    public class BusinessLayer
    {
        private DataAccessLayer _dataAccessLayer;

        public BusinessLayer(string connectionString)
        {
            _dataAccessLayer = new DataAccessLayer(connectionString);
        }

        public int GetNumberOfCustomers()
        {
            return _dataAccessLayer.GetNumberOfCustomers();
        }

        public List<string> GetCustomerContactNames()
        {
            return _dataAccessLayer.GetCustomerContactNames();
        }

        public void AddCustomer(string contactName)
        {
            _dataAccessLayer.AddCustomer(contactName);
        }

        public int GetNumberOfEmployees()
        {
            return _dataAccessLayer.GetNumberOfEmployees();
        }

        public List<string> GetEmployeeNames()
        {
            return _dataAccessLayer.GetEmployeeNames();
        }

        public int GetNumberOfOrders()
        {
            return _dataAccessLayer.GetNumberOfOrders();
        }

        public List<int> GetOrderIds()
        {
            return _dataAccessLayer.GetOrderIds();
        }
    }
}

// Data Access Layer (DAL)

namespace yournamespace3
{
    public class DataAccessLayer
    {
        private string _connectionString;

        public DataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int GetNumberOfCustomers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Customers", connection);
                return (int)command.ExecuteScalar();
            }
        }

        public List<string> GetCustomerContactNames()
        {
            List<string> contactNames = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT ContactName FROM Customers", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contactNames.Add(reader["ContactName"]?.ToString());
                }
            }
            return contactNames;
        }

        public void AddCustomer(string contactName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Customers (ContactName) VALUES (@ContactName)", connection);
                command.Parameters.AddWithValue("@ContactName", contactName);
                command.ExecuteNonQuery();
            }
        }

        public int GetNumberOfEmployees()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Employees", connection);
                return (int)command.ExecuteScalar();
            }
        }

        public List<string> GetEmployeeNames()
        {
            List<string> employeeNames = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT FirstName, LastName FROM Employees", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employeeNames.Add(reader["FirstName"].ToString() + " " + reader["LastName"].ToString());
                }
            }
            return employeeNames;
        }

        public int GetNumberOfOrders()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Orders", connection);
                return (int)command.ExecuteScalar();
            }
        }

        public List<int> GetOrderIds()
        {
            List<int> orderIds = new List<int>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT OrderID FROM Orders", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orderIds.Add((int)reader["OrderID"]);
                }
            }
            return orderIds;
        }
    }
}
