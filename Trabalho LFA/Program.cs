using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Trabalho_LFA.Extensions;
using Trabalho_LFA.Models;

namespace Trabalho_LFA
{
    class Program
    {
        /* Instanciando o autômato na mão */
        static void Exemplo1()
        {
            var no1 = new Estado("1", false);
            var no2 = new Estado("2", false);
            var no3 = new Estado("3", false);
            var no4 = new Estado("4", false);
            var no5 = new Estado("5", true);

            var listaEstados = new List<Estado> { no1, no2, no3, no4, no5 };

            /* Arestas do No1 */
            var aresta1 = new Aresta('a', no2);
            no1.Arestas.Add(aresta1);

            /* Arestas do No1 */
            var aresta2 = new Aresta('b', no1);
            var aresta3 = new Aresta('a', no3);
            var aresta4 = new Aresta('a', no4);
            no2.Arestas.Add(aresta2);
            no2.Arestas.Add(aresta3);
            no2.Arestas.Add(aresta4);

            /* Arestas do No3 */
            var aresta5 = new Aresta('b', no4);
            no3.Arestas.Add(aresta5);

            /* Arestas do No4 */
            var aresta6 = new Aresta('a', no5);
            no4.Arestas.Add(aresta6);

            var simbolosAceitos = new List<char> { 'a', 'b' };

            var afn = new AFN(listaEstados, no1, simbolosAceitos);

            /* Função Pe */
            var resultado1 = VerificaPalavra("aab", afn);
            Console.WriteLine($"Palavra: aab -> {(resultado1 ? "ACEITA" : "NÃO ACEITA")}");

            var resultado2 = VerificaPalavra("baaa", afn);
            Console.WriteLine($"Palavra: baaa -> {(resultado2 ? "ACEITA" : "NÃO ACEITA")}");

            var resultado3 = VerificaPalavra("aaa", afn);
            Console.WriteLine($"Palavra: aaa -> {(resultado3 ? "ACEITA" : "NÃO ACEITA")}");

            var resultado4 = VerificaPalavra("abababaaa", afn);
            Console.WriteLine($"Palavra: abababaaa -> {(resultado4 ? "ACEITA" : "NÃO ACEITA")}");
        }

        /* Lendo de um arquivo JSON */
        static void Exemplo2()
        {
            /* Realizamos a leitura do JSON e carregamos no objeto afnLeitura utilizando a classe AFNLeitura */
            var arquivoText = File.ReadAllText(@"C:/Users/Vinicius/Downloads/Teste.json");
            var afnLeitura = JsonConvert.DeserializeObject<AFNLeitura>(arquivoText);

            /* Criamos os estados */
            var estados = afnLeitura.Estados.Select(x => new Estado(x, false)).ToList();

            /* Atualizamos os estados que são estados finais */
            foreach (var noFinailAux in afnLeitura.EstadosFinais)
            {
                var estadoAux = estados.Find(x => x.Nome == noFinailAux);
                estadoAux.NoFinal = true;
            }

            var arestas = new List<Aresta>();

            /* Preenchemos a lista de arestas */
            foreach (var arestaAux in afnLeitura.FuncaoPrograma)
            {
                var noOrigem = estados.Find(x => x.Nome == arestaAux.Origem);
                var noDestino = estados.Find(x => x.Nome == arestaAux.Destino);
                var simbolo = arestaAux.Simbolo;

                var arestaInserir = new Aresta(simbolo, noDestino);

                noOrigem.Arestas.Add(arestaInserir);
            }

            /* Buscamos o nó inicial a partir da lista de estados */
            var noInicial = estados.Find(x => x.Nome == afnLeitura.EstadoInicial);

            /* Instanciamos o AFN */
            var afn = new AFN(estados, noInicial, afnLeitura.Simbolos);

            var keepRunning = true;

            if (afn.IsValid)
            {
                Console.WriteLine("AFN: " + afnLeitura.ToString());

                var input = string.Empty;

                while (keepRunning)
                {
                    Console.WriteLine("Digite a palavra que deseja testar (Para sair, digite -1): ");
                    input = Console.ReadLine();

                    if (input.Contains("-1"))
                        keepRunning = false;
                    else
                    {
                        /* Função Pe */
                        var resultado = VerificaPalavra(input, afn);
                        Console.WriteLine($"Palavra: {input} -> {(resultado ? "ACEITA" : "NÃO ACEITA")}");
                    }
                }

                Console.WriteLine("Finalizando programa :)");
            }
            else
            {
                Console.WriteLine("O AFN inserido não é válido!");
            }
        }

        /* Função Main */
        static void Main(string[] args)
        {
            Exemplo2();
        }

        static bool VerificaPalavra(string palavra, AFN afn)
        {
            var estadosAux = new List<Estado>();

            /* Adicionamos o estado inicial na lista de estados
             * auxiliar para começar a verificação */
            estadosAux.Add(afn.EstadoInicial);

            foreach (char simbolo in palavra)
            {
                /* Buscamos os estados que possuem transição com o
                 * simbolo 'simbolo' partindo da lista de estados 'estadosAux' */
                estadosAux = Movimento(estadosAux, simbolo);

                /* Se a lista de estados for 
                 * vazia a resposta será negativa */
                if (!estadosAux.Any())
                    return false;
            }

            var resultado = false;

            var estadosFinais = afn.EstadosFinais;

            foreach (var estado in estadosAux)
            {
                /* Verificamos se o estado está na lista de estados finais
                 * Caso esteja, resultado = true */
                resultado = resultado || estadosFinais.Any(x => x.Nome == estado.Nome);
            }

            return resultado;
        }

        /* Recebe a lista de estados e o símbolo que deseja consumir */
        static List<Estado> Movimento(List<Estado> estados, char simbolo)
        {
            var listaEstados = new List<Estado>();
            foreach (var estadoAux in estados)
            {
                /* Procura pelos estados que possuem transição com o simbolo
                 * que recebemos via parâmetro a partir da lista de estados 
                 que recebemos como parâmetro trambém */
                var nosInserir = estadoAux.Arestas
                    .Where(x => x.Simbolo == simbolo)
                    .Select(x => x.EstadoDestino)
                    .DistinctBy(x => x.Nome);

                /* Adiciona os estados na lista caso ele não esteja lá
                 * PS: Validação para não inserir estados repetidamente */
                foreach (var no in nosInserir)
                {
                    if(!listaEstados.Any(x => x.Nome == no.Nome))
                    {
                        listaEstados.Add(no);
                    }
                }
            }

            return listaEstados;
        }
    }
}
