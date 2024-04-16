namespace Event_Burst_Web_App.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string ErrorMessage { get; set; }
    public string ErrorDetails { get; set; }
}