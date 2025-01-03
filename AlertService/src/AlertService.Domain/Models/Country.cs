using DomainAbstractions;

namespace AlertService.Domain.Models;
public class Country : ISoftDeletable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AdministrationName { get; set; } = string.Empty;
    public IList<Agglomeration> Agglomerations { get; set; } = [];
    public bool IsDeleted { get; set; }
}
