using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Utils;

public sealed class CrudEntityRequestOperationFilter : IOperationFilter
{
    private static readonly HashSet<string> CreateExcludedProperties = new(StringComparer.Ordinal)
    {
        nameof(BaseEntity.Id),
        nameof(BaseEntity.CreatedAt),
        nameof(BaseEntity.UpdatedAt)
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody is null || !IsWriteOperation(context.ApiDescription.HttpMethod))
        {
            return;
        }

        var bodyParameter = context.ApiDescription.ParameterDescriptions
            .FirstOrDefault(parameter => parameter.Source == BindingSource.Body);

        var entityType = bodyParameter?.Type;
        if (entityType is null || !typeof(BaseEntity).IsAssignableFrom(entityType))
        {
            return;
        }

        var isCreateOperation = string.Equals(context.ApiDescription.HttpMethod, HttpMethods.Post, StringComparison.OrdinalIgnoreCase);

        var allowedPropertyNames = entityType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => !EntityRequestSanitizer.IsNavigationProperty(property))
            .Where(property => !isCreateOperation || !CreateExcludedProperties.Contains(property.Name))
            .Select(GetJsonPropertyName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var mediaType in operation.RequestBody.Content.Values)
        {
            var sourceSchema = ResolveSchema(mediaType.Schema, context.SchemaRepository);
            if (sourceSchema is null)
            {
                continue;
            }

            mediaType.Schema = CreateRequestSchema(sourceSchema, allowedPropertyNames);
        }
    }

    private static bool IsWriteOperation(string? httpMethod)
    {
        return string.Equals(httpMethod, HttpMethods.Post, StringComparison.OrdinalIgnoreCase)
            || string.Equals(httpMethod, HttpMethods.Put, StringComparison.OrdinalIgnoreCase);
    }

    private static OpenApiSchema? ResolveSchema(OpenApiSchema? schema, SchemaRepository schemaRepository)
    {
        if (schema?.Reference?.Id is { } referenceId && schemaRepository.Schemas.TryGetValue(referenceId, out var referencedSchema))
        {
            return referencedSchema;
        }

        return schema;
    }

    private static OpenApiSchema CreateRequestSchema(OpenApiSchema sourceSchema, HashSet<string> allowedPropertyNames)
    {
        return new OpenApiSchema
        {
            Type = sourceSchema.Type,
            Title = sourceSchema.Title,
            Description = sourceSchema.Description,
            Nullable = sourceSchema.Nullable,
            AdditionalPropertiesAllowed = sourceSchema.AdditionalPropertiesAllowed,
            Properties = sourceSchema.Properties
                .Where(property => allowedPropertyNames.Contains(property.Key))
                .ToDictionary(property => property.Key, property => property.Value),
            Required = new HashSet<string>(sourceSchema.Required.Where(allowedPropertyNames.Contains), StringComparer.OrdinalIgnoreCase)
        };
    }

    private static string GetJsonPropertyName(PropertyInfo property)
    {
        var jsonPropertyName = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name;
        return jsonPropertyName ?? JsonNamingPolicy.CamelCase.ConvertName(property.Name);
    }
}