using System.Diagnostics.CodeAnalysis;

namespace MailSyncer.Infrastructure.HttpClients.Models
{
    public class ResponseWrapper<T> : ResponseWrapper
    {
        public T? Data { get; }

        [MemberNotNullWhen(true, nameof(Data))]
        public bool IsSuccessfulWithData => IsSuccessful && Data != null;

        private ResponseWrapper(bool isSuccess, T? data = default, string? errorMessage = null) : base(isSuccess, errorMessage)
        {
            if (isSuccess && data == null)
                throw new ArgumentNullException(nameof(data), "Data cannot be null for a successful response.");

            Data = data;
        }

        public static ResponseWrapper<T> Success(T data)
        {
            return new ResponseWrapper<T>(true, data);
        }

        public static new ResponseWrapper<T> Fail(string errorMessage)
        {
            return new ResponseWrapper<T>(false, default, errorMessage);
        }
    }
}