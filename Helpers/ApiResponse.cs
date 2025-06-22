namespace DLARS.Helpers
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ApiResponse SuccessMessage(string message)
            => new() { Success = true, Message = message };

        public static ApiResponse FailMessage(string message)
            => new() { Success = false, Message = message };
    }
}
