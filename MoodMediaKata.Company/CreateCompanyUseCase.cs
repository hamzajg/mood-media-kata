using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class CreateCompanyUseCase(IRepository<Company> companyRepository, AddDevicesUseCase addDevicesUseCase)
{
    public Company Execute(string name, string code, Licensing licensing, IEnumerable<Device> devices)
    {
        var company = companyRepository.Save(new Company(name, code, licensing, devices));
        addDevicesUseCase.Execute(company);
        return company;
    }
}