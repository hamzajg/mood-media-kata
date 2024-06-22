namespace MoodMediaKata;

using System.Collections.Concurrent;

public class InMemoryCompanyRepository : ICompanyRepository
{
    private readonly IDictionary<long, Company> _companies = new ConcurrentDictionary<long, Company>();
    
    public Company Save(Company company)
    {
        _companies.Add(company.Id, company);
        return company;
    }

    public IEnumerable<Company> FindAll() => _companies.Values;

    public Company? FindOneById(long id) => _companies.ToDictionary().GetValueOrDefault(id);
}