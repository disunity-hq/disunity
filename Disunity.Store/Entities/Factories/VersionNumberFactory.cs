using System;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Entities.Factories {

    [AsScoped(typeof(IVersionNumberFactory))]
    public class VersionNumberFactory : IVersionNumberFactory {

        private readonly ApplicationDbContext _context;

        public VersionNumberFactory(ApplicationDbContext context) {
            _context = context;

        }

        [Factory]
        public static Func<string, Task<VersionNumber>> FindOrCreateVersionNumberFromString(IServiceProvider services) {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var factory = new VersionNumberFactory(context);

            return factory.FindOrCreateVersionNumber;
        }

        [Factory]
        public static Func<VersionNumber, Task<VersionNumber>> FindOrCreateVersionNumber(IServiceProvider services) {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var factory = new VersionNumberFactory(context);

            return factory.FindOrCreateVersionNumber;
        }

        public Task<VersionNumber> FindOrCreateVersionNumber(string versionString) {
            return FindOrCreateVersionNumber(VersionNumber.Create(versionString));
        }

        public async Task<VersionNumber> FindOrCreateVersionNumber(VersionNumber versionNumber) {
            var existingVersionNumber = await _context.VersionNumbers.SingleOrDefaultAsync(
                v => v.Major == versionNumber.Major &&
                     v.Minor == versionNumber.Minor &&
                     v.Patch == versionNumber.Patch);

            if (existingVersionNumber != null) {
                return existingVersionNumber;
            }

            _context.VersionNumbers.Attach(versionNumber);
            await _context.SaveChangesAsync();

            return versionNumber;
        }

    }

}