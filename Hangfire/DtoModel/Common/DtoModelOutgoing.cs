using System;
using System.Collections.Generic;
using Hangfire.Common;
using Hangfire.Common.Exceptions;

namespace Hangfire.Web.Api.DtoModel.Common
{
    public class DtoModelOutgoing
    {
        /// <summary>
        ///     Construct DTO model using payload object.
        ///     The error code is ZERO and error message is NULL.
        /// </summary>
        /// <param name="payload">the DTO payload object</param>
        public DtoModelOutgoing(IPayload payload)
        {
            Payload = payload;
            Service = new ServiceObject();
        }

        /// <summary>
        ///     Construct DTO model using payload object.
        ///     The error code is ZERO and error message is NULL.
        /// </summary>
        /// <param name="payloads">The collection of the DTO payload objects</param>
        public DtoModelOutgoing(IEnumerable<IPayload> payloads)
        {
            Payload = payloads;
            Service = new ServiceObject();
        }

        public DtoModelOutgoing(IEnumerable<DtoModelOutgoing> payloads)
        {
            Payload = payloads;
            Service = new ServiceObject();
        }

        /// <summary>
        ///     Construct DTO model without payload object using error code and message.
        /// </summary>
        /// <param name="errorCode">the error code</param>
        /// <param name="errorMessage">the error user friendly message</param>
        public DtoModelOutgoing(ErrorCodes errorCode, string errorMessage)
        {
            Payload = null;
            Service = new ServiceObject(errorCode, errorMessage);
        }

        /// <summary>
        ///     Construct DTO model without payload object using WebApiServiceException.
        /// </summary>
        /// <param name="exception">the exception</param>
        public DtoModelOutgoing(ManagedException exception, IPayload errorDetails = null)
        {
            Payload = errorDetails;
            Service = new ServiceObject(exception.ErrorCode, exception.ErrorMessage);
        }

        /// <summary>
        ///     Construct DTO model without payload object using WebApiServiceException.
        /// </summary>
        /// <param name="exception">the exception</param>
        public DtoModelOutgoing(ManagedException exception, IEnumerable<IPayload> errorDetails)
        {
            Payload = errorDetails;
            Service = new ServiceObject(exception.ErrorCode, exception.ErrorMessage);
        }

        public ServiceObject Service { get; private set; }
        public Object Payload { get; set; }

        /// <summary>
        ///     Internal service object implementation.
        /// </summary>
        public class ServiceObject
        {
            public ServiceObject()
            {
                ErrorCode = ErrorCodes.None;
                ErrorMessage = null;
            }

            public ServiceObject(ErrorCodes errorCode, string errorMessage)
                : this()
            {
                ErrorCode = errorCode;
                ErrorMessage = errorMessage;
            }

            public ErrorCodes ErrorCode { get; private set; }
            public string ErrorMessage { get; private set; }
            public bool Successful
            {
                get { return ErrorCode == 0; }
            }
        }
    }    
}