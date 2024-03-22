using DLMS.Core.Models;
using DLMS.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Application> Applications { get; }
        IBaseRepository<ApplicationType> ApplicationTypes { get; }
        IBaseRepository<Country> Countries { get; }
        IBaseRepository<DetainedLicense> DetainedLicenses { get; }
        IBaseRepository<Driver> Drivers { get; }
        IBaseRepository<InternationalLicense> InternationalLicenses { get; }
        IBaseRepository<License> Licenses { get; }
        IBaseRepository<LicenseClass> LicenseClasses { get; }
        IBaseRepository<LocalDrivingLicenseApplication> LocalDrivingLicenseApplications { get; }
        IBaseRepository<Person> People { get; }
        IBaseRepository<Test> Tests { get; }
        IBaseRepository<TestAppointment> TestAppointments { get; }
        IBaseRepository<TestType> TestTypes { get; }

        int Commit();
        Task<int> CommitAsync();
    }
}
