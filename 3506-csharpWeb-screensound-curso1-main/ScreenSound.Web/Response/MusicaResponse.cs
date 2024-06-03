namespace ScreenSound.Web.Response;

public record MusicaResponse(int id, string nome, int? anoLancamento, int artistaId, string nomeArtista);

