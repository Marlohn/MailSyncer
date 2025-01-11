namespace MailSyncer.Infrastructure.HttpClients.Models
{
    public class ResponseWrapper
    {
        public bool IsSuccessful { get; }
        public string? ErrorMessage { get; }

        protected ResponseWrapper(bool isSuccess, string? errorMessage = null)
        {
            if (!isSuccess && string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Error message cannot be null or empty for a failed response.", nameof(errorMessage));

            IsSuccessful = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static ResponseWrapper Success()
        {
            return new ResponseWrapper(true);
        }

        public static ResponseWrapper Fail(string errorMessage)
        {
            return new ResponseWrapper(false, errorMessage);
        }
    }
}