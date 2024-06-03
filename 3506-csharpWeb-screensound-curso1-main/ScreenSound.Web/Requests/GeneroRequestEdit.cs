using System.ComponentModel.DataAnnotations;

namespace ScreenSound.Web.Requests;

public record GeneroRequestEdit(int id, [Required] string nome, string descricao);

