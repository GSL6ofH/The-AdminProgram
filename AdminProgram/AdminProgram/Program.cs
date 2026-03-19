using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

namespace AdministratieProgramma
{
    [Serializable]
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
        private static string _filePath = "customers.dat";

        public static void Main(string[] args)
        {
            Load();
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
                        DeleteCustomer();
                        break;
                    case "4":
                        _isBusy = false;
                        Save();
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
            Console.WriteLine("1. Add customer");
            Console.WriteLine("2. Show all customers");
            Console.WriteLine("3. Delete user(by name)");
            Console.WriteLine("4. Shutdown");
            Console.Write("Decision: ");
        }

        private static void AddCustomer()
        {
            Console.Clear();
            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            if (!email.Contains("@gmail") && !email.Contains("@hotmail") && !email.Contains("@outlook") && !email.Contains("@live"))
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
                Save();
                Console.WriteLine("Customer succesfully added!");
            }
            else
            {
                Console.WriteLine("Error put in a correct age.");
            }
            Console.ReadKey();
            Console.Clear();

        }

        private static void ShowAll()
        {
            Console.Clear();
            if (_CustomerList.Count == 0)
            {
                Console.WriteLine("The list is still empty.");
                Console.ReadKey();
                return;
            }
            Console.Write("Do you want it to  be sorted from youngest to oldest? (y/n): ");
            string sortDecision = Console.ReadLine().ToLower();

            List<Customer> listToDisplay;
            if (sortDecision == "y")
            {
                listToDisplay = GetSortedList();
            }
            else
            {
                listToDisplay = _CustomerList;
            }

            Console.WriteLine("\n--- Filter options (leave empty to skip) ---");
            Console.Write("Enter minimum age: ");
            string ageInput = Console.ReadLine();
            int minAge = 0;
            bool filterByAge = int.TryParse(ageInput, out minAge);

            Console.Write("Enter maximum age: ");
            string maxAgeInput = Console.ReadLine();
            int maxAge = int.MaxValue;
            bool filterByMaxAge = int.TryParse(maxAgeInput, out maxAge);

            Console.Write("Enter starting letter: ");
            string letterInput = Console.ReadLine();
            bool filterByLetter = !string.IsNullOrEmpty(letterInput);

            Console.Clear();
            Console.WriteLine("--- Filtered Customer info ---");
            int foundCount = 0;
            int totalAge = 0;

            foreach (Customer customer in listToDisplay)
            {
                bool matchesAge = true;
                bool matchesLetter = true;

                if (filterByAge && customer.Age < minAge)
                {
                    matchesAge = false;
                }
                if (filterByMaxAge && customer.Age > maxAge)
                {
                    matchesAge = false;
                }
                if (filterByLetter && !customer.Name.ToLower().StartsWith(letterInput.ToLower()))
                {
                    matchesLetter = false;
                }

                if (matchesAge && matchesLetter)
                {
                    customer.ShowInfo();
                    foundCount++;
                    totalAge += customer.Age;
                }
            }
            if (foundCount == 0)
            {
                Console.WriteLine("No customers found with these filters.");
            }
            else
            {
                double average = (double)totalAge / foundCount;
                Console.WriteLine($"\nTotal found: {foundCount}");
                Console.WriteLine($"Average age of these customers: {average:F1}");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
            Console.Clear();
        }
        private static void DeleteCustomer()
        {
            Console.Clear();
            if (_CustomerList.Count == 0)
            {
                Console.WriteLine("This list is empty. nothing here to delete");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("fill in the name of the one you want to delete");
            string InputName = Console.ReadLine();

            Customer customerFound = _CustomerList.Find(c => c.Name.Equals(InputName, StringComparison.OrdinalIgnoreCase));

            if (customerFound != null)
            {
                _CustomerList.Remove(customerFound);
                Save();
                Console.WriteLine($"\nSucces: Customer'{customerFound.Name}' is deleted.");
            }
            else
            {
                Console.WriteLine($"\nError no customer exist with the name '{InputName}'.");
            }

            Console.WriteLine("\npress a key to return back to menu");
            Console.ReadKey();
            Console.Clear();
        }
        private static List<Customer> GetSortedList()
        {
            List<Customer> sorted = new List<Customer>(_CustomerList);

            for (int i = 0; i < sorted.Count - 1; i++)
            {
                for (int j = 0; j < sorted.Count - i - 1; j++)
                {
                    if (sorted[j].Age > sorted[j + 1].Age)
                    {
                        Customer temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }
            return sorted;
        }
        public static void Save()
        {
            try
            {
                BinaryFormatter binform = new BinaryFormatter();
                using (FileStream file = File.Open(_filePath, FileMode.Create))
                {
                    binform.Serialize(file, _CustomerList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Save error: " + ex.Message);
            }
        }
        public static void Load()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    BinaryFormatter binform = new BinaryFormatter();
                    using (FileStream file = File.Open(_filePath, FileMode.Open))
                    {
                        _CustomerList = (List<Customer>)binform.Deserialize(file);
                    }
                }
                catch (Exception)
                {
                    _CustomerList = new List<Customer>();
                }
            }
        }
    }
}