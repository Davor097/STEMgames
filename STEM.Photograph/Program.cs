﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using STEM.Model.Responses;
using STEM.Utility;

namespace STEM.Photograph
{
    class Program
    {
        private const string UriBase = "https://tech.stemgames.hr/api/competitive/v1/33602146-07d7-4083-b18e-4c0eb25ce7b9";
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

            int n = Convert.ToInt32(lines[0]);

            matrix = new int[n,n];

            for (int i = 1; i <= n; i++)
            {
                int[] line = lines[i].Split(' ').Select(int.Parse).ToArray();
                for (int j = 0; j < line.Length; j++)
                {
                    matrix[i, j] = line[j];
                }
            }

            int sumMax = 0;

            for (int q = n; q >= 1; q--)
            {
                for (int z = n; z >= 1; z--)
                {
                    for (int i = 0; i < n - q; i++)
                    {
                        for (int j = 0; j < n - z; j++)
                        {
                            int sum = CalculateSum(i, j, q, z);

                            if (sum > sumMax)
                            {
                                sumMax = sum;
                            }
                        }
                    }
                }
            }

            var content = sumMax.ToString();

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content, Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }

        private static int CalculateSum(int x, int y, int q, int z)
        {
            int sum = 0;
            for (int i = x; i < x + q; i++)
            {
                for (int j = y; j < y + z; j++)
                {
                    if (matrix[i,j] % 2 == 1)
                    {
                        return 0;
                    }
                    sum += matrix[i, j];
                }
            }
            return sum;
        }
    }
}