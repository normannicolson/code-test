namespace Reserve.Presentation.Api.Models;

public class HateoasModel<T>
{
    public T Value { get; }

    public IDictionary<string, HateoasLink> Links { get; }

    public HateoasModel(T value)
    {
        this.Value = value;
        this.Links = new Dictionary<string, HateoasLink>();
    }

    public void AddLink(string url, string rel, string method)
    {
        this.Links.Add(rel.ToLower(System.Globalization.CultureInfo.InvariantCulture), new HateoasLink(url, rel, method));
    }
}
