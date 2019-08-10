using System.Threading.Tasks;

using Disunity.Core.Archives;


namespace Disunity.Store.Entities.Factories {

    public interface IModVersionFactory {

        /// <summary>
        /// Creates a new <see cref="ModVersion"/> from the information available in the <see cref="ZipArchive"/>
        /// </summary>
        /// <remarks>
        /// This method does not actually do any of the hard work of storing the archive and creating a url for it
        /// </remarks>
        /// <param name="archive">An <see cref="ZipArchive"/> containing information to build a <see cref="Mod"/></param>
        /// <param name="context">The DbContext on which to search for version numbers</param>
        /// <returns>A new <see cref="ModVersion"/>. Be careful to ensure it is valid before adding to the db</returns>
        Task<ModVersion> FromArchiveAsync(ZipArchive archive);

    }

}