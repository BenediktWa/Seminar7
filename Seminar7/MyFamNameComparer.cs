/*
 * Klasse für das Sortieren eines Arrays
 * mit der Sort-Methode der Klasse Array
 * für das Feld Familienname
 * 
 * Letzte Änderung: 05.05.23
 * Autor: B.W
*/

using Sem7;
using System.Collections;

namespace Seminar7
{
    public class MyFamNameComparer:IComparer<Student>
    {
        public int Compare(Student? s1, Student? s2) //Nullable Objekt
        {
            if (s1 != null && s2 != null)
            {
                //return s1.FamName.CompareTo(s2.FamName); //CompareTo unterscheidet zwischen Groß und Kleinschreibung (Meier und meier sind nicht das gleiche)
                return new CaseInsensitiveComparer().Compare(s1.FamName, s2.FamName); //sortieren ohne Beachtung von Groß und Kleinschreibung
            }
            return 0;
            
        }
    }
}
