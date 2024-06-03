using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class GeneroExtensions
    {
        public static void AddEndPointsGeneros(this WebApplication app)
        {
            app.MapGet("/Generos", ([FromServices]DAL<Genero> dal) =>
            {
                var generos = EntityListResponse(dal.Listar());
                return Results.Ok(generos);
            });

            app.MapGet("/Generos/{nome}", ([FromServices]DAL<Genero> dal, string nome) =>
            {
                var genero = EntityToResponse(dal.RecuperarPor(g => g.Nome.ToUpper().Equals(nome.ToUpper())));

                if(genero is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(genero);
            });

            app.MapPost("/Generos", ([FromServices]DAL<Genero> dal, [FromBody]GeneroRequest generoRequest) =>
            {
                var genero = dal.RecuperarPor(g => g.Nome == generoRequest.nome);

                if(genero is not null)
                {
                    return Results.Ok("Gênero já cadastrado");
                }
                else
                {
                    genero = new Genero(generoRequest.nome)
                    {
                        Descricao = generoRequest.descricao,
                    };

                    dal.Adicionar(genero);
                }

                return Results.NoContent();
            });

            app.MapPut("/Generos", ([FromServices]DAL<Genero> dal, [FromBody]GeneroRequestEdit generoRequest) =>
            {
                var genero = dal.RecuperarPor(g => g.Id == generoRequest.id);

                if (genero is null)
                {
                    return Results.NotFound();
                }

                genero.Nome = generoRequest.nome;
                genero.Descricao = generoRequest.descricao;
                return Results.NoContent();
            });

            app.MapDelete("/Generos/{id}", ([FromServices] DAL<Genero> dal, int id) =>
            {
                var genero = dal.RecuperarPor(g => g.Id == id);
                if(genero is null)
                {
                    return Results.NotFound();
                }

                dal.Deletar(genero);
                return Results.NoContent();
            });

        }

        private static ICollection<GeneroResponse> EntityListResponse(IEnumerable<Genero> listaDeGeneros)
        {
            return listaDeGeneros.Select(g => EntityToResponse(g)).ToList();
        }

        private static GeneroResponse EntityToResponse(Genero genero)
        {
            return new GeneroResponse(genero.Id, genero.Nome, genero.Descricao);
        }
    }
}
