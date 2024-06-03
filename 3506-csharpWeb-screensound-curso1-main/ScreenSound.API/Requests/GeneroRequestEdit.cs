using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests;

public record GeneroRequestEdit(int id, [Required]string nome, string descricao);

