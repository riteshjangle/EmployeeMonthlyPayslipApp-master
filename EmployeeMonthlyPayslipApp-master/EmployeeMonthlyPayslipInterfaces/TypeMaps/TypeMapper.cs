using AutoMapper;
using EmployeePayDetailsCommon.TypeMaps;

namespace EmployeeMonthlyPayslipInterfaces.TypeMaps
{
    public class TypeMapper
    {
        private static readonly IConfigurationProvider _autoMapperConfiguration;

        static TypeMapper()
        {
            _autoMapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AppModelProfile>();
                cfg.AddProfile<DomainToAppProfile>();
            });
        }

        public static TypeMapper InitilizeTypeConfiguration()
        {
            return new TypeMapper();
        }

        public IMapper InitializeMapper()
        {
            _autoMapperConfiguration.AssertConfigurationIsValid();
            return _autoMapperConfiguration.CreateMapper();
        }
    }
}