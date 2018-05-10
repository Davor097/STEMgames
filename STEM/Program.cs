using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using STEM.Model;
using STEM.Model.Responses;
using STEM.Utility;

namespace STEM
{
    class Program
    {
        private const char F = 'F';
        private const char T = 'T';

        private const string UriBase = "https://tech.stemgames.hr/api/competitive/v1/4292bf95-9793-48b5-9576-daa6d2685e20";
        private const string Authorization = "token VBzFKLfr8jZDUAsjdJJe0lSuMNPCJlxU";

        public static RequestManager Manager = new RequestManager();

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

            string templeValue = testCaseResponse.input.Trim();

            List<Style> styles = new List<Style>
            {
                new Style{Open = '{', Close = '}'},
                new Style{Open = '(', Close = ')'},
                new Style{Open = '[', Close = ']'}
            };

            Temple temple = new Temple(templeValue, styles);

            bool isTampleOk = temple.IsTempleOk();

            var content = isTampleOk ? T : F;

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content.ToString(), Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }
    }
}