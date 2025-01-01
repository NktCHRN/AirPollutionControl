namespace AccountService.Domain.Models;
public sealed class Country
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AdministrationName { get; set; } = string.Empty;
    public IList<Agglomeration> Agglomerations { get; set; } = [];
}
