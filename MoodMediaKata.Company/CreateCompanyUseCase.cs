using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class CreateCompanyUseCase(IRepository<Company> companyRepository, AddDevicesUseCase addDevicesUseCase)
{
    public async Task<Company> Execute(string name, string code, Licensing licensing, IEnumerable<Device> devices)
    {
        var company = await companyRepository.Save(new Company(name, code, licensing, devices));
        await addDevicesUseCase.Execute(company);
        return company;
    }
}