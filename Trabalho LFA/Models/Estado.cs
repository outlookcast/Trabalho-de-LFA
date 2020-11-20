using System;
using System.Collections.Generic;
using System.Text;

namespace Trabalho_LFA.Models
{
    public class Estado
    {
        public string Nome { get; set; }
        public bool NoFinal { get; set; }

        public List<Aresta> Arestas { get; set; }

        public Estado(string nome, bool noFinal)
        {
            Nome = nome;
            NoFinal = noFinal;
            Arestas = new List<Aresta>();
        }
    }
}
