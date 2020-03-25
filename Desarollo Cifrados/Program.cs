using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using Desarollo_Cifrados;
namespace Desarollo_Cifrados
{
    class Program
    {
        static void Main(string[] args)
        {

            var CifradoPath = "C:\\Users\\roche\\Desktop\\Tea.txt";
            var KeyCesar = "Quesio";
           // Cifrados.Instance.CifrarCesar(KeyCesar, CifradoPath);
            var DesCifradoPath = "C:\\Users\\roche\\Desktop\\Ces_Tea.txt";
           // Cifrados.Instance.DecifrarCesar(KeyCesar, DesCifradoPath);


            Cifrados.Instance.RailCodificar(5, CifradoPath);
            Cifrados.Instance.RailDecodificar(5, CifradoPath);

        }
    }
}
    
