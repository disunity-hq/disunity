using System.Threading.Tasks;


namespace Disunity.Store.Data.Seeds {

    public interface ISeeder {

        bool ShouldSeed();
        Task Seed();

    }

}