namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface ITournamentTeamService : ICrudService<TournamentTeam> {}

public sealed class TournamentTeamService : ITournamentTeamService
{
    private readonly ITournamentTeamRepository _repository;

    public TournamentTeamService(ITournamentTeamRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<TournamentTeam>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<TournamentTeam>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<TournamentTeam>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<TournamentTeam>.Fail($"{nameof(TournamentTeam)} com id {id} nao encontrado.")
            : CustomResponse<TournamentTeam>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<TournamentTeam>> CreateAsync(TournamentTeam entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<TournamentTeam>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<TournamentTeam>> UpdateAsync(int id, TournamentTeam entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<TournamentTeam>.Fail($"{nameof(TournamentTeam)} com id {id} nao encontrado.")
            : CustomResponse<TournamentTeam>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(TournamentTeam)} com id {id} nao encontrado.");
    }
}
