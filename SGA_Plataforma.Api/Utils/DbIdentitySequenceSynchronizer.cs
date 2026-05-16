using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SGA_Plataforma.Infrastructure.Data;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Utils;

public static class DbIdentitySequenceSynchronizer
{
    public static async Task SynchronizeAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var entityTypes = dbContext.Model.GetEntityTypes()
            .Where(entityType => typeof(BaseEntity).IsAssignableFrom(entityType.ClrType));

        foreach (var entityType in entityTypes)
        {
            var tableName = entityType.GetTableName();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                continue;
            }

            var schema = entityType.GetSchema() ?? "public";
            var tableIdentifier = StoreObjectIdentifier.Table(tableName, schema);
            var idProperty = entityType.FindProperty(nameof(BaseEntity.Id));
            var columnName = idProperty?.GetColumnName(tableIdentifier) ?? "id";

            var qualifiedTableName = $"{QuoteIdentifier(schema)}.{QuoteIdentifier(tableName)}";
            var sequenceLookupTableName = $"{EscapeSqlLiteral(schema)}.{QuoteIdentifier(tableName)}";
            var quotedColumnName = QuoteIdentifier(columnName);
            var escapedColumnName = EscapeSqlLiteral(columnName);

            var sql = $"""
DO $$
BEGIN
    IF pg_get_serial_sequence('{sequenceLookupTableName}', '{escapedColumnName}') IS NOT NULL THEN
        PERFORM setval(
            pg_get_serial_sequence('{sequenceLookupTableName}', '{escapedColumnName}'),
            COALESCE((SELECT MAX({quotedColumnName}) FROM {qualifiedTableName}), 0) + 1,
            false);
    END IF;
END $$;
""";

            await dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }
    }

    private static string QuoteIdentifier(string identifier)
    {
        return $"\"{identifier.Replace("\"", "\"\"")}\"";
    }

    private static string EscapeSqlLiteral(string value)
    {
        return value.Replace("'", "''");
    }
}