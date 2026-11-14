namespace _01_MVC_Giris.Models
{
    // Model verilerin şeklini temsil eder
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
