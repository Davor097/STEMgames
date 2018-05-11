using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using STEM.Model.Responses;
using STEM.Utility;

namespace STEM.HansAndGretta
{
    class Program
    {
        private const string UriBase = "https://tech.stemgames.hr/api/competitive/v1/0626a7d8-c57b-4988-9fc1-89a040b609e1";
        private const string Authorization = "token VBzFKLfr8jZDUAsjdJJe0lSuMNPCJlxU";

        public static RequestManager Manager = new RequestManager();
        public static Dictionary<long, long> F;

        public const long M = 1000000007; // modulo

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

            if (testCaseResponse.input == null)
            {
                return;
            }

            int finNumber = Convert.ToInt32(testCaseResponse.input);

            long sum = 0;

            for (int i = finNumber; i > 0; i--)
            {
                sum += Fibonacci(i * 4 - 1);
            }

            string content = sum.ToString();

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content.ToString(), Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }

        public static int Fibonacci(int n)
        {

            int num = Math.Abs(n);
            if (num == 0)
            {
                return 0;
            }
            else if (num <= 2)
            {
                return 1;
            }

            int[][] number = {new int[] { 1, 1 }, new int[] { 1, 0 }};
            int[][] result = {new int[] { 1, 1 }, new int[] { 1, 0 }};

            while (num > 0)
            {
                if (num % 2 == 1) result = MultiplyMatrix(result, number);
                number = MultiplyMatrix(number, number);
                num /= 2;
            }
            return result[1][1] * ((n < 0) ? -1 : 1);
        }

        public static int[][] MultiplyMatrix(int[][] mat1, int[][] mat2)
        {
            return new int[][] {
                new int[] { mat1[0][0]*mat2[0][0] + mat1[0][1]*mat2[1][0],
                    mat1[0][0]*mat2[0][1] + mat1[0][1]*mat2[1][1] },
                new int[] { mat1[1][0]*mat2[0][0] + mat1[1][1]*mat2[1][0],
                    mat1[1][0]*mat2[0][1] + mat1[1][1]*mat2[1][1] }
            };
        }
    }
}