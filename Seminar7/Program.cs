using Seminar7;
using System.IO;

/*
 * Seminar 7: 05.06.23
 * Containerklassen
 * Autor: B.W
 */

namespace Sem7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int actCount = 0; //Zähler für Studierenden-Datensätze
            //int capacity = 4;
            char inputChar;
            bool ok = false;
            Student[] students = null;

            //bool ok = ReadFromFile(ref students, ref actCount, ref capacity);
            try
            {
                ok = ReadAllBinaryFromFile(ref students, ref actCount /*ref capacity*/);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("\n\tFehler: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("\n\tFehler: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\tFehler: " + "Keine Daten!");
            }

            if (ok)
            {
                Array.Sort(students, new MyFamNameComparer() ); //Mein Sortierer, nötig für Sortierung
                PrintStudentData(students);
            }

            do
            {
                /*
                if (actCount == capacity)
                {
                    capacity *= 2; //verdoppeln
                }
                */

                ReadDataSetFromConsole(ref students, ref actCount);

                Console.Write("\n Einen weiteren Datensatz [j/n]? ");
                inputChar = (char)Console.Read();
                if (inputChar == 'n') break;


            } while (true);

            PrintStudentData(students);
            if (actCount > 0)
            {
                //WriteToFile(students, actCount); //Speichern der Datensätze in einer Datei
                WriteAllBinaryToFile(students); //Speichern im Binär-Modus
            }

            Student s = FindStudentByName(students, "Obermoser");


            Console.ReadKey();
        }

        private static Student FindStudentByName(Student[] students, string name)
        {
            Student s = null;
            s = Array.Find(students, element => element.FamName == name); //Element im Array suchen //in Kurzschreibweise
            /* Find durchsuche alle Arrayelemente und Vergleich auf Übereinstimmung
             * Wenn es mehrere "Obermoser" gibt dann muss FindAll verwendet werden
            */
            return s;
        }


        /// <summary>
        /// Lesen eines Studierendendatensatzes von der Konsole und
        /// Erzeugen eines neuen Studierendenobjekts
        /// </summary>
        /// <param name="students">Array</param>
        /// <param name="count">aktuelle Anzahl</param>
        private static void ReadDataSetFromConsole(ref Student[]? students, ref int count) //Fragezeichen bei Student[] = kann null sein
        {
            string famName;
            int mNo, cCode;
            string course, city, street;
            bool ok = true;

            //Einlesen der Parameter über die Konsole
            do
            {
                Console.Write("\n Familienname: ");
                famName = Console.ReadLine();
                if (famName != null && famName.Length > 2) ok = true;
                else
                {
                    ok = false;
                    Console.WriteLine("\n Fehler: ");
                }
            } while (!ok);

            do
            {
                Console.Write("\n Wohnort: ");
                city = Console.ReadLine();
                if (city != null && city.Length > 3)
                    ok = true;
                else
                {
                    ok = false;
                    Console.WriteLine("\n Fehler: ...");
                }
            } while (!ok);

            do
            {
                Console.Write("\n PLZ: ");
                string codeString = Console.ReadLine();
                ok = Int32.TryParse(codeString, out cCode);
                if (ok && codeString.Length == 5) ok = true;
                else ok = false;
                if (!ok)
                {
                    Console.WriteLine("\n Fehler:  ");
                }

            } while (!ok);

            do
            {
                Console.Write("\n Straße und Hausnummer: ");
                street = Console.ReadLine();
                if (street != null && street.Length > 5)
                    ok = true;
                else
                {
                    ok = false;
                    Console.WriteLine("\n Fehler: ...");
                }
            } while (!ok);

            do
            {
                Console.Write("\n Studiengang: ");
                course = Console.ReadLine();
                if (course != null && course.Length > 5)
                    ok = true;
                else
                {
                    ok = false;
                    Console.WriteLine("\n Fehler: ...");
                }
            } while (!ok);

            //students[actCount++] = new Student(famName, course);
            //students[actCount++] = new Student(famName, city, cCode, street, course);
            try
            {
                Array.Resize(ref students, count + 1);
                students[count] = new Student(famName, course, new Address(city, cCode, street));
                PrintData(students[count]);
                count++;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("\n\tFehler: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("\n\tFehler: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\tFehler: " + ex.Message);
            }
        }

        /// <summary>
        /// Ausgabe aller Studierendendatensätze
        /// Statischer Datentyp
        /// </summary>
        /// <param name="students">Array</param>
        private static void PrintStudentData(Student[] students)
        {
            Console.WriteLine("\n\t****** Ausgabe aller Datensätze ******");
            Console.WriteLine("\n\t{0,-20}{1,-20}{2,-30}", "Matrikelnummer", "Familienname", "Studiengang");

            foreach (Student student in students)
            {
                if (student != null)
                    Console.WriteLine("\t{0,-20}{1,-20}{2,-30}",
                        student.MatrNo,
                        student.FamName,
                        student.Course);
                else
                    break;
            }
        }

        /// <summary>
        /// Ausgabe des aktuellen Datentyps
        /// </summary>
        /// <param name="p">polymorphes Personenobjekt</param>
        private static void PrintData(Person p)
        {
            Console.WriteLine("\n\t****** Aktueller Datensatz ******");
            string format = "\n\t{0,-20}{1,-20}{2,-30}{3,-20}";
            Console.WriteLine(format, "ID-Nummer", "Familienname", "Department", "Rolle");

            if (p != null)
            {
                Console.WriteLine(format,
                    p.GetIDNumber(),
                    p.FamName,
                    p.GetDepartment(),
                    p.GetRole());
                Console.WriteLine("\n\t{0}", p.ToString());
            }
        }

        /// <summary>
        /// Speichern aller Studierendendatensätze in einer Textdatei
        /// (strukturierte Daten)
        /// </summary>
        /// <param name="students">Array</param>
        /// <param name="count">aktuelle Anzahl</param>
        private static void WriteToFile(Student[] students, int count)
        {
            string path = @"..\..\students.txt";
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (int i = 0; i < count; i++)
                    {
                        sw.Write("{0,15},{1,20},{2,20},{3,20},{4,20},{5,10}#",
                            students[i].MatrNo,
                            students[i].FamName,
                            students[i].Course,
                            students[i].Adr.Residence,
                            students[i].Adr.Street,
                            students[i].Adr.PostalCode);
                        sw.Flush();
                    }
                }
            }
            //sw.Close();
            //fs.Close();
        }

        private static bool ReadFromFile(ref Student[] students, ref int count, ref int capacity)
        {
            bool ok = false;
            string path = @"..\..\students.txt";
            string line = "";
            int number;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                if (fs.CanRead)
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            //Lesen des gesamten Dateiinhalts
                            line = sr.ReadLine();
                            string[] studentStringArray = line.Split('#');
                            foreach (string studentString in studentStringArray)
                            {
                                string[] items = studentString.Split(',');
                                if (count == capacity)
                                {
                                    capacity += 2;
                                    Array.Resize(ref students, capacity);
                                }
                                if (!string.IsNullOrEmpty(items[0]))
                                {
                                    Array.Resize(ref students, count + 1); //Array um ein neues Feldelement vergrößern
                                    students[count] = new Student();
                                    ok = Int32.TryParse(items[0].Trim(), out number);
                                    if (ok)
                                    {
                                        students[count].MatrNo = number;
                                        students[count].FamName = items[1].Trim();
                                        students[count].Course = items[2].Trim();
                                        students[count].Adr.Residence = items[3].Trim();
                                        students[count].Adr.Street = items[4].Trim();
                                        ok = Int32.TryParse(items[5].Trim(), out number);
                                        if (ok)
                                            students[count].Adr.PostalCode = number;
                                        else
                                            break;
                                        count++; //Count hochzählen
                                    }
                                    else
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            return ok;
        }

        static bool WriteAllBinaryToFile(Student[] students)
        {
            bool ok = false;
            string path = @"..\..\studentsBinaryData.dat";

            if (students != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    if (fs.CanWrite)
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            foreach (Student s in students)
                            {
                                if (s == null) break;
                                bw.Write(s.MatrNo);
                                bw.Write(s.FamName);
                                bw.Write(s.Course);
                                bw.Write(s.Adr.Residence);
                                bw.Write(s.Adr.Street);
                                bw.Write(s.Adr.PostalCode);
                            }
                            ok = true;
                        }
                    }
                    else ok = false;
                }
            }

            return ok;
        }

        private static bool ReadAllBinaryFromFile(ref Student[]? students, ref int count /*ref int capacity*/) //Fragezeichen = lässt auch null
        {
            bool ok = false;
            string path = @"..\..\studentsBinaryData.dat";
            count = 0;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (fs.CanRead)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        while (br.BaseStream.Position != br.BaseStream.Length)
                        {
                            Array.Resize(ref students, count+1); //count+1 da count = 0
                            /* Nicht mehr benötigt da wir resize verwenden (dynamisch das Array vergrößern)
                            if (count == capacity)
                            {
                                capacity += 2;
                                Array.Resize(ref students, capacity);
                            }
                            */
                            students[count] = new Student();
                            students[count].MatrNo = br.ReadInt32();
                            students[count].FamName = br.ReadString().Trim();
                            students[count].Course = br.ReadString().Trim();
                            students[count].Adr.Residence = br.ReadString().Trim();
                            students[count].Adr.Street = br.ReadString().Trim();
                            students[count].Adr.PostalCode = br.ReadInt32();
                            count++; //Bleibt für den nächsten Datensatz

                        }
                        ok = true;
                    }
                }
                else ok = false;
            }

            return ok;
        }
    }

}