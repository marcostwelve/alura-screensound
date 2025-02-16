﻿namespace ScreenSound.Web.Requests;

public record MusicaRequest(string nome, int anoLancamento, int artistaId, ICollection<GeneroRequest> generos = null);