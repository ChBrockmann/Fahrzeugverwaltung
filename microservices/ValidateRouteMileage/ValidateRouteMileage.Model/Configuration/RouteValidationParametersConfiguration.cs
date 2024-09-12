namespace ValidateRouteMileage.Model.Configuration;

public sealed record RouteValidationParametersConfiguration
{
    public int AdditionalDistancePerDayInKm { get; set; }
    public int GrantAdditionalDistanceAfterDays { get; set; }
    public int AllowedDeviationPercent { get; set; }
}