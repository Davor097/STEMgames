using System;
using System.Linq;
using Newtonsoft.Json;
using STEM.Model.Responses;
using STEM.Utility;

namespace MathHomework
{
    class Program
    {
        private const string UriBase = "https://tech.stemgames.hr/api/competitive/v1/98bd6b3a-1c22-4cea-9665-17a74b25af0b";
        private const string Authorization = "token VBzFKLfr8jZDUAsjdJJe0lSuMNPCJlxU";

        public static RequestManager Manager = new RequestManager();

        public static int[,] matrix;

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

            string[] lines = testCaseResponse.input.Split('\n').ToArray();

            int n, m;
            int aCost, bCost;
            int aCounter = 0, bCounter = 0;

            n = Convert.ToInt32(lines[0].Split(' ').ToArray()[0]);
            m = Convert.ToInt32(lines[0].Split(' ').ToArray()[1]);
            aCost = Convert.ToInt32(lines[1].Split(' ').ToArray()[0]);
            bCost = Convert.ToInt32(lines[1].Split(' ').ToArray()[1]);

            while (n > m)
            {
                n /= 2;
                bCounter++;
                if (n / 2 < m)
                    break;

            }
            while (n > m)
            {
                n -= 1;
                aCounter++;
            }
            int valueA = aCost * aCounter;
            int valueB = bCost * bCounter;

            int value = valueA + valueB;

            var content = value.ToString();

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content, Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }
    }
}