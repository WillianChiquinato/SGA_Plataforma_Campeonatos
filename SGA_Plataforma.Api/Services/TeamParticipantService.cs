namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface ITeamParticipantService : ICrudService<TeamParticipant> {}

public sealed class TeamParticipantService : ITeamParticipantService
{
    private readonly ITeamParticipantRepository _repository;

    public TeamParticipantService(ITeamParticipantRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<TeamParticipant>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<TeamParticipant>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<TeamParticipant>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<TeamParticipant>.Fail($"{nameof(TeamParticipant)} com id {id} nao encontrado.")
            : CustomResponse<TeamParticipant>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<TeamParticipant>> CreateAsync(TeamParticipant entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<TeamParticipant>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<TeamParticipant>> UpdateAsync(int id, TeamParticipant entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<TeamParticipant>.Fail($"{nameof(TeamParticipant)} com id {id} nao encontrado.")
            : CustomResponse<TeamParticipant>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(TeamParticipant)} com id {id} nao encontrado.");
    }
}
