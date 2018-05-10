using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using STEM.Model.Responses;
using STEM.Utility;

namespace STEM.Shoutistan
{
    class Program
    {
        private const string UriBase =
            "https://tech.stemgames.hr/api/competitive/v1/d57599bd-4fb3-41eb-b28a-8352b6f37699";

        private const string Authorization = "token VBzFKLfr8jZDUAsjdJJe0lSuMNPCJlxU";

        public static RequestManager Manager = new RequestManager();

        public static Dictionary<int, int> fact;

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

            //int input = Convert.ToInt32(testCaseResponse.input);
            //try
            //{
            //    input ;
            //}
            //catch (Exception e)
            //{
            //    //if (testCaseResponse.submission_id != null)
            //    //{
            //    //    response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", "-1", Authorization);
            //    //    SubmitResponse submitResponse2 = JsonConvert.DeserializeObject<SubmitResponse>(response);
            //    //    Console.WriteLine($"{submitResponse2.status} - {submitResponse2.points_won}");
            //    //}
            //}

            //int[] result = {};

            //switch (input)
            //{
            //    case 0:
            //        result = new[] {0, 1, 2, 3};
            //        break;
            //    case 1:
            //        result = new[] { 5, 6, 7, 8, 9 };
            //        break;
            //    case 2:
            //        result = new[] { 10, 11, 12, 13, 14 };
            //        break;
            //    case 3:
            //        result = new[] { 15, 16, 17, 18, 19 };
            //        break;
            //    case 4:
            //        result = new[] { 20 };
            //        break;
            //}

            var content = "-1";

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content, Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }
    }
}
