namespace TaskManagerAPI.Models.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; }

        public object ResponseObject { get; set; }

        public string Message { get; set; }

        public List<ErrorDTO> ErrorDTOs { get; set; } = new();
    }
}
