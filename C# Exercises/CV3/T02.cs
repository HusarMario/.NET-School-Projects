using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV3
{
    internal class T02
    {
        public static void Run ()
        {
            Person jano1 = new Person();
            jano1.FirstName = "Jano";
            jano1.LastName = "Mrkvicka";
            jano1.Gender = Gender.Male;

            Person jano2 = new Person { FirstName = "Jano",
                                        LastName = "Mrkvicka",
                                        Gender = Gender.Male};

            Person jano3 = new Person ("Jano", "Mrkvicka", new DateTime(1985, 12, 31), Gender.Male);
            Person jano4 = new Person("Jano", "Mrkvicka", null);

            Person jano5 = new Person
            {
                FirstName = "Ján",
                LastName = "Mrkvička",
                Gender = Gender.Male,
                Birthday = DateTime.Parse("31.12.1985", new CultureInfo("sk"))
            };
            Person jano6 = new Person("Ján", "Mrkvička", new DateTime(1985, 12, 31), Gender.Male);

            if (jano5.Equals(jano6))
            {
                Console.WriteLine("yes");
            } else
            {
                Console.WriteLine("no");
            }

            Console.WriteLine(jano3.ToString());
        }

        
    }

    internal class Person
    {
        private string _firstName;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; }
        public DateTime? Birthday { get; set; }
        public int Age { get; }
        public Gender Gender { get; set; }

        public Person()
        {

        }

        public Person(string firstName, string lastName, DateTime? dateTime, Gender gender)
        {
            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }

            FirstName = firstName;
            LastName = lastName;
            FullName = firstName + " " + lastName;
            Birthday = dateTime;
            Gender = gender;

            DateTime current = DateTime.Now;
            Age = (int) (current.Subtract ((DateTime)dateTime).TotalDays / 365);

            Console.WriteLine($"{FullName}, {Birthday:dd.MM.yyyy}, age: {Age}, gender: {Gender}");
        }

        public Person(string firstName, string lastName, DateTime? dateTime) : this(firstName, lastName, dateTime, Gender.Male)
        {

        }
      
        public override bool Equals(object? other)
        {
            if (other as Person == null) return false;
            Person person = (Person)other;

            if (!FirstName.Equals(person.FirstName)) return false;
            if (!LastName.Equals(person.LastName)) return false;
            if (!Birthday.Equals(person.Birthday)) return false;
            if (!Gender.Equals(person.Gender)) return false; return true;
        }

        
        public override string ToString()
        {
            return FirstName + " " + LastName + " " + Age + " " + Gender;
        }
    }

    internal enum Gender
    {
        Unknown,
        Female,
        Male
    }

    internal class PersonDatabase
    {
        private List<Person> _people;
        public PersonDatabase() 
        {
        _people = new List<Person>();
        }

        public void Add(Person person)
        {
            _people.Add(person);
        }

        public void Add(params Person[] persons)
        {
            foreach (Person person in persons)
            {
                Add(person);
            }
        }

        public List<Person> Find(string text) { 
            List<Person> list = new List<Person>();

            foreach (Person person in _people)
            {
                if (person.FirstName == text) list.Add(person);
            }

            return list;
        }

        public void Remove(Person person)
        {
            _people.Remove(person);
        }

        public void PrintToConsole()
        {
            foreach(Person person in _people)
            {
                Console.WriteLine(person.ToString());
            }
        } 

    }
}
