﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using STEM.Model.Responses;
using STEM.Photograph.Model;
using STEM.Utility;

namespace STEM.Photograph
{
    class Program
    {
        private const string UriBase = "https://tech.stemgames.hr/api/competitive/v1/ea95c560-4e7d-47e5-a36a-8bf32c11dd4e";
        private const string Authorization = "token VBzFKLfr8jZDUAsjdJJe0lSuMNPCJlxU";

        public static RequestManager Manager = new RequestManager();

        public static int[,] Matrix;

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

            Matrix = new int[n,n];

            for (int i = 1; i <= n; i++)
            {
                int[] line = lines[i].Split(' ').Select(int.Parse).ToArray();
                for (int j = 0; j < line.Length; j++)
                {
                    Matrix[i, j] = line[j];
                }
            }

            MatrixResult result = new MatrixResult();

            for (int q = n; q >= 1; q--)
            {
                for (int z = n; z >= 1; z--)
                {
                    for (int i = 0; i < n - q; i++)
                    {
                        for (int j = 0; j < n - z; j++)
                        {
                            if (q*z >= result.P)
                            {
                                MatrixResult current = CalculateSum(i, j, q, z);

                                if (current.Sum > result.Sum)
                                {
                                    result = current;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            var content = result.Sum.ToString();

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content, Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }

        private static MatrixResult CalculateSum(int x, int y, int q, int z)
        {
            MatrixResult result = new MatrixResult();
            for (int i = x; i < x + q; i++)
            {
                for (int j = y; j < y + z; j++)
                {
                    if (Matrix[i,j] % 2 == 1)
                    {
                        return new MatrixResult();
                    }
                    result.Sum += Matrix[i, j];
                }
            }
            return result;
        }
    }
}