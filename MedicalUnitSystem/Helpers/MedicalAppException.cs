using System.Net;

namespace MedicalUnitSystem.Helpers
{
    public class MedicalAppException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public MedicalAppException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError):base(message)
        {
            StatusCode = statusCode;
        }
    }
}
