namespace STEM.Model.Responses
{
    public class TestCaseResponse
    {
        public string submission_id { get; set; }
        public string status { get; set; }
        public decimal points_if_correct { get; set; }
        public string input { get; set; }
    }
}