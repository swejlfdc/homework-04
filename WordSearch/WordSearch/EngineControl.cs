using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordSearch
{
    public class comp : IComparer<string>
    {
        public int Compare(string a, string b)
        {
            if (a.Length > b.Length) return -1;
            if (a.Length == b.Length) return 0;
            return 1;
        }
    }
    static class EngineControl
    {
        public static int[,] resultMatrix;
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static void Run()
        {
            string filename = Environment.GetCommandLineArgs()[1];
            StreamReader file = new StreamReader(filename);
            List<string> words = new List<string>();
            int maxLen = 0;
            for (string str; (str = file.ReadLine()) != null; )
            {
                str = str.ToUpper();
                words.Add(str);
                if (str.Count() > maxLen) maxLen = str.Count();
            }
            WordMatrixEngine engine = new WordMatrixEngine();
            words.Sort(new comp());
            engine.Run(words.ToArray(), maxLen * 2);
            if (resultMatrix == null || resultMatrix.GetLength(0) > engine.matrixM.GetLength(0))
                resultMatrix = (int[,])engine.matrixM.Clone();
            int maxLmt = 200000 / (words.Count * words.Count);
            for (int i = 0; i < maxLmt; ++i)
            {
                Shuffle(words);
                engine.Run(words.ToArray(), maxLen * 2);
                if (resultMatrix == null || resultMatrix.GetLength(0) > engine.matrixM.GetLength(0))
                    resultMatrix = (int[,])engine.matrixM.Clone();
            }
            Console.WriteLine("hang: {0}", resultMatrix.GetLength(0));
        }
    }
}
