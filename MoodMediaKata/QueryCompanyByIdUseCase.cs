namespace MoodMediaKata;

public class QueryCompanyByIdUseCase(ICompanyRepository companyRepository)
{
    public Company? Execute(long id) => companyRepository.FindOneById(id);
}