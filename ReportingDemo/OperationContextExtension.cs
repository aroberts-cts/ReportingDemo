using System;
using System.ServiceModel;

namespace ReportingDemo
{
    public class OperationContextExtension : IExtension<OperationContext>
    {
        public OperationContextExtension()
        {
            RequestId = Guid.NewGuid();
        }

        public static OperationContextExtension Current
        {
            get
            {
                OperationContextExtension c = OperationContext.Current.Extensions.Find<OperationContextExtension>();
                if (c == null)
                {
                    c = new OperationContextExtension();
                    OperationContext.Current.Extensions.Add(c);
                }
                return c;
            }
        }

        public Guid RequestId { get; }

        public void Attach(OperationContext owner) { }

        public void Detach(OperationContext owner) { }
    }
}