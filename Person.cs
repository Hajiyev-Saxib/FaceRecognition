using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCam
{
    class Person
    {
        public String Name;
        public String Surname;
        public int Old;
        public int Id;
        public String[] Images;
        Person(String name,String surname,int old,int id, String[] images)
        {
            Name = ""+name;
            Surname = "" + surname;
            Old = old;
            Id = id;
            Images = new String[5];
            for (int i = 0; i < 5; i++)
                Images[i] = "" + images[i];
            

          

        }
    }
}
