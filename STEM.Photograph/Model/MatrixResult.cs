namespace STEM.Photograph.Model
{
    public class MatrixResult
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int N { get; set; }
        public int M { get; set; }
        public int Sum { get; set; }

        public int P => N * M;
    }
}