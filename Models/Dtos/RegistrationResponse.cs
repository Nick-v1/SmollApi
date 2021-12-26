namespace SmollApi.Models.Dtos
{
    public class RegistrationResponse
    {
        public RegistrationResponse(string errors, bool success)
        {
            Errors = errors;
            Success = success;
        }
        public string Errors;
        public bool Success;
    }
}
