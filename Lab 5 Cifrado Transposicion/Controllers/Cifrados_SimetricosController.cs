using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lab_5_Cifrado_Transposicion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cifrados_SimetricosController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hola";
        }
        [HttpPost]
        [Route("Cifrar")]
        public void Cifrar([FromBody]object json)
        {
            var x = JsonConvert.DeserializeObject<Entrada>(json.ToString());
            switch (x.Algoritmo.ToLower())
            {
                case "cesar":
                    Cifrados.Instance.CifrarCesar(x.LlaveCesar, x.Path, x.Nombre);
                    break;
                case "zigzag":
                    Cifrados.Instance.CifrarRail(x.Carriles, x.Path, x.Nombre);
                    break;
                case "ruta":
                    Cifrados.Instance.Ruta(x.m, x.Direccion, x.Path, x.Nombre);
                    break;
                default:
                    break;
            }
        }
        [HttpPost]
        [Route("Decifrar")]
        public void Decifrar([FromBody]object json)
        {
            var x = JsonConvert.DeserializeObject<Entrada>(json.ToString());
            switch (x.Algoritmo.ToLower())
            {
                case "cesar":
                    Cifrados.Instance.DecifrarCesar(x.LlaveCesar, x.Path, x.Nombre);
                    break;
                case "zigzag":
                    Cifrados.Instance.DecifrarRail(x.Carriles, x.Path, x.Nombre);
                    break;
                case "ruta":
                    Cifrados.Instance.Ruta(x.m, x.Direccion, x.Path, x.Nombre);
                    break;
                default:
                    break;
            }
        }
    }
}