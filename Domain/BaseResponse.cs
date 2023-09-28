using Domain.Enum;

namespace Domain
{
    public class BaseResponse<T>
    {
        public Status Status { get; set; }        
        public T Data { get; set; }
    }
}