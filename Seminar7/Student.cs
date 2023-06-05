using System;
/*
 * S6: Einfache Klasse für Studierendenobjekte
 *     mit objektorientierter Fehlerbehandlung (throw())
 *     Polymorphie (überschriebene Methoden)
 * Autor: B.W
 * Version: 05.06.23
 */
namespace Sem7
{
    public class Student: Person
    {
        //fields, Instanz- oder Klassenvariable
        private static int matriculationNo = 1000000;
        private string course;
        private int matrNo;

        public Student():base()
        {
            MatrNo = 9999999;
            Course = "not assigned";
        }

        public Student(
            string famName,
            string course,
            string city="",
            int ccode=99999,
            string street = "") :base(famName,city,street,ccode)
        {
            MatrNo = GetMatriculationNo();
            Course = course;
        }
        
        public Student(
            string famName,
            string course):base(famName)
        {
            MatrNo = GetMatriculationNo(); ;
            Course = course;
        }
        
        public Student(
            string famName,
            string course,
            Address address):base(famName,address)
        {
            MatrNo = GetMatriculationNo(); 
            Course = course;
        }

        //Properties
        
        public int MatrNo
        {
            get => matrNo;
            set
            {
                //if (value > 1000000)
                if(value.ToString().Length == 7)
                {
                    matrNo = value;
                    matriculationNo = value;
                    WriteLastMatriculationNo();
                }
            }
        }
        public string Course
        {
            get => course;
            set
            {
                if (value != null && value.Length > 0)
                {
                    course = value;
                }
                else
                    throw new ArgumentException("Studiengangsname ist nicht gültig!");
            }
        }

        public int GetMatriculationNo()
        {
            return ++matriculationNo;
        }
        
        private void WriteLastMatriculationNo()
        {
            matriculationNo = matrNo;
            string path = @"..\..\lastMatriculationNo.txt";
            using(FileStream fs = new FileStream(path, FileMode.Create))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(matriculationNo);
                    sw.Flush();
                }
            }
        }

        public override string? ToString()
        {
            string adresse =
                Adr.PostalCode.ToString() + " - " +
                Adr.Residence + " - " +
                Adr.Street;
            
            return 
                FamName + " - " +
                adresse + " - " +
                MatrNo.ToString() + " - " +
                Course + " - " +
                Role.Student;
        }

        public override Role GetRole()
        {
            return Role.Student;
        }

        public override int GetIDNumber()
        {
            return MatrNo;
        }

        public override string GetDepartment()
        {
            return this.Course;
        }
    }
}
