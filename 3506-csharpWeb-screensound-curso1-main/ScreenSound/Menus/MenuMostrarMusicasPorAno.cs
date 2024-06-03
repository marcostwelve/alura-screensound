using ScreenSound.Banco;
using ScreenSound.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSound.Menus
{
    internal class MenuMostrarMusicasPorAno : Menu
    {
        public override void Executar(DAL<Artista> artistaDAL)
        {
            base.Executar(artistaDAL);
            ExibirTituloDaOpcao("Exibir Musicas por ano de lançamento");
            Console.Write("Digite o ano de lançamento: ");
            string anoLancamento = Console.ReadLine()!;
            var musicaDal = new DAL<Musica>(new ScreenSoundContext());
            var listaAnoLancamento = musicaDal.ListarMusicasPorAno(a => a.AnoLancamento == Convert.ToInt32(anoLancamento));
            if(listaAnoLancamento.Any())
            {
                Console.WriteLine("Músicas do ano {0}", anoLancamento);
                foreach(var musica in listaAnoLancamento)
                {
                    musica.ExibirFichaTecnica();
                }

                Console.WriteLine("\nDigite um tecla para voltar ao menu anterior");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine($"O ano {anoLancamento} não foi encontrado");
                Console.WriteLine("\nDigite uma tecla para voltar ao menu anterior");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
