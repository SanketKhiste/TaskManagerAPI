namespace TaskManagerAPI.Models
{
    public class ErrorDTO
    {
        //public int StatusCode { get; set; }
        //public string Message { get; set; }
        public int ErrorCode { get; set; } = 0;

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
