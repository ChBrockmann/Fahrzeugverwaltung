using System.Text.Json.Serialization;

namespace ValidateRouteMileage.Service.ExternalServices.GoogleMaps.RequestResponse;

public record GoogleMapsRouteDistanceResponse
{
    [JsonPropertyName("destination_addresses")]
    public List<string> DestinationAddresses { get; set; } = new();

    [JsonPropertyName("origin_addresses")]
    public List<string> OriginAddresses { get; set; } = new();

    [JsonPropertyName("rows")]
    public List<Row> Rows { get; set; } = new();

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}

public class Row
{
    [JsonPropertyName("elements")]
    public List<Element> Elements { get; set; } = new();
}

public class Element
{
    [JsonPropertyName("distance")]
    public Distance Distance { get; set; } = new();

    [JsonPropertyName("duration")]
    public Duration Duration { get; set; } = new();

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}

public class Distance
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public int Value { get; set; }
}

public class Duration
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public int Value { get; set; }
}