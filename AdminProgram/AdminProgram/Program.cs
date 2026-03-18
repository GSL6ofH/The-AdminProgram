using System;
using System.Collections.Generic;

namespace AdministratieProgramma
{
    public class Customer
    {
        
        private string _name;
        private int _age;
        private string _email;


        public Customer(string name, int age, string email)
        {
            _name = name;
            _age = age;
            _email = email;
        }
        public string Name => _name;
        public int Age => _age;
        public string Email => _email;
        public void ShowInfo()
        {
            Console.WriteLine($"- Name: {_name}, Age: {_age}, Email: {_email}");
        }
    }

    public class Program
    {
        private static List<Customer> _CustomerList = new List<Customer>();
        public static void Main(string[] args)
        {
            bool _isBusy = true;

            Console.WriteLine("--- Welkom in the Administration Program ---");
            while (_isBusy)
            {
                ShowMenu();
                string decision = Console.ReadLine();

                switch (decision)
                {
                    case "1":
                        AddCustomer();
                        break;
                    case "2":
                        ShowAll();
                        break;
                    case "3":
                        _isBusy = false;
                        Console.WriteLine("Programma is shutting down...");
                        break;
                    default:
                        Console.WriteLine("invalid try again.");
                        break;
                }
            }
        }
        private static void ShowMenu()
        {
            Console.WriteLine("\nMake a decision:");
            Console.WriteLine("1. add customer");
            Console.WriteLine("2. show all customers");
            Console.WriteLine("3. shutdown");
            Console.Write("Decision: ");
        }

        private static void AddCustomer()
        {
            Console.Clear();
            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            if (!email.Contains("@gmail") && !email.Contains("@hotmail") && !email.Contains("@outlook"))
            {
                Console.WriteLine("Error: This isn't a valid email provider (Gmail/Hotmail/Outlook required).");
                Console.ReadKey(); 
                return;
            }


            Console.Write("Enter age: ");
            if (int.TryParse(Console.ReadLine(), out int age))
            {
                Customer newCustomer = new Customer(name, age, email);
                _CustomerList.Add(newCustomer);
                Console.WriteLine("Customer succesfully added!");
            }
            else
            {
                Console.WriteLine("Error put in a correct age.");
            }
            Console.Clear();

        }
        private static void ShowAll()
        {
            Console.Clear();
            Console.WriteLine("--- Filter options (leave empty to skip) ---");

            Console.Write("Enter minimum age: ");
            string ageInput = Console.ReadLine();
            int minAge = 0;
            bool filterByAge = int.TryParse(ageInput, out minAge);

            Console.Write("Enter starting letter: ");
            string letterInput = Console.ReadLine();
            bool filterByLetter = !string.IsNullOrEmpty(letterInput);

            Console.Clear();
            Console.WriteLine("\n--- Filterd Customer info ---");

            int foundCount = 0;

          foreach (Customer customer in _CustomerList)
            {
                bool matchesAge = true;
                bool matchesLetter = true;

                if (filterByAge && customer.Age < minAge)
                {
                    matchesAge = false;
                }
                if(filterByLetter && !customer.Name.ToLower().StartsWith(letterInput.ToLower()))
                {
                    matchesLetter = false;
                }
                if(matchesAge && matchesLetter)
                {
                    customer.ShowInfo();
                    foundCount++;
                }
            }
            if (foundCount == 0)
            {
                Console.WriteLine("No customers found with these filters.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
            Console.Clear();

        }
    }
}