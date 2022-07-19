using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine
{
    class tabledata
    {


        public class Doljnost
        { 
            public int id = -1;

            public string dolj { get; set; }
            public string doljid { get; set; }


        }

        public class kategorii
        {
            public int id = -1;

            public string kat { get; set; }
            public string katjid { get; set; }


        }

        public class EdinicaIzmerenia
        {
            public int id = -1;

            public string EdIz { get; set; }
            public string EdIzid { get; set; }


        }

        public class postavshik
        {
            public int id = -1;

            public string fio { get; set; }
            public string nomertel { get; set; }
            public string adress { get; set; }
            public string postid { get; set; }


        }

        public class sotrudnik
        {
            public int id = -1;

            public string familia { get; set; }
            public string im { get; set; }
            public string otch { get; set; }
            public string vozr { get; set; }
            public string staj { get; set; }
            public string nomertel { get; set; }
            public string adress { get; set; }
            public string dolj { get; set; }
            public string sotrid { get; set; }


        }

        public class sklad
        {
            public int id = -1;

            public string nazvtov { get; set; }
            public string kolvo { get; set; }
            public string ediz { get; set; }
            public string kat { get; set; }
            public string stoim { get; set; }
            public string tovid { get; set; }


        }

        public class posttov
        {
            public int id = -1;

            public string kodtov { get; set; }
            public string stoim { get; set; }
            public string kolvo { get; set; }
            public string otvzapost { get; set; }
            public string data { get; set; }
            public string tovid { get; set; }


        }



    }
}
