namespace Rib.Common.Application.Web.Owin
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using JetBrains.Annotations;
    using Microsoft.Owin;
    using TsSoft.ContextWrapper;

    public class OwinStore : CallContextStore
    {
        private const string StoreName = "OwinStore";
        [NotNull] private readonly IOwinContextResolver _owinContextResolver;

        public OwinStore([NotNull] IOwinContextResolver owinContextResolver)
        {
            if (owinContextResolver == null) throw new ArgumentNullException(nameof(owinContextResolver));
            _owinContextResolver = owinContextResolver;
        }

        private IOwinContext Owin => _owinContextResolver.Current;

        /// <summary>
        ///     Получить объект по ключу
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="key">Ключ</param>
        public override T Get<T>(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return Owin != null
                           ? GetFromContext<T>(key, GetFromOwin<ILazy<T>>, StoreName)
                           : base.Get<T>(key);
        }

        /// <summary>
        ///     Подготовить хранилище для нового контекста
        /// </summary>
        public override void NewContext()
        {
            if (Owin != null)
            {
                NewContext(Keys, GetFromOwin<object>, PutIntoOwin, StoreName);
            }
            else
            {
                base.NewContext();
            }
        }

        /// <summary>
        ///     Вызвать Dispose для объекта из хранилища
        /// </summary>
        /// <param name="key">Ключ объекта</param>
        public override void DisposeIfCreated(string key)
        {
            if (Owin != null)
            {
                DisposeByKey(key, GetFromOwin<IDisposable>, StoreName);
            }
            base.DisposeIfCreated(key);
        }

        /// <summary>
        ///     Вызвать Dispose для всех объектов из хранилища
        /// </summary>
        public override void DisposeAll()
        {
            Dispose();
        }

        /// <summary>
        ///     Удалить все элементы из хранилища
        /// </summary>
        public override void Clear()
        {
            ClearContext(Keys, DeleteFromOwin);
            base.Clear();
        }

        /// <summary>
        ///     Инициализировано ли хранилище
        /// </summary>
        /// <param name="key">Ключ объекта</param>
        public override bool IsInitialized<T>(string key)
        {
            return Owin != null ? IsInitialized<T>(key, GetFromOwin<ILazy<T>>) : base.IsInitialized<T>(key);
        }

        /// <summary>
        ///     Создано ли значение в хранилище
        /// </summary>
        /// <param name="key">Ключ объекта</param>
        public override bool IsValueCreated<T>(string key)
        {
            return Owin != null ? IsValueCreated<T>(key, GetFromOwin<ILazy<T>>) : base.IsValueCreated<T>(key);
        }

        /// <summary>
        ///     Инициализировать хранилище для объекта
        /// </summary>
        /// <param name="key">Ключ объекта</param>
        /// <param name="contextElement">Элемент контекста с производящей объект функцией</param>
        protected override void Initialize(string key, IDisposableLazy<object> contextElement)
        {
            var isCallContextInitialized = base.IsInitialized<object>(key);
            var elem = contextElement;
            if (Owin != null)
            {
                var isOwinInitialized = IsInitialized<object>(key, GetFromOwin<ILazy<object>>);

                if (isOwinInitialized && !isCallContextInitialized)
                {
                    elem = GetFromOwin<IDisposableLazy<object>>(key);
                }
                else if (!isOwinInitialized && isCallContextInitialized)
                {
                    elem = CallContext.LogicalGetData(key) as IDisposableLazy<object>;
                }
                if (!isOwinInitialized)
                {
                    PutIntoContext(key, elem, PutIntoOwin, StoreName, Keys);
                }
            }
            if (!isCallContextInitialized)
            {
                base.Initialize(key, elem);
            }
        }

        private T GetFromOwin<T>(string key) where T : class
        {
            if (Owin == null) throw new NullReferenceException("Owin is null");
            return Owin.Get<T>(key);
        }

        private void PutIntoOwin<T>(string key, T item) where T : class
        {
            if (Owin == null) throw new NullReferenceException("Owin is null");
            Owin.Set(key, item);
        }

        private void DeleteFromOwin(string key)
        {
            if (Owin == null) throw new NullReferenceException("Owin is null");
            Owin.Environment.Remove(key);
        }

        /// <summary>
        ///     Вызвать Dispose для всех объектов из хранилища
        /// </summary>
        public new void Dispose()
        {
            if (Owin != null)
            {
                Dispose(Keys, GetFromOwin<IDisposable>, StoreName);
            }
            CallContextStore.Dispose();
        }
    }
}