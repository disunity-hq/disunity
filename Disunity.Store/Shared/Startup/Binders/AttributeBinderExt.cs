using BindingAttributes;

using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Startup.Binders {

    public static class AttributeBinderExt {

        public static void ConfigureAttributes(this IServiceCollection services) {
            BindingAttribute.ConfigureBindings(services);
            FactoryAttribute.ConfigureFactories(services);
        }

    }

}