using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Numerics;

namespace Desarollo_Cifrados
{
    public class Cifrados
    {
        private static Cifrados _instance = null;
        public static Cifrados Instance
        {
            get
            {
                if (_instance == null) _instance = new Cifrados();
                return _instance;
            }
        }
        string GlobalPath = string.Empty;
        #region VariablesGlobales
        string original_path = string.Empty;
        string index_p10 = "2416390875";
        //string Index_PermutacionSeleccionada = "39860127";
        string index_p8 = "52637498";
        string index_p4 = "0321";
        string index_Expand = "13023201";
        string index_inicial = "15203746";
        string index_IPinverse = "30246175";
        string index_leftshift1 = "12340";
        string[,] S0 = new string[4, 4];
        string[,] S1 = new string[4, 4];
        #endregion

        #region RSA

        public void RSA_Cipher(int p, int q, string llave)
        {
            var monitor = 0;
            var PublicKey = 0;
            var PrivateKey = 0;
            //n y phi(n)
            var N = p * q;
            var Phin = (p - 1) * (q - 1);
            var random = new Random();
            var coprimo = false;
            int lel = MCD(p, q);

            //numero entre 1 y phi(n) y verificar que sean coprimos
            while (coprimo != true && monitor != 1)
            {
                PublicKey = random.Next(1, Phin);
                //ya son coprimos
                coprimo = coprimos(PublicKey, Phin);
                //obtener inverso multiplicativo0
                PrivateKey = modInverse(PublicKey, Phin);
                //1 si el inverso esta bien
                monitor = (PublicKey * PrivateKey) % Phin;
            }
            //cifrado
            var Original = new FileStream(GlobalPath, FileMode.Open);
            var reader = new StreamReader(Original);
            var nombrearchivo = $"{Path.GetFileName(Original.Name).Split('.')[0]}_.{"RSAcif"}";

            var Cifrado = new FileStream(original_path + "\\" + nombrearchivo, FileMode.OpenOrCreate);
            var Writer = new BinaryWriter(Cifrado);
            var linea = reader.ReadLine();
            if (llave == "publica")
            {
                while (linea != null)
                {
                    foreach (var caracter in linea)
                    {
                        Writer.Write((char)Ecuacion(caracter, PublicKey, N));

                    }
                    linea = reader.ReadLine();
                }
            }
            else
            {
                while (linea != null)
                {
                    foreach (var caracter in linea)
                    {
                        Writer.Write((char)Ecuacion(caracter, PrivateKey, N));

                    }
                    linea = reader.ReadLine();
                }

            }
            Original.Close();
            Cifrado.Close();

            //Escritura de archivos
            var D = $"{PrivateKey}-{N}";
            var E = $"{PublicKey}-{N}";
            var WPc = new StreamWriter(original_path + "\\public.key");
            WPc.Write(E);
            var WPt = new StreamWriter(original_path + "\\private.key");
            WPt.Write(D);
            WPc.Close();
            WPt.Close();



        }

        public void RSA_Uncipher(int key, int N)
        {
            //Decifrando

            var Cifrado = new FileStream(GlobalPath, FileMode.Open);
            var reader = new StreamReader(Cifrado);
            var nombrearchivo = $"{Path.GetFileName(Cifrado.Name).Split('.')[0]}_Decifrado.{"txt"}";
            var Decifrado = new FileStream(original_path + "\\" + nombrearchivo, FileMode.OpenOrCreate);
            var Writer = new BinaryWriter(Decifrado);
            var linea = reader.ReadLine();
            while (linea != null)
            {
                foreach (var caracter in linea)
                {
                    Writer.Write((char)Ecuacion(caracter, key, N));

                }
                linea = reader.ReadLine();
            }
            Decifrado.Close();
            Cifrado.Close();

        }

        int Ecuacion(char caracter, int key, int n)
        {
            return int.Parse(Convert.ToString(BigInteger.ModPow((int)caracter, key, n)));
        }
        bool coprimos(int a, int b)
        {
            int delete = MCD(a, b);
            if (delete == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int MCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }
            return a == 0 ? b : a;
        }
        int modInverse(int a, int m)
        {
            a = a % m;
            for (int x = 1; x < m; x++)
                if ((a * x) % m == 1)
                    return x;
            return 0;
        }

        #endregion
        #region MetodosSDES

        //CIFRANDO
        public string[] ReturnKeys(string KeyGet)
        {
            S0[0, 0] = "01";
            S0[0, 1] = "00";
            S0[0, 2] = "11";
            S0[0, 3] = "10";
            S0[1, 0] = "11";
            S0[1, 1] = "10";
            S0[1, 2] = "01";
            S0[1, 3] = "00";
            S0[2, 0] = "00";
            S0[2, 1] = "10";
            S0[2, 2] = "01";
            S0[2, 3] = "11";
            S0[3, 0] = "11";
            S0[3, 1] = "01";
            S0[3, 2] = "11";
            S0[3, 3] = "10";
            S1[0, 0] = "00";
            S1[0, 1] = "01";
            S1[0, 2] = "10";
            S1[0, 3] = "11";
            S1[1, 0] = "10";
            S1[1, 1] = "00";
            S1[1, 2] = "01";
            S1[1, 3] = "11";
            S1[2, 0] = "11";
            S1[2, 1] = "00";
            S1[2, 2] = "01";
            S1[2, 3] = "00";
            S1[3, 0] = "10";
            S1[3, 1] = "01";
            S1[3, 2] = "00";
            S1[3, 3] = "11";


            var originalkey = int.Parse(KeyGet);
            var KEYAR = Generarkeys(originalkey);
            return KEYAR;
        }
        public void SDESCifrado(string llave1, string llave2)
        {

            var Original = new FileStream(GlobalPath, FileMode.Open);
            var lector = new BinaryReader(Original);
            var buffer = new byte[100000];
            var nombrearchivo = $"{Path.GetFileName(Original.Name).Split('.')[0]}_.{"scif"}";
            var encoded = new FileStream(original_path + "\\" + nombrearchivo, FileMode.OpenOrCreate);
            var writer = new BinaryWriter(encoded);
            while (lector.BaseStream.Position != lector.BaseStream.Length)
            {
                buffer = lector.ReadBytes(100000);
                foreach (var item in buffer)
                {
                    var caracter = (char)item;
                    var bin = Convert.ToString(item, 2).PadLeft(8, '0');
                    var monitor = Convert.ToByte(BinarioADecimal(CifradoSDES(llave1, llave2, bin)));
                    caracter = (char)monitor;
                    writer.Write(monitor);
                }
            }
            Original.Close();
            encoded.Close();
        }
        //decifrado
        public void SDESDecifrado(string llave1, string llave2)
        {
            var Cifrado = new FileStream(GlobalPath, FileMode.Open);
            var lector = new BinaryReader(Cifrado);
            var nombrearchivo = $"{Path.GetFileName(Cifrado.Name).Split('.')[0]}_.{"txt"}";

            var decoded = new FileStream(original_path + "\\" + nombrearchivo, FileMode.OpenOrCreate);
            var writer = new BinaryWriter(decoded);
            var buffer = new byte[100000];
            while (lector.BaseStream.Position != lector.BaseStream.Length)
            {
                buffer = lector.ReadBytes(100000);
                foreach (var item in buffer)
                {
                    var bin = Convert.ToString(item, 2).PadLeft(8, '0');
                    var monitor = Convert.ToByte(BinarioADecimal(CifradoSDES(llave2, llave1, bin)));
                    writer.Write(monitor);
                }
            }
            decoded.Close();
            Cifrado.Close();
        }

        string CifradoSDES(string key1, string key2, string actual)
        {
            //1 permutar 8
            var entrada = inicial(actual);

            //2 tomar izquierda y derecha 
            var Mitadizquierda = entrada.Substring(0, 4);
            var MitadDerecha = entrada.Remove(0, 4);

            //3 expandir derecho
            var expandido = Expandir(MitadDerecha);

            //4 xor key1 y lado derecho
            var xorResultado = XOR(key1, expandido);

            //5 separar en bloques de 4
            var xor1izquierda = xorResultado.Substring(0, 4);
            var xor1derecha = xorResultado.Remove(0, 4);

            //6 s0box para xorizq y s1box para xorder
            var Yaux = BinarioADecimal(($"{xor1izquierda[1]}{xor1izquierda[2]}"));
            var XAux = BinarioADecimal(($"{xor1izquierda[0]}{xor1izquierda[3]}"));

            var BoxResultL = S0[XAux, Yaux];//izquierda
            Yaux = BinarioADecimal(($"{xor1derecha[1]}{xor1derecha[2]}"));
            XAux = BinarioADecimal(($"{xor1derecha[0]}{xor1derecha[3]}"));
            var BoxResultD = S1[XAux, Yaux];//derecha

            //7 P4 a BoxResultL 
            var paso7 = P4($"{BoxResultL}{BoxResultD}");

            //8 XOR con mitarizquierda
            var paso8 = XOR(Mitadizquierda, paso7);

            //ppaso 9 y 10
            var juntosSwaped = MitadDerecha + paso8;

            //paso 11 EP bloque 2 del paso10 
            var segundoexpandido = Expandir(juntosSwaped.Remove(0, 4));
            var monico = segundoexpandido.Length;

            ////paso12 xor de segundo expandido con key 2
            var xorPaso12 = XOR(key2, segundoexpandido);
            var xorpaso12izq = xorPaso12.Substring(0, 4);
            var xorpaso12der = xorPaso12.Remove(0, 4);

            //13 s0box para xorpaso12izq y 1 para el derecho
            Yaux = BinarioADecimal(($"{xorpaso12izq[1]}{xorpaso12izq[2]}"));
            XAux = BinarioADecimal(($"{xorpaso12izq[0]}{xorpaso12izq[3]}"));
            var s0result = S0[XAux, Yaux];
            Yaux = BinarioADecimal(($"{xorpaso12der[1]}{xorPaso12.Remove(0, 4)[2]}"));
            XAux = BinarioADecimal(($"{xorpaso12der[0]}{xorpaso12der[3]}"));
            var s1result = S1[XAux, Yaux];

            //14 P4 para s0 + s1
            var Pas14 = P4(s0result + s1result);

            //15 XOR resultado paso14 con bloque1 del swap(paso10) 
            var paso15 = XOR(juntosSwaped.Substring(0, 4), Pas14);

            //16 union
            var paso16 = paso15 + juntosSwaped.Remove(0, 4);

            //17 Ip inverso
            var SalidaCifrada = IPReverse(paso16);
            return SalidaCifrada;
        }
        string[] Generarkeys(int llave)
        {
            var Devolver = new string[2];

            var binarikey = Convert.ToString(llave, 2); //1010000010
            binarikey = binarikey.PadLeft(10, '0');
            var binarikeyp10 = P10(binarikey);
            var subkey1 = binarikeyp10.Substring(0, 5);
            var subkey2 = binarikeyp10.Remove(0, 5);
            var shifedsubkey1 = LeftShift1(subkey1);
            var shifedsubkey2 = LeftShift1(subkey2);
            //primera key
            Devolver[0] = P8($"{shifedsubkey1}{shifedsubkey2}");
            //segunda key
            Devolver[1] = P8($"{LeftShift1(LeftShift1(shifedsubkey1))}{LeftShift1(LeftShift1(shifedsubkey2))}");

            return Devolver;
        }
        int BinarioADecimal(string Binario) //String binario a byte
        {

            int num, ValorBinario, ValorDecimal = 0, baseVal = 1, rem;
            num = int.Parse(Binario);
            ValorBinario = num;

            while (num > 0)
            {
                rem = num % 10;
                ValorDecimal = ValorDecimal + rem * baseVal;
                num = num / 10;

                baseVal = baseVal * 2;
            }
            return Convert.ToInt32(ValorDecimal);
        }
        string XOR(string Comparador, string AComparar)
        {
            var xorResult = string.Empty;
            for (int i = 0; i < Comparador.Length; i++)
            {
                if (AComparar[i] == Comparador[i])
                {
                    xorResult = $"{xorResult}{0}";
                }
                else
                {
                    xorResult = $"{xorResult}{1}";
                }
            }
            return xorResult;
        }
        string Expandir(string aExpandir)
        {
            var Expandido = string.Empty;
            foreach (var index in index_Expand)
            {
                Expandido = $"{Expandido}{aExpandir[int.Parse(index.ToString())]}";
            }
            return Expandido;
        }
        string IPReverse(string actual)
        {
            var IP8RevReturn = string.Empty;
            foreach (var index in index_IPinverse)
            {
                IP8RevReturn = $"{IP8RevReturn}{actual[int.Parse(index.ToString())]}";
            }
            return IP8RevReturn;
        }
        string inicial(string actual)
        {
            var iniciaretl = string.Empty;
            foreach (var index in index_inicial)
            {
                iniciaretl = $"{iniciaretl}{actual[int.Parse(index.ToString())]}";
            }
            return iniciaretl;
        }
        string P8(string actual)
        {
            var P8return = string.Empty;
            foreach (var index in index_p8)
            {
                P8return = $"{P8return}{actual[int.Parse(index.ToString())]}";
            }
            return P8return;
        }
        string LeftShift1(string aShiftear)
        {
            var Shifted = string.Empty;
            foreach (var index in index_leftshift1)
            {
                Shifted = $"{Shifted}{aShiftear[int.Parse(index.ToString())]}";
            }
            return Shifted;
        }
        string P10(string Entrada10bits)
        {
            var P10return = string.Empty;
            foreach (var index in index_p10)
            {
                P10return = $"{P10return}{Entrada10bits[Convert.ToInt32(Convert.ToString(index))]}";
            }
            return P10return;
        }
        string P4(string aPermutar)
        {
            var permmuted = string.Empty;
            foreach (var index in index_p4)
            {
                permmuted = $"{permmuted}{aPermutar[Convert.ToInt32(Convert.ToString(index))]}";
            }
            return permmuted;
        }
        #endregion
        #region Ruta

        public void CifrarRuta(int m, int n, bool tipo, bool Horario, bool Horizontal, string _Path)
        {
            GlobalPath = _Path;
            var File = new FileStream(GlobalPath, FileMode.Open);
            var reader = new StreamReader(File);
            var raw_text = reader.ReadToEnd();
            var salida = string.Empty;
            if (raw_text.Length> (m*n))
            {
                // no cabe
            }
            else
            {
                //si cabe
             salida =LecturaHoraria(raw_text, m, n,false);
            }

             salida =Vertical_a_Horizontal(salida, m, n);

        }

        public void DecifrarRuta(int m, int n, bool tipo, bool Horario, bool Horizontal, string _Path)
        {
            GlobalPath = _Path;

        }
        void Direccional(string texto, bool Horizontal, bool accion)
        {

            if (accion)
            {//lectura
                if (Horizontal)
                {

                }
                else
                {

                }
            }
            else
            {//escritura
                if (Horizontal)
                {

                }
                else
                {

                }
            }
        }
        public string Horizontal_a_Vertical(string Texto,int m,int n)
        {
            var i = 0;
            var matriz = new string[n, m];

            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < m; x++)
                {
                    if (i<Texto.Length)
                    {

                        matriz[y, x] = Texto[i].ToString();
                        i++;
                    }
                    else
                    {
                        matriz[y, x] = "_";
                    }
                    
                }
            }
            var salida = string.Empty;
            for (int x = 0; x < m; x++)
            {
                for (int y = 0; y < n; y++)
                {

                        salida += matriz[y, x];
                        i++;
                    
                }
            }
            return salida;
        }
        public string Vertical_a_Horizontal(string Texto, int m, int n)
        {

            var i = 0;
            var matriz = new string[n, m];

            
            for (int x = 0; x < m; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    if (i < Texto.Length)
                    {

                        matriz[y, x] = Texto[i].ToString();
                        i++;
                    }
                    else
                    {
                        matriz[y, x] = "_";
                    }

                }
            }
            var salida = string.Empty;
            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < m; x++)
                {

                    salida += matriz[y, x];
                    i++;

                }
            }
            return salida;
        }
        void Espiral(string texto, bool horario,bool accion)
        {

            if (accion)
            {//lectura
                if (horario)
                {

                }
                else
                {

                }
            }
            else
            {//escritura
                if (horario)
                {

                }
                else
                {

                }
            }
        }
        public string LecturaHoraria(string Texto, int m,int n, bool lectura)
        {
            bool derecha = true, izquierda = false, abajo = false;
            var matrizc = new int[m, n];
            int x = 0, y = -1;

            for (int k = 1; k <= n * m; k++)
            {
                if (izquierda)
                {
                    y--;
                    if (y == -1)
                    {
                        y = 0; x--;
                        izquierda = false;
                    }
                    else if (matrizc[x, y] != 0)
                    {
                        y++; x--;
                        izquierda = false;
                    }
                }
                else if (derecha)
                {
                    y++;
                    if (y == n)
                    {
                        y = n - 1; x++;
                        derecha = false;
                        abajo = true;
                    }
                    else if (matrizc[x, y] != 0)
                    {
                        y--; x++;
                        derecha = false;
                        abajo = true;
                    }
                }
                else if (abajo)
                {
                    x++;
                    if (x == m)
                    {
                        x = m - 1; y--;
                        abajo = false;
                        izquierda = true;
                    }
                    else if (matrizc[x, y] != 0)
                    {
                        y--; x--;
                        abajo = false;
                        izquierda = true;
                    }
                }
                else
                {
                    x--;
                    if (x == -1 || matrizc[x, y] != 0)
                    {
                        x++; y++;
                        derecha = true;
                    }
                }

                matrizc[x, y] = Texto[k-1];
            }

            //lectura 
                var salida = string.Empty;
            if (lectura)
            {//horizontal

                for (int ym = 0; ym < m; ym++)
                {
                    for (int xm = 0; xm < n; xm++)
                    {
                        salida += (char)matrizc[ym, xm];
                    }
                }
            }
            else
            {//vertical
                for (int xm = 0; xm < n; xm++)
                {
                    for (int ym = 0; ym < m; ym++)
                    {
                        salida += (char)matrizc[ym, xm];
                    }
                }
            }

            return salida;
        }
        public string LecturaAntiHoraria(string Texto, int m, int n)
        {

            return "";
        }

        #endregion
        #region Rail

        public void RailCodificar(int grado,string _path, string NonmbreArchivo)
        {
            GlobalPath = _path;
            var decoded = new FileStream(GlobalPath, FileMode.Open);
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
            var nombrearchivo = $"{Path.GetDirectoryName(decoded.Name)}\\{NonmbreArchivo}_ZigZag.txt";
            var encoded = new FileStream(nombrearchivo, FileMode.OpenOrCreate); //archivo
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


            var monitor = 0;
        }
        public void RailDecodificar(int grado, string _path, string NonmbreArchivo)
        {
            GlobalPath = _path;
            var Original = new FileStream(GlobalPath, FileMode.Open);
            var reader = new StreamReader(Original);
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


            var nombrearchivo = $"{Path.GetDirectoryName(Original.Name)}\\{NonmbreArchivo}_ZZD.txt".Replace("_ZigZag", "");
            var decoded = new FileStream(nombrearchivo, FileMode.OpenOrCreate); //archivo
            var writer = new BinaryWriter(decoded);
            var codedtext = "";


            foreach (var item in uncipher)
            {
                writer.Write(item);

            }
            decoded.Close();
            Original.Close();


            var matrix = new string[grado];
        }
        // listo
        #endregion
        #region Cesar

        public void CifrarCesar(string clave,string _path,string nombre)//recibe el archivo a cifrar
        {
            GlobalPath = _path;
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
            var Original = new FileStream(GlobalPath, FileMode.Open); //archivo
            var reader = new StreamReader(Original);
            var nombrearchivo = $"{Path.GetDirectoryName(Original.Name)}\\Ces_{nombre}.txt";
            var encoded = new FileStream(nombrearchivo, FileMode.OpenOrCreate); //archivo
            var writer = new BinaryWriter(encoded);

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
            Original.Close();
            encoded.Close();
        }
        public void DecifrarCesar(string codigo, string _path, string nombre)
        {
            GlobalPath = _path;

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

            var Cifrado = new FileStream(GlobalPath, FileMode.Open); //archivo
            var reader = new StreamReader(Cifrado);
            var nombrearchivo = $"{Path.GetDirectoryName(Cifrado.Name)}\\{nombre}_Des.txt".Replace("Ces_","");

            var decoded = new FileStream(nombrearchivo, FileMode.OpenOrCreate); //archivo
            var writer = new BinaryWriter(decoded);


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
            decoded.Close();
            Cifrado.Close();
        }
        Dictionary<char, int> LlenarAlfabeto_Cifrado()
        {
            var temp = new Dictionary<char, int>();
            int cont = 0;
           // var fTemp = new FileStream("C:\\Users\\roche\\Desktop\\Lab22_Cifrado\\Alfabeto.txt", FileMode.Open);
           // var reader = new StreamReader(fTemp);
            var bufer = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚabcdefghijklmnñopqrstuvwxyzáéíóú";

            foreach (var item in bufer)
            {
                if (!temp.ContainsKey(item))
                {
                    temp.Add((char)item, cont);
                    cont++;
                }

            }
           // fTemp.Close();
            return temp;
        }
        Dictionary<int, char> LlenarCesar_Cifrado(Dictionary<int, char> actual)
        {

           // var fTemp = new FileStream("C:\\Users\\roche\\Desktop\\Lab22_Cifrado\\Alfabeto.txt", FileMode.Open);
          //  var reader = new StreamReader(fTemp);
            var texto = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚabcdefghijklmnñopqrstuvwxyzáéíóú";
            foreach (var item in texto)
            {
                if (actual.ContainsValue(item) == false)
                {
                    actual.Add(actual.Count, item);
                }

            }


          //  fTemp.Close();
            return actual;
        }
        Dictionary<int, char> LlenarAlfabeto_DeCifrado()
        {
            var temp = new Dictionary<int, char>();
            int cont = 0;
          //  var fTemp = new FileStream("C:\\Users\\roche\\Desktop\\Lab22_Cifrado\\Alfabeto.txt", FileMode.Open);
          //  var reader = new StreamReader(fTemp);
            var bufer = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚabcdefghijklmnñopqrstuvwxyzáéíóú";

            foreach (var item in bufer)
            {
                if (!temp.ContainsKey(item))
                {
                    temp.Add(cont, (char)item);
                    cont++;
                }

            }
          //  fTemp.Close();
            return temp;
        }
        Dictionary<char, int> LlenarCesar_DeCifrado(Dictionary<char, int> actual)
        {

          //  var fTemp = new FileStream("C:\\Users\\roche\\Desktop\\Lab22_Cifrado\\Alfabeto.txt", FileMode.Open);
          //  var reader = new StreamReader(fTemp);
            var texto = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚabcdefghijklmnñopqrstuvwxyzáéíóú";
            foreach (var item in texto)
            {
                if (actual.ContainsKey(item) == false)
                {
                    actual.Add(item, actual.Count);
                }

            }


           // fTemp.Close();
            return actual;
        }
        #endregion

    } //listo
}