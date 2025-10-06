namespace BoleriaAPI.Api.Hateoas
{
    public record Link(string Rel, string Href, string? Method = null);
}