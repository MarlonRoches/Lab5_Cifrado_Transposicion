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

            #region Transposition
            void ZigZag()
            {
                Codificar(4);
                Decodificar(4);

                void Codificar(int grado)
                {
                    var decoded = new FileStream("C:\\Users\\roche\\Desktop\\Lab22_Cifrado\\SpiderSenses.txt", FileMode.Open);
                    var reader = new StreamReader(decoded);
                    var text = reader.ReadToEnd();
                    var nivel = new string[grado];
                    var index = 0;
                    bool direction = false;
                    var ciclos = text.Length / grado;
                    while (text != "")
                    {
                        if (index < 4) //abajo
                        {
                            for (int i = 0; i < grado; i++)
                            {
                                if (text == "")
                                {
                                    break;
                                }
                                nivel[i] = nivel[i] + text[0].ToString();
                                text = text.Remove(0, 1);
                                index++; direction = false;
                            }
                            if (text == "")
                            {
                                break;
                            }
                        }//para abajo
                        else //arriba
                        {
                            for (int i = grado - 2; i > 0; i--)
                            {
                                if (text == "")
                                {
                                    for (int r = i; r > -1; r--)
                                    {
                                        nivel[r] = nivel[r] + "$";
                                    }
                                    break;
                                }
                                nivel[i] = nivel[i] + text[0].ToString();
                                text = text.Remove(0, 1);
                                direction = true;
                            }
                            index = 0;
                        } //para arriba

                    }

                    //Escritura
                    var nombrearchivo = $"{Path.GetFileName(decoded.Name).Split('.')[0]}_ZigZag.{"cif"}";
                    var encoded = new FileStream("C:\\Users\\roche\\Desktop\\Lab22_Cifrado\\" + nombrearchivo, FileMode.OpenOrCreate); //archivo
                    var writer = new BinaryWriter(encoded);
                    var codedtext = "";
                    foreach (var item in nivel)
                    {
                        codedtext = codedtext + item;
                    }
                    var temp = codedtext.ToArray();
                    foreach (var item in temp)
                    {
                        writer.Write(item);

                    }
                    decoded.Close();
                    encoded.Close();


                }
                void Decodificar(int grado)
                {
                    var encoded = new FileStream("C:\\Users\\roche\\Desktop\\Lab22_Cifrado\\SpiderSenses_ZigZag.cif", FileMode.Open);
                    var reader = new StreamReader(encoded);
                    var ciphertext = reader.ReadToEnd();
                    var m = (ciphertext.Length + (2 * grado) - 3) / ((2 * grado) - 2);
                    var midtline = (m - 1) * 2;
                    var lastline = m - 1;
                    var nivel = new string[grado];
                    //primer nivel
                    nivel[0] = ciphertext.Substring(0, m);
                    ciphertext = ciphertext.Remove(0, m);
                    //intermedios
                    for (int i = 1; i < grado - 1; i++)
                    {
                        nivel[i] = ciphertext.Substring(0, midtline);
                        ciphertext = ciphertext.Remove(0, midtline);

                    }
                    //final
                    nivel[grado - 1] = ciphertext;
                    var uncipher = string.Empty;
                    while (nivel[0] != "" && nivel[grado - 1] != "")
                    {
                        var index = 0;
                        for (int i = 0; i < grado; i++)
                        {
                            uncipher = uncipher + nivel[i][0];
                            nivel[i] = nivel[i].Remove(0, 1);
                        }
                        for (int i = grado - 2; i > 0; i--)
                        {
                            uncipher = uncipher + nivel[i][0];
                            nivel[i] = nivel[i].Remove(0, 1);
                        }
                    }
                    uncipher = uncipher.Replace('$', ' ');


                    var nombrearchivo = $"{Path.GetFileName(encoded.Name).Split('_')[0]}_ZigZagDecoded.{"txt"}";
                    var decoded = new FileStream("C:\\Users\\roche\\Desktop\\Lab22_Cifrado\\" + nombrearchivo, FileMode.OpenOrCreate); //archivo
                    var writer = new BinaryWriter(decoded);
                    var codedtext = "";


                    foreach (var item in uncipher)
                    {
                        writer.Write(item);

                    }
                    decoded.Close();
                    encoded.Close();


                    var matrix = new string[grado];
                }
            }
            void Espiral()
            {
                var Reader = new StreamReader("C:\\Users\\roche\\Desktop\\Lab22_Cifrado\\SpiderSenses.txt");
                var texto = Reader.ReadToEnd();
                Console.WriteLine("n");
                var n = int.Parse(Console.ReadLine());
                Console.WriteLine("m");
                var temp = Console.ReadLine();
                var m = 0.00;
                if (temp == "") //segundo valor vacio
                {
                    m = texto.Length / n;
                    var mod = texto.Length % n;

                    if (mod != 0)
                    {
                        m++;
                    }
                }
                var matriz = new char[n, (int)m];
                var array = texto.ToArray();
                int index = 0;
                for (int y = 0; y < n; y++)
                {
                    for (int x = 0; x < m; x++)
                    {
                        try
                        {
                            matriz[y, x] = array[index];
                        }
                        catch (Exception)
                        {

                            matriz[y, x] = '$';
                        }
                        index = index + 1;
                    }
                }


                void Escribir(int x, int y)
                {
                    for (int i = 0; i < y; i++)
                    {
                    }
                }
            } //falta recorrido en espiral
           

            #endregion        
        }
    }
}
    
