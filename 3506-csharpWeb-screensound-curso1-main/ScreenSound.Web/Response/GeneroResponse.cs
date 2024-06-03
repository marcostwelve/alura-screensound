namespace ScreenSound.Web.Response;

public record GeneroResponse(int? id, string? nome, string? descricao)
{
    public override string ToString()
    {
        return $"{this.nome}";
    }
}

