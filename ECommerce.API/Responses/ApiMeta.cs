using System.Text.Json.Serialization;

namespace ECommerce.API.Responses;

public class ApiMeta
{
    public string TraceId { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PaginationMeta? Pagination { get; set; }

}
