using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trabalho_LFA.Models
{
    public class AFNLeitura
    {
        public List<char> Simbolos { get; set; }
        public List<string> Estados { get; set; }
        public List<string> EstadosFinais { get; set; }
        public List<ArestaLeitura> FuncaoPrograma { get; set; }
        public string EstadoInicial { get; set; }

        public override string ToString()
        {
            return "M=("+"{"+$"{string.Join(",", Simbolos)}"+"}, "+"{"+$"{string.Join(",", Estados)}"+"}, "+"{"+$"{string.Join(",", FuncaoPrograma.Select(x => $"({x.Origem},{x.Simbolo},{x.Destino})"))}"+"}, "+ $"{EstadoInicial}" + ", " + "{"+ $"{string.Join(",",EstadosFinais)}"+ "}"+")";
        }
    }
}
