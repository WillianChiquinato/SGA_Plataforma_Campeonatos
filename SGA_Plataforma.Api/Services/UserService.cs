namespace SGA_Plataforma.Api.Services;

using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IUserService : ICrudService<User>
{
    Task<CustomResponse<UserDTO>> GetUserByEmailAndPassword(string email, string password, CancellationToken cancellationToken = default);
}

public sealed class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<User>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            return CustomResponse<IReadOnlyList<User>>.SuccessTrade(entities, entities.Count);
        }
        catch (Exception ex)
        {
            return CustomResponse<IReadOnlyList<User>>.Fail($"Erro ao obter usuários: {ex.Message}");
        }
    }

    public async Task<CustomResponse<User>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            return entity is null
                ? CustomResponse<User>.Fail($"{nameof(User)} com id {id} nao encontrado.")
                : CustomResponse<User>.SuccessTrade(entity);
        }
        catch (Exception ex)
        {
            return CustomResponse<User>.Fail($"Erro ao obter usuário: {ex.Message}");
        }
    }

    public async Task<CustomResponse<User>> CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        try
        {
            // Garante hash bcrypt na senha
            if (!string.IsNullOrWhiteSpace(entity.PasswordHash) && !IsBcryptHash(entity.PasswordHash))
            {
                entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(entity.PasswordHash);
            }

            var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
            return CustomResponse<User>.SuccessTrade(createdEntity);
        }
        catch (Exception ex)
        {
            return CustomResponse<User>.Fail($"Erro ao criar usuário: {ex.Message}");
        }
    }

    public async Task<CustomResponse<User>> UpdateAsync(int id, User entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
            return updatedEntity is null
                ? CustomResponse<User>.Fail($"{nameof(User)} com id {id} nao encontrado.")
                : CustomResponse<User>.SuccessTrade(updatedEntity);
        }
        catch (Exception ex)
        {
            return CustomResponse<User>.Fail($"Erro ao atualizar usuário: {ex.Message}");
        }
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var deleted = await _repository.DeleteAsync(id, cancellationToken);
            return deleted
                ? CustomResponse<bool>.SuccessTrade(true)
                : CustomResponse<bool>.Fail($"{nameof(User)} com id {id} nao encontrado.");
        }
        catch (Exception ex)
        {
            return CustomResponse<bool>.Fail($"Erro ao deletar usuário: {ex.Message}");
        }
    }

    public async Task<CustomResponse<UserDTO>> GetUserByEmailAndPassword(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return CustomResponse<UserDTO>.Fail("Email ou senha inválidos");

            var result = await _repository.GetUserByEmail(email, cancellationToken);

            if (result == null || string.IsNullOrWhiteSpace(result.PasswordHash))
                return CustomResponse<UserDTO>.Fail("Email ou senha inválidos");

            var isValidPassword = VerifyPassword(password, result.PasswordHash);

            return !isValidPassword
                    ? CustomResponse<UserDTO>.Fail("Email ou senha inválidos")
                    : CustomResponse<UserDTO>.SuccessTrade(result.ToSafeDto());
        }
        catch (Exception ex)
        {
            return CustomResponse<UserDTO>.Fail($"Erro ao autenticar usuário: {ex.Message}");
        }
    }

    private static bool VerifyPassword(string rawPassword, string storedHash)
    {
        if (IsBcryptHash(storedHash))
            return BCrypt.Net.BCrypt.Verify(rawPassword, storedHash);

        return false;
    }

    private static bool IsBcryptHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 20)
            return false;

        return value.StartsWith("$2a$") || value.StartsWith("$2b$") || value.StartsWith("$2y$");
    }
}
