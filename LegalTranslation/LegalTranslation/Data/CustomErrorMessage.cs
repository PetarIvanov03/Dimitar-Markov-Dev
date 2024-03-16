namespace LegalTranslation.Data
{
    public class CustomErrorMessage
    {
        public CustomErrorMessage(bool check, string message)
        {
            this.Check = check;
            this.Message = message;
        }
        public bool Check { get; set; }
        public string Message { get; set; }
    }
}
