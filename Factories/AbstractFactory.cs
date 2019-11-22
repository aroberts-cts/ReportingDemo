namespace ReportingDemo.Factories
{
    public abstract class AbstractFactory<TFactory, TIFactoryObject> : IFactory<TIFactoryObject> where TFactory : IFactory<TIFactoryObject>, new()
    {
        private static TFactory _instance { get; set; }

        private static readonly object _lock = new object(); // Lock can't be null

        protected static TFactoryObject CreateInstance<TFactoryObject>()
            where TFactoryObject : TIFactoryObject, new()
        {
            // Try once outside the synchronization to avoid the cost of the lock
            if (_instance != null)
            {
                return _instance.Create<TFactoryObject>();
            }

            // Now make sure it wasn't due to the object being out of sync
            lock (_lock)
            {
                if (_instance != null)
                {
                    return _instance.Create<TFactoryObject>();
                }

                // Now that we're certain it doesn't exist, create a new one and cache it
                _instance = new TFactory();
                return CreateInstance<TFactoryObject>();
            }
        }

        TFactoryObject IFactory<TIFactoryObject>.Create<TFactoryObject>()
        {
            return new TFactoryObject();
        }
    }
}