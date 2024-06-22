using System.Collections;

namespace MoodMediaKata;

public interface ICompanyRepository
{
    Company Save(Company company);
    IEnumerable<Company> FindAll();
    Company? FindOneById(long id);
}