using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes;

public static class EstudantesRotas
{
    public static void AddRotasEstudantes(this WebApplication app)
    {
        var rotasEstudantes = app.MapGroup("estudantes");
        
        // Listar estudantes ativos.
        rotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken ct) => 
        {
            var estudantes = await context
                .Estudantes
                .Where(x => x.Ativo)
                .Select(x => new EstudanteDto(x.Id, x.Nome))
                .ToListAsync(ct);

            return Results.Ok(estudantes);
        });

        // Cadastrar estudante.
        rotasEstudantes.MapPost("", async (
            AddEstudanteRequest request,
            AppDbContext context,
            CancellationToken ct
        ) => 
        {
            var jaExiste = await context.Estudantes.AnyAsync(x => x.Nome == request.Nome, ct);

            if (jaExiste) return Results.Conflict("Já existe!");

            var novoEstudante = new Estudante(request.Nome);

            await context.Estudantes.AddAsync(novoEstudante, ct);
            await context.SaveChangesAsync(ct);

            var estudanteRetorno = new EstudanteDto(novoEstudante.Id, novoEstudante.Nome);

            return Results.Ok(estudanteRetorno);
        });

        // Atualizar Nome do estudante.
        rotasEstudantes.MapPut("{id:guid}", async (
            Guid id,
            UpdateEstudanteRequest request,
            AppDbContext context,
            CancellationToken ct
        ) =>
        {
            var estudante = await context.Estudantes.FirstOrDefaultAsync(x => x.Id == id, ct);

            if (estudante == null) return Results.NotFound();

            estudante.AtualizarNome(request.Nome);

            await context.SaveChangesAsync(ct);

            var estudanteRetorno = new EstudanteDto(estudante.Id, estudante.Nome);

            return Results.Ok(estudanteRetorno);
        });

        // Deletar estudante.
        rotasEstudantes.MapDelete("{id:guid}", async (Guid id, AppDbContext context, CancellationToken ct) => 
        {
            var estudante = await context.Estudantes.SingleOrDefaultAsync(x => x.Id == id, ct);

            if (estudante == null) return Results.NotFound();

            estudante.Desativar();

            await context.SaveChangesAsync(ct);

            return Results.Ok();
        });
    }
}
