namespace marusa_line.Dtos
{
    public class GoogleSettings
    {
        public string? Client_Id { get; set; }
        public string? Client_Secret { get; set; }
        public string? Project_Id { get; set; }
        public string? Auth_Uri { get; set; }
        public string? Token_Uri { get; set; }
        public string? Auth_Provider_X509_Cert_Url { get; set; }
        public List<string>? Redirect_Uris { get; set; }
    }

}
