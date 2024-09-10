using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes;

public static class EstudantesRotas
{
    public static void AddRotasEstudantes(this WebApplication app)
    {
        var rotasEstudantes = app.MapGroup("estudantes");
        
        // Listar estudantes ativos.
        rotasEstudantes.MapGet("", async (AppDbContext context) => 
        {
            var estudantes = await context
                .Estudantes
                .Where(x => x.Ativo)
                .ToListAsync();

            return Results.Ok(estudantes);
        });

        // Cadastrar estudante.
        rotasEstudantes.MapPost("", async (
            AddEstudanteRequest request,
            AppDbContext context
        ) => 
        {
            var jaExiste = await context.Estudantes.AnyAsync(x => x.Nome == request.Nome);

            if (jaExiste) return Results.Conflict("Já existe!");

            var novoEstudante = new Estudante(request.Nome);

            await context.Estudantes.AddAsync(novoEstudante);
            await context.SaveChangesAsync();

            return Results.Ok(novoEstudante);
        });

        // Atualizar Nome do estudante.
        rotasEstudantes.MapPut("{id:guid}", async (
            Guid id,
            UpdateEstudanteRequest request,
            AppDbContext context
        ) =>
        {
            var estudante = await context.Estudantes.FirstOrDefaultAsync(x => x.Id == id);

            if (estudante == null) return Results.NotFound();

            estudante.AtualizarNome(request.Nome);

            await context.SaveChangesAsync();

            return Results.Ok(estudante);
        });
    }
}
