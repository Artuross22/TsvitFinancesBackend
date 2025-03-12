namespace FinancialData;

public class ApiSettings
{
    public required _FPM FPM { get; set; }

    public class _FPM
    {
        public required string Key { get; set; }

        public required string BaseUrl { get; set; }
    }
}