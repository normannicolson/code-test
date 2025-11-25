namespace Reserve.Presentation.Api.Models;

public class HateoasLink
{
    public string Href { get; }
    
    public string Rel { get; }

    public string Method { get; }

    public HateoasLink(string href, string rel, string method)
    {
        this.Href = href;
        this.Rel = rel;
        this.Method = method;
    }
}
