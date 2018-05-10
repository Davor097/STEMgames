using System;
using System.Collections.Generic;
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

        public static SortedDictionary<int, int> heights;

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


            //string dasd = "4\n173.35\n155.53\n173.35\n150.00";

            heights = new SortedDictionary<int, int>();

            string[] lines = testCaseResponse.input.Split('\n').ToArray();
            int n = Convert.ToInt32(lines[0]);

            if (n > 5000)
            {
                throw new Exception();
            }

            //List<decimal> peoples = new List<decimal>();



            int sum = 0;

            for (int i = 1; i < n + 1; i++)
            {
                int height = (int)Convert.ToDecimal(lines[i]) - 10000;

                if (heights.ContainsKey(height))
                {
                    heights[height] += 1;
                }
                else
                {
                    heights.Add(height, 1);
                }

                sum += heights.Where(x => x.Key > height).Sum(x => x.Value);
            }

            //foreach (var people in peoples)
            //{
            //    heights[people] += 1;
            //    sum += heights.Where(x => x.Key > people).Sum(x => x.Value);
            //}

            var content = sum;

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content.ToString(), Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }
    }
}