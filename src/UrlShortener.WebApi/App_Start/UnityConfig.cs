using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using UrlShortener.Logic;
using UrlShortener.DataAccess;
using UrlShortener.Core.Models;

namespace UrlShortener.WebApi.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        public static void RegisterTypes(IUnityContainer container)
        {
			container.RegisterType<IUrlLogic, UrlLogic>();
			container.RegisterType<IUnitOfWork, UnitOfWork>();
			container.RegisterType<IUrlShortenerContext, UrlShortenerContext>(new HierarchicalLifetimeManager());
			container.RegisterType<IRepository<Link>, Repository<Link>>(new TransientLifetimeManager());
		}
    }
}
