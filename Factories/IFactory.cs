namespace ReportingDemo.Factories
{
    public interface IFactory<in TIFactoryObject>
    {
        TFactoryObject Create<TFactoryObject>() where TFactoryObject : TIFactoryObject, new();
    }
}
