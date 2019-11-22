using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using ReportingDemo.Repositories.Log;
using Type = System.Type;

namespace ReportingDemo
{
    public class ErrorHandler : IErrorHandler
    {
        private static ILogRepository Logger { get; set; } = LogRepository.Logger(typeof(ErrorHandler));

        private ISet<Type> NonCritical { get; }

        public ErrorHandler()
        {
            NonCritical = new HashSet<Type>
            {
                typeof(FaultException),
                typeof(ArgumentException),
                typeof(InvalidOperationException)
            };
        }

        /// <summary>
        /// For Testing Only
        /// </summary>
        /// <param name="logger">Replaces the standard logger with one used for testing; e.g., mock logger</param>
        public ErrorHandler(ILogRepository logger) : this()
        {
            Logger = logger;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
        }

        public bool HandleError(Exception e)
        {
            if (NonCritical.Contains(e.GetType()))
            {
                Logger.Warn(e.Message, e);
            }
            else
            {
                Logger.Error(e.Message, e);
            }

            return true;
        }
    }

    public class ErrorHandlingBehavior : Attribute, IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var handler = new ErrorHandler();
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(handler);
            }
        }
    }
}