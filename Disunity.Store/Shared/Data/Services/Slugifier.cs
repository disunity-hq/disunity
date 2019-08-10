using BindingAttributes;

using Slugify;


namespace Disunity.Store.Data.Services {

    public interface ISlugifier {

        string Slugify(string input);

    }

    [AsSingleton(typeof(ISlugifier))]
    public class Slugifier : ISlugifier {

        private SlugHelper _slugHelper;

        public Slugifier() {
            _slugHelper = new SlugHelper();    
        }

        public string Slugify(string input) {
            return _slugHelper.GenerateSlug(input);
        }

    }

}