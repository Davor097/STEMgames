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
            //string response = Manager.SendGETRequest(UriBase, Authorization);
            //TestCaseResponse testCaseResponse = JsonConvert.DeserializeObject<TestCaseResponse>(response);

            //if (testCaseResponse.input == null)
            //{
            //    return;
            //}

            //int finNumber = Convert.ToInt32(testCaseResponse.input);


            long n = 3;
            F[0] = F[1] = 1;

            long rez = 0;
            while (rez > n)
                rez =  (n == 0 ? 0 : f(n - 1));

            long content = rez;


            //response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content.ToString(), Authorization);
            //SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            //Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }

        //public static long Fibonatchi(int n)
        //{

        //    //if (position == 0)
        //    //{
        //    //    return 1;
        //    //}
        //    //if (position == 1)
        //    //{
        //    //    return 1;
        //    //}
        //    //else
        //    //{
        //    //    return Fibonatchi(position - 2) + Fibonatchi(position - 1);
        //    //}

        //    int res = 1;
        //    for (int mod = 1000000007, i = 20; i < n; i++)
        //    {
        //        res *= i; // an obvious step to be done 
        //        if (res > mod) // check if the number exceeds mod
        //            res %= mod; // so as to avoid the modulo as it is costly operation 
        //    }

        //    return res;

        //    //long a = 0;
        //    //    long b = 1;
        //    //    while (n-- > 1)
        //    //    {
        //    //        long t = a;
        //    //        a = b;
        //    //        b += t;
        //    //    }
        //    //    return b % 1000000007;
        //}
        public static long f(long n)
        {
            if (F.Count == n) return F[n];
            long k = n / 2;
            if (n % 2 == 0)
            { // n=2*k
                return F[n] = (f(k) * f(k) + f(k - 1) * f(k - 1)) % M;
            }
            else
            { // n=2*k+1
                return F[n] = (f(k) * f(k + 1) + f(k - 1) * f(k)) % M;
            }
        }
    }
}