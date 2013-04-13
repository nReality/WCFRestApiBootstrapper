using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
using System.ServiceModel.Web;
using API.DataContract;
using PostSharp.Aspects;
using log4net;

namespace API.Aspects
{
    [Serializable]
    public class ExceptionShieldAttribute : OnExceptionAspect
    {
        [NonSerialized]
        private ILog _log;

        public override void RuntimeInitialize(System.Reflection.MethodBase method)
        {
            _log = LogManager.GetLogger(GetType());
            base.RuntimeInitialize(method);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            InternalError internalError;
            HttpStatusCode httpStatusCode;

            var exceptionType = args.Exception.GetType();

            if (exceptionType == typeof(KeyNotFoundException))
            {
                httpStatusCode = HttpStatusCode.NotFound;
                var keyNotFoundException = args.Exception as KeyNotFoundException;
                internalError = InternalError.CreateNotFound(keyNotFoundException);
            }
            else if (exceptionType == typeof(ValidationException))
            {
                httpStatusCode = HttpStatusCode.BadRequest;
                var validationException = args.Exception as ValidationException;
                internalError = InternalError.CreateValidation(validationException);
            }
            else if (exceptionType == typeof(AuthenticationException))
            {
                httpStatusCode = HttpStatusCode.Unauthorized;
                var authenticationException = args.Exception as AuthenticationException;
                internalError = InternalError.CreateAuthentication(authenticationException);
            }
            else
            {
                httpStatusCode = HttpStatusCode.InternalServerError;
                internalError = InternalError.CreateUnexpected();
            }

            _log.Error(internalError.Id, args.Exception);
            throw new WebFaultException<InternalError>(internalError, httpStatusCode);
        }
    }

}