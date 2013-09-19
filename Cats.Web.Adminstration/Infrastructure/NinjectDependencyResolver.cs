﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cats.Data;
using Cats.Services.Security;
using LanguageHelpers.Localization.Services;
using Ninject;
using log4net;
using Cats.Services.Administration;


namespace Cats.Web.Administration.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

       

    public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        
        private void AddBindings()
        {
            kernel.Bind<IUserAccountService>().To<UserAccountService>();
            kernel.Bind<Cats.Data.Security.IUnitOfWork>().To<Cats.Data.Security.UnitOfWork>();
            //kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<ILocalizedTextService>().To<LocalizedTextService>();
            kernel.Bind<LanguageHelpers.Localization.Data.IUnitOfWork>().To<LanguageHelpers.Localization.Data.UnitOfWork>();
            kernel.Bind<ILog>().ToMethod(context => LogManager.GetLogger(context.Request.Target.Member.DeclaringType));
            kernel.Bind<ILanguageService>().To<LanguageService>();
            kernel.Bind<IHubOwnerService>().To<HubOwnerService>();
        }
    }
}