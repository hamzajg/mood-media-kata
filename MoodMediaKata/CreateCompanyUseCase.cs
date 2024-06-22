namespace MoodMediaKata;

public class CreateCompanyUseCase(ICompanyRepository companyRepository, AddDevicesUseCase addDevicesUseCase)
{
    public Company Execute(string name, string code, Licensing licensing, IEnumerable<Device> devices)
    {
        var company = companyRepository.Save(new Company(name, code, licensing, devices));
        addDevicesUseCase.Execute(company);
        return company;
    }
}