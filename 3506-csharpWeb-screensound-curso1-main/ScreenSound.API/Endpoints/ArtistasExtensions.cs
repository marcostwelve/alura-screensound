using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class ArtistasExtensions
    {
        public static void AddEndPointsArtistas(this WebApplication app)
        {
            app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
            {
                var artistas = EntityListToResponse(dal.Listar());
                return Results.Ok(artistas);
            });

            app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
            {
                var artista = EntityToResponse(dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper())));

                if (artista is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(artista);
            });

            app.MapPost("/Artistas", async ([FromServices] IHostEnvironment env, [FromServices] DAL<Artista> dal, [FromBody] ArtistaRequest artistaRequest) =>
            {
                var nome = artistaRequest.nome.Trim();
                var imagemArtista = DateTime.Now.ToString("ddMMyyyyhhss") + "." + nome + ".jpg";

                var path = Path.Combine(env.ContentRootPath, "wwwroot", "FotosPerfil", imagemArtista);
                using MemoryStream ms = new MemoryStream(Convert.FromBase64String(artistaRequest.fotoPerfil!));
                using FileStream fs = new(path, FileMode.Create);
                await ms.CopyToAsync(fs);

                var artista = new Artista(artistaRequest.nome, artistaRequest.bio)
                {
                    FotoPerfil = $"/fotoPerfil/{imagemArtista}",
                };

                dal.Adicionar(artista);
                return Results.Ok();
            });

            app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artistaEdit) =>
            {
                var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artistaEdit.id);

                if (artistaAAtualizar is null)
                {
                    return Results.NotFound();
                }

                artistaAAtualizar.Nome = artistaEdit.nome;
                artistaAAtualizar.Bio = artistaEdit.bio;

                dal.Atualizar(artistaAAtualizar);
                return Results.Ok();
            });

            app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
            {
                var artista = dal.RecuperarPor(a => a.Id == id);

                if (artista is null)
                {
                    return Results.NotFound();
                }

                dal.Deletar(artista);
                return Results.NoContent();
            });
        }

        private static ICollection<ArtistaResponse> EntityListToResponse(IEnumerable<Artista> listaDeArtista)
        {
            return listaDeArtista.Select(a => EntityToResponse(a)).ToList();
        }

        private static ArtistaResponse EntityToResponse(Artista artista)
        {
            return new ArtistaResponse(artista.Id, artista.Nome,artista.Bio, artista.FotoPerfil);
        }
    }
}
