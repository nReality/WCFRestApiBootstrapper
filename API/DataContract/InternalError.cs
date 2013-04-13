using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;

namespace API.DataContract
{
    [DataContract(Name = "InternalError")]
    public class InternalError
    {
        public InternalError(ErrorCodes code, string message)
        {
            Code = code.ToString();
            Message = message;
            Id = Guid.NewGuid();
        }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public Exception OriginalException { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        public override string ToString()
        {
            return Message;
        }

        public static InternalError CreateUnexpected()
        {
            return new InternalError(ErrorCodes.ErrorCodeUnexpected, Resources.ErrorMessageUnexpected);
        }

        public static InternalError CreateValidation(ValidationException validationException)
        {
            return new InternalError(ErrorCodes.ErrorCodeInputValidation, validationException.Message);
        }

        public static InternalError CreateAuthentication(AuthenticationException authenticationException)
        {
            return new InternalError(ErrorCodes.ErrorCodeUnAuthenticated, authenticationException.Message);
        }

        public static InternalError CreateNotFound( KeyNotFoundException notFoundException)
        {
            return new InternalError(ErrorCodes.ErrorCodeEntryDoesNotExist, notFoundException.Message);
        }
    }
}