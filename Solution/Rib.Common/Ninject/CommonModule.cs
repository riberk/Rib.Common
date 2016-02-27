﻿namespace Rib.Common.Ninject
{
    /// <summary>
    ///     Загружает модули сборки <see cref="Rib.Common" />
    /// </summary>
    public class CommonModule : RibNinjectModule
    {
        /// <summary>
        ///     Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            base.Load();
            Bind<IResolver>().To<NinjectResolver>().InSingletonScope();
        }
    }
}