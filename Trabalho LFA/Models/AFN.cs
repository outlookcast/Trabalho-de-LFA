using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trabalho_LFA.Models
{
    public class AFN
    {
        public List<Estado> Estados { get; set; }
        public Estado EstadoInicial { get; set; }

        public List<char> SimbolosAceitos { get; set; }

        public List<Estado> EstadosFinais => Estados.Where(x => x.NoFinal).ToList();

        public bool IsValid { get; set; }

        public AFN(List<Estado> estados, Estado estadoInicial, List<char> simbolosAceitos)
        {
            Estados = estados;
            EstadoInicial = estadoInicial;
            SimbolosAceitos = simbolosAceitos;

            IsValid = true;

            /* Caso exista alguma transição com simbolo que não esteja
             * na lista de Simbolos aceitos o AFN não é válido! */
            foreach (var estado in Estados)
            {
                if(estado.Arestas.Any(x => !SimbolosAceitos.Contains(x.Simbolo)))
                {
                    IsValid = false;
                }
            }
        }
    }
}
