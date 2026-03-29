using System;

namespace AdministratieProgramma
{
    [Serializable]
    public class UserClass
    {
        private string _name;
        private int _age;
        private string _email;

        public UserClass(string name, int age, string email)
        {
            _name = name;
            _age = age;
            _email = email;
        }

        public string Name { get => _name; set => _name = value; }
        public int Age { get => _age; set => _age = value; }
        public string Email { get => _email; set => _email = value; }

        public void ShowInfo()
        {
            Console.WriteLine($"- Name: {_name}, Age: {_age}, Email: {_email}");
        }
    }
}