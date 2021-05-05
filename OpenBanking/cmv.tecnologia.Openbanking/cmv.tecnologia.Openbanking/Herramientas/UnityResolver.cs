using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Unity;

namespace cmv.tecnologia.Openbanking.Herramientas
{
    public class UnityResolver : IDependencyResolver
    {
        private readonly IUnityContainer _container;
        /// <summary>
        /// Constructor del resolvedor de dependencias
        /// </summary>
        /// <param name="container"></param>
        public UnityResolver(IUnityContainer container)
        {
            _container = container;
        }
        /// <summary>
        /// Inicializador del resolvedor de dependencias
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            var child = _container.CreateChildContainer();
            return new UnityResolver(child);
        }
        /// <summary>
        /// Método para destruir el objeto
        /// </summary>
        public void Dispose()
        {
            _container.Dispose();
        }
        /// <summary>
        /// Método para obtener la dependencia
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }
        /// <summary>
        /// Método para devolver todas las dependencias
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }
    }
}