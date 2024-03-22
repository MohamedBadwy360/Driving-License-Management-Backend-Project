using DLMS.Core;
using DLMS.Core.Models;
using DLMS.Core.Repositories;
using DLMS.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DLMSContext context)
        {
            _context = context;

            Applications = new BaseRepository<Application>(_context);
            ApplicationTypes = new BaseRepository<ApplicationType>(_context);
            Countries = new BaseRepository<Country>(_context);
            DetainedLicenses = new BaseRepository<DetainedLicense>(_context);
            Drivers = new BaseRepository<Driver>(_context);
            InternationalLicenses = new BaseRepository<InternationalLicense>(_context);
            Licenses = new BaseRepository<License>(_context);
            LicenseClasses = new BaseRepository<LicenseClass>(_context);
            LocalDrivingLicenseApplications = new BaseRepository<LocalDrivingLicenseApplication>(_context);
            People = new BaseRepository<Person>(_context);
            Tests = new BaseRepository<Test>(_context);
            TestAppointments = new BaseRepository<TestAppointment>(_context);
            TestTypes = new BaseRepository<TestType>(_context);
        }

        private readonly DLMSContext _context;

        public IBaseRepository<Application> Applications { get; private set; }

        public IBaseRepository<ApplicationType> ApplicationTypes { get; private set; }

        public IBaseRepository<Country> Countries { get; private set; }

        public IBaseRepository<DetainedLicense> DetainedLicenses { get; private set; }

        public IBaseRepository<Driver> Drivers { get; private set; }

        public IBaseRepository<InternationalLicense> InternationalLicenses { get; private set; }

        public IBaseRepository<License> Licenses { get; private set; }

        public IBaseRepository<LicenseClass> LicenseClasses { get; private set; }

        public IBaseRepository<LocalDrivingLicenseApplication> LocalDrivingLicenseApplications { get; private set; }

        public IBaseRepository<Person> People { get; private set; }

        public IBaseRepository<Test> Tests { get; private set; }

        public IBaseRepository<TestAppointment> TestAppointments { get; private set; }

        public IBaseRepository<TestType> TestTypes { get; private set; }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }      
    }
}
