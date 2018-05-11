using System;
using System.Linq;
using Newtonsoft.Json;
using STEM.Model.Responses;
using STEM.Utility;

namespace STEM.HeightCount
{
    class Program
    {
        private const string UriBase = "https://tech.stemgames.hr/api/competitive/v1/c396c102-5032-463c-bda9-8cc844f4e54e";
        private const string Authorization = "token VBzFKLfr8jZDUAsjdJJe0lSuMNPCJlxU";

        public static RequestManager Manager = new RequestManager();

        public static int[] Heights;

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    ProcessJob();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private static void ProcessJob()
        {
            string response = Manager.SendGETRequest(UriBase, Authorization);
            TestCaseResponse testCaseResponse = JsonConvert.DeserializeObject<TestCaseResponse>(response);

            //string input = "4\n173.35\n155.53\n173.35\n150.00";

            string[] lines = testCaseResponse.input.Split('\n').ToArray();
            int n = Convert.ToInt32(lines[0]);

            if (n > 60000)
            {
                throw new Exception();
            }

            int max = 15001;
            Heights = new int[max];

            long sum = 0;

            for (int i = 1; i < n + 1; i++)
            {
                int height = (int)Convert.ToDecimal(lines[i]) - 10000;

                Heights[height]++;

                sum += Heights.Skip(height + 1).Sum();
            }

            var content = sum;

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content.ToString(), Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }
    }
}