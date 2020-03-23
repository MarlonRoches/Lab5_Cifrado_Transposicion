using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
namespace Desarollo_Cifrados
{
    class Program
    {
        static void Main(string[] args)
        {

            var GlobalPath = "C:\\Users\\roche\\Desktop\\Tea.txt";
            var KeyCesar = "Linqea8";
            CifrarCesar(KeyCesar, GlobalPath);
            DecifrarCesar(KeyCesar, GlobalPath);
            void CifrarCesar(string clave, string _path)//recibe el archivo a cifrar
            {
                var Alfabeto = LlenarAlfabeto_Cifrado();
                var write = "";
                var AlfabetoCifrado = new Dictionary<int, char>();
                byte[] KEYENCRYPTER = Encoding.ASCII.GetBytes(clave);
                var n = 0;
                foreach (var item in KEYENCRYPTER)
                {
                    if (!AlfabetoCifrado.ContainsValue((char)item))
                    {
                        AlfabetoCifrado.Add(n, (char)item);
                        n++;
                    }
                }
                AlfabetoCifrado = LlenarCesar_Cifrado(AlfabetoCifrado);

                #region Variables De Acceso
                var pathcifrado = Path.GetDirectoryName(GlobalPath);
                var nombre = Path.GetFileNameWithoutExtension(GlobalPath);
                var Archivo_Original = new FileStream(_path, FileMode.Open); //Original
                var reader = new StreamReader(Archivo_Original);

                var Archivo_Cifrado = new FileStream($"{pathcifrado}\\Csr_{nombre}.txt", FileMode.OpenOrCreate); //archivo
                var writer = new BinaryWriter(Archivo_Cifrado);

                #endregion
                var text = reader.ReadToEnd();
                foreach (var item in text)
                {
                    if (Alfabeto.ContainsKey(item) == false)
                    {
                        Alfabeto.Add(item, Alfabeto.Count);
                    }
                    if (AlfabetoCifrado.ContainsValue(item) == false)
                    {
                        AlfabetoCifrado.Add(AlfabetoCifrado.Count, item);
                    }

                    var monitor = Alfabeto[item];
                    var monitorcecar = AlfabetoCifrado[monitor];
                    write = $"{write}{monitorcecar}";
                }
                foreach (var item in write)
                {
                    writer.Write(item);
                }
                Archivo_Original.Close();
                Archivo_Cifrado.Close();
            }
            void DecifrarCesar(string codigo, string _path)
            {
                var Alfabeto = LlenarAlfabeto_DeCifrado();
                var write = "";
                var AlfabetoCifrado = new Dictionary<char, int>();
                byte[] KEYENCRYPTER = Encoding.ASCII.GetBytes(codigo);
                var n = 0;
                foreach (var item in KEYENCRYPTER)
                {
                    if (!AlfabetoCifrado.ContainsValue((char)item))
                    {
                        AlfabetoCifrado.Add((char)item, n);
                        n++;
                    }
                }
                AlfabetoCifrado = LlenarCesar_DeCifrado(AlfabetoCifrado);

                var Archivo_Cifrado = new FileStream(_path, FileMode.Open); //archivo
                var reader = new StreamReader(Archivo_Cifrado);
                var Nombre = Path.GetFileNameWithoutExtension(_path);
                var Direccion = Path.GetDirectoryName(_path);

                var Archivo_Decifrado = new FileStream($"{Direccion}\\D_csr{Nombre}.txt", FileMode.OpenOrCreate); //archivo
                var writer = new BinaryWriter(Archivo_Decifrado);


                var text = reader.ReadToEnd();
                foreach (var item in text)
                {
                    if (Alfabeto.ContainsValue(item) == false)
                    {
                        Alfabeto.Add(Alfabeto.Count, item);
                    }
                    if (AlfabetoCifrado.ContainsKey(item) == false)
                    {
                        AlfabetoCifrado.Add(item, AlfabetoCifrado.Count);
                    }

                    var monitor = AlfabetoCifrado[item];
                    var monitorcecar = Alfabeto[monitor];
                    write = $"{write}{monitorcecar}";
                }
                foreach (var item in write)
                {
                    writer.Write(item);
                }
                Archivo_Decifrado.Close();
                Archivo_Cifrado.Close();
            }

            #region Metodos de Llenado
            Dictionary<char, int> LlenarAlfabeto_Cifrado()
            {
                var temp = new Dictionary<char, int>();
                int cont = 0;
                var bufer = "(\n\r ABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚabcdefghijklmnñopqrstuvwxyzáéíóú0123456789.+-*/|°!\"#$%&/)=?'\\¿¡ _-[]:;,";

                foreach (var item in bufer)
                {
                    if (!temp.ContainsKey(item))
                    {
                        temp.Add(item, cont);
                        cont++;
                    }

                }
                return temp;
            }
            Dictionary<int, char> LlenarCesar_Cifrado(Dictionary<int, char> actual)
            {

                var texto = "(\n\r ABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚabcdefghijklmnñopqrstuvwxyzáéíóú0123456789.+-*/|°!\"#$%&/)=?'\\¿¡ _-[]:;,";
                foreach (var item in texto)
                {
                    if (actual.ContainsValue(item) == false)
                    {
                        actual.Add(actual.Count, item);
                    }

                }

                return actual;
            }
            Dictionary<int, char> LlenarAlfabeto_DeCifrado()
            {
                var temp = new Dictionary<int, char>();
                int cont = 0;
                var bufer = "(\n\r ABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚabcdefghijklmnñopqrstuvwxyzáéíóú0123456789.+-*/|°!\"#$%&/)=?'\\¿¡ _-[]:;,";

                foreach (var item in bufer)
                {
                    if (!temp.ContainsKey(item))
                    {
                        temp.Add(cont, (char)item);
                        cont++;
                    }
                }
                return temp;
            }
            Dictionary<char, int> LlenarCesar_DeCifrado(Dictionary<char, int> actual)
            {
                var texto = "(\n\r ABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚabcdefghijklmnñopqrstuvwxyzáéíóú0123456789.+-*/|°!\"#$%&/)=?'\\¿¡ _-[]:;,";
                foreach (var item in texto)
                {
                    if (actual.ContainsKey(item) == false)
                    {
                        actual.Add(item, actual.Count);
                    }

                }
                return actual;
            }
            #endregion

         
        }
    }
}
    
