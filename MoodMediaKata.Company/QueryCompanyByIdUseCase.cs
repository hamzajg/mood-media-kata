using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class QueryCompanyByIdUseCase(IRepository<Company> companyRepository)
{
    public async Task<Company?> Execute(long id) => await companyRepository.FindOneById(id);
}