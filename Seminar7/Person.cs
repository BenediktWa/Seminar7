/*
 * Elternklasse
 * mit objektorientierter Fehlerbehandlung (throw())
 * und Polymorphie (abstrakte Klasse, Methoden)
 * Autor: B.W
 * Version: 05.06.23
 */

namespace Sem7
{
    public enum Role
    {
        Student = 0,
        Lecturer = 1,
        StaffMember = 2
    }

    public abstract class Person
    {
        private string famName;
        Address adr;

        public Person()
        {
            famName = "not Assigned!";
            adr = new Address();
        }
       
        public Person(string famName, Address adr)
        {
            FamName = famName;
            Adr = adr;
        }

        public Person(
            string famName, 
            string city="", 
            string street="", 
            int cityCode=99999)
        {
            FamName= famName;
            Adr = new Address(city,cityCode,street);    
        }

        public string FamName
        {
            get
            {
                return famName;
            }

            set
            {
                if (value != null && value.Length > 0)
                {
                    famName = value;
                }
                else
                    throw new ArgumentNullException("Der Familienname muss angegeben werden!");
            }
        }

        public Address Adr
        {
            get => adr;
            set
            {
                if (value != null)
                    adr = value;
                else
                    throw new ArgumentNullException("Das Adressobjekt darf nicht leer sein!");
            }
        }

        public abstract Role GetRole();
        public abstract int GetIDNumber();
        public virtual int GetAge() { return DateTime.Now.Year; }
        public virtual string GetFaculty() { return "Faculty"; }
        public virtual string GetDepartment() { return "Arbeitsbereich"; }
        public virtual double GetSalary() { return 0.0; }
        public virtual string[] GetTeachingSubjects() { return new string[10]; }
        public virtual string GetContactInformtion() { return "person@hsbi.de"; }
    }
}
