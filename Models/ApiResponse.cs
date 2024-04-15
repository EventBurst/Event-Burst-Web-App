namespace Event_Burst_Web_App.Models
{
    public class ApiResponse<T>
    {
        public string StatusCode { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}