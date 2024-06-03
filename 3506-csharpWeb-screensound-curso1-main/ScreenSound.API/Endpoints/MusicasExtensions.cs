using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class MusicasExtensions
    {
        public static void AddEndPointsMusicas(this WebApplication app)
        {
            app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
            {
                var musicas = EntityToListResponseList(dal.Listar());
                return Results.Ok(musicas);
            });

            app.MapGet("/Musicas/MusicaPorNome/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
            {
                var musica = EntityToResponse(dal.RecuperarPor(m => m.Nome.ToUpper().Equals(nome.ToUpper())));

                if (musica is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(musica);
            });

            app.MapGet("/Musicas/MusicaPorAno/{ano}", ([FromServices] DAL<Musica> dal, int ano) =>
            {
                var musica = EntityToListResponseList(dal.ListarMusicasPorAno(m => m.AnoLancamento == ano));

                if (musica is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(musica);
            });

            app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal, [FromServices] DAL <Genero> dalGenero, [FromBody] MusicaRequest musicaRequest) =>
            {
                var musica = new Musica(musicaRequest.nome)
                {
                    AnoLancamento = musicaRequest.anoLancamento,
                    ArtistaId = musicaRequest.artistaId,
                    Generos = musicaRequest.generos is not null ? GeneroRequestConverter(musicaRequest.generos, dalGenero) : new List<Genero>()
                };

                dal.Adicionar(musica);

                Results.Ok(musica);
            });

            app.MapPut("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequestEdit musicaRequest) =>
            {
                var musicaAtualizada = dal.RecuperarPor(m => m.Id == musicaRequest.id);

                if (musicaAtualizada is null)
                {
                    return Results.NotFound();
                }

                musicaAtualizada.Nome = musicaRequest.nome;
                musicaAtualizada.AnoLancamento = musicaRequest.anoLancamento;
                musicaAtualizada.ArtistaId = musicaRequest.artistaId;
                return Results.NoContent();
            });

            app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> dal, int id) =>
            {
                var musica = dal.RecuperarPor(m => m.Id == id);
                if (musica is null)
                {
                    return Results.NotFound();
                }

                dal.Deletar(musica);
                return Results.NoContent();
            });

        }

        private static ICollection<MusicaResponse> EntityToListResponseList(IEnumerable<Musica> musicaList)
        {
            return musicaList.Select(a => EntityToResponse(a)).ToList();
        }

        private static MusicaResponse EntityToResponse(Musica musica)
        {
            return new MusicaResponse(musica.Id, musica.Nome, musica.AnoLancamento);
        }

        private static ICollection<Genero> GeneroRequestConverter(ICollection<GeneroRequest> generos, DAL<Genero> dalGeneros)
        {
            var listaDeGeneros = new List<Genero>();
            foreach (var item in generos)
            {
                var entity = RequestToEntity(item);
                var genero = dalGeneros.RecuperarPor(g => g.Nome.ToUpper().Equals(item.nome.ToUpper()));
                if(genero is not null)
                {
                    listaDeGeneros.Add(genero);
                }
                else
                {
                    listaDeGeneros.Add(entity);
                }
            }

            return listaDeGeneros;
        }
        
        private static Genero RequestToEntity(GeneroRequest genero)
        {
            return new Genero() { Nome = genero.nome, Descricao = genero.descricao };
        }
    }
}
