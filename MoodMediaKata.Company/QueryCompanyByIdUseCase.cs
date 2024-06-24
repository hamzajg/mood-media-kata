using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class QueryCompanyByIdUseCase(IRepository<Company> companyRepository)
{
    public Company? Execute(long id) => companyRepository.FindOneById(id);
}