using Microsoft.Extensions.DependencyInjection;
using Nobiom.Domain.Contracts;
using Nobiom.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Infra.IOC
{
    public static class DIContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Repositories

            services.AddScoped<IUserRepository, UserRepositroy>();

            #endregion

            #region Services



            #endregion
        }
    }
}
