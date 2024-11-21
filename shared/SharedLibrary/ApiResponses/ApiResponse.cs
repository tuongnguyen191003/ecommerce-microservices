namespace SharedLibrary.ApiResponses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } // status
        public int Code { get; set; } // status code
        public T? Data { get; set; } // response data
        public string? Message { get; set; } // message data
        public List<string>? ErrorMessages { get; set; } // list error messages

        // default constructor for without data
        public ApiResponse(bool success = false, int code = 200, string? message = null, List<string>? errorMessages = null)
        {
            Success = success;
            Code = code;
            Message = message ?? string.Empty;
            ErrorMessages = errorMessages ?? new List<string>();
        }

        // con
        public ApiResponse(bool success, int code, T? data, string? message = null)
        {
            Success = success;
            Code = code;
            Data = data;
            Message = message ?? string.Empty;
            //ErrorMessages = new List<string>();
        }
    }
}
