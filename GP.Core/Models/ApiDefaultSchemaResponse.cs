using AutoWrapper.Wrappers;

namespace GP.Core.Models
{
    public class DefaultApiSchemaResponse<T> : ApiResponse
    {
        public new T? Result { get; set; }
    }
    public class DefaultApiSchemaResponse : ApiResponse
    {
    }
}
