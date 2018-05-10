using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using STEM.Model.Responses;
using STEM.Utility;

namespace STEM.TheOrder
{
    class Program
    {
        private const string UriBase = "https://tech.stemgames.hr/api/competitive/v1/87b68bba-8273-4ce2-b588-31d8e441d769";
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

            string[] lines = testCaseResponse.input.Split('\n').ToArray();
            string input = lines[0];
            Console.WriteLine(input + "- " + input.Length);
            //if (input.Length > 8)
            //{
            //    throw new Exception();
            //}
            long index = Convert.ToInt64(lines[1]);

            //var permutations = GetPermutations(index, input.ToCharArray().OrderBy(x => x).ToArray()).Select(x => string.Join("", x)).OrderBy(x => x).ToList();

            var permutation = Encoding.ASCII.GetBytes(input).OrderBy(x => x).Select(x => (int)x).ToArray();
            for (long i = 0; i < index; i++)
            {
                permutation = NextPermutation(permutation);
            }

            var content = string.Join("", permutation.Select(x => (char)x).ToArray());

            response = Manager.SendPOSTRequest($"{UriBase}/{testCaseResponse.submission_id}", content.ToString(), Authorization);
            SubmitResponse submitResponse = JsonConvert.DeserializeObject<SubmitResponse>(response);
            Console.WriteLine($"{submitResponse.status} - {submitResponse.points_won}");
        }

        public static IEnumerable<IEnumerable<T>>GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new[] { t });
            
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new[] { t2 }));
        }

        public static int[] NextPermutation(int[] array)
        {
            // Find non-increasing suffix
            int i = array.Length - 1;
            while (i > 0 && array[i - 1] >= array[i])
                i--;
            if (i <= 0)
                return new int[0];

            // Find successor to pivot
            int j = array.Length - 1;
            while (array[j] <= array[i - 1])
                j--;
            int temp = array[i - 1];
            array[i - 1] = array[j];
            array[j] = temp;

            // Reverse suffix
            j = array.Length - 1;
            while (i < j)
            {
                temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }
            return array;
        }
    }
}