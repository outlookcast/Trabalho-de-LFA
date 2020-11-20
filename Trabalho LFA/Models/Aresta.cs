using System;
using System.Collections.Generic;
using System.Text;

namespace Trabalho_LFA.Models
{
    public class Aresta
    {
        public char Simbolo { get; set; }
        public Estado EstadoDestino { get; set; }

        public Aresta(char simbolo, Estado estadoDestino)
        {
            Simbolo = simbolo;
            EstadoDestino = estadoDestino;
        }
    }
}
