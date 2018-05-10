using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using STEM.FaultyRocket.Model;
using STEM.Model.Responses;
using STEM.Utility;

namespace STEM.FaultyRocket
{
    class Program
    {
        private const string UriBase = "https://tech.stemgames.hr/api/competitive/v1/fec8307a-488d-4f24-88b5-97f47db80207";
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

            Dictionary<char, Code> dictionary = new Dictionary<char, Code>();

            string[] lines = testCaseResponse.input.Split('\n').ToArray();

            string formula = lines[0];

            for (int i = 1; i < lines.Length; i++)
            {
                string[] lineElements = lines[i].Split(' ').ToArray();
                dictionary.Add(lineElements[0][0], new Code
                {
                    Bits = lineElements[1].ToCharArray().Select(x => int.Parse(x.ToString())).ToArray()
                });
            }

            Code first1 = dictionary[formula[0]];
            Code second1 = dictionary[formula[2]];

            char op1 = formula[1];

            var result = CalculateResult(first1, second1, op1);

            for (int i = 3; i < formula.Length - 1; i = i + 2)
            {
                try
                {
                    Code second = dictionary[formula[i + 1]];

                    char op = formula[i];

                    result = CalculateResult(result, second, op);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }


            string content = string.Join("", result.Bits);

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content, Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }

        private static Code CalculateResult(Code first, Code second, char op)
        {
            for (int i = 31; i >= 0; i--)
            {
                switch (op)
                {
                    case '&':
                        first.Bits[i] = Convert.ToInt32(first.Bits[i] == 1 && second.Bits[i] == 1);
                        break;
                    case '|':
                        first.Bits[i] = Convert.ToInt32(first.Bits[i] == 1 || second.Bits[i] == 1);
                        break;
                    case '^':
                        first.Bits[i] = Convert.ToInt32(!(first.Bits[i] == 1 && second.Bits[i] == 1 || first.Bits[i] == 0 && second.Bits[i] == 0));
                        break;
                }
            }

            return first;
        }
    }
}
