namespace EvolApp.Shared.DTOs
{
    public class VerifyCodeRequest
    {
        public string Documento { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
    }
}