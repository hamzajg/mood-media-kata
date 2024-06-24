using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class CreateCompanyUseCase(IRepository<MoodMediaKata.Company.Company> companyRepository, AddDevicesUseCase addDevicesUseCase)
{
    public MoodMediaKata.Company.Company Execute(string name, string code, Licensing licensing, IEnumerable<Device> devices)
    {
        var company = companyRepository.Save(new MoodMediaKata.Company.Company(name, code, licensing, devices));
        addDevicesUseCase.Execute(company);
        return company;
    }
}