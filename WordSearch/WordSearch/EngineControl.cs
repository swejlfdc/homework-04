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
                str = str.Replace(" ", "");
                words.Add(str);
                if (str.Count() > maxLen) maxLen = str.Count();
            }
            bool[] fg = new bool[words.Count];
            for(int i = 0; i < words.Count; ++i)
                for(int j = 0; j < words.Count; ++j) 
                    if(i != j && words[i].Contains(words[j])) {
                        fg[j] = true;
                    }
            for(int i = words.Count - 1; i >= 0; --i) 
                if(fg[i]) words.RemoveAt(i);
            WordMatrixEngine engine = new WordMatrixEngine();
            words.Sort(new comp());
            for (int i = 17; i != 30; ++i)
            {
                if (!engine.Run(words.ToArray(), i)) continue;
                if (resultMatrix == null || resultMatrix.GetLength(0) > engine.matrixM.GetLength(0))
                    resultMatrix = (int[,])engine.matrixM.Clone();
            }
            /*int maxLmt = 200000 / (words.Count * words.Count);
            for (int i = 0; i < maxLmt; ++i)
            {
                Shuffle(words);
                engine.Run(words.ToArray(), maxLen * 2);
                if (resultMatrix == null || resultMatrix.GetLength(0) > engine.matrixM.GetLength(0))
                    resultMatrix = (int[,])engine.matrixM.Clone();
            }*/
            engine.excuteSize = resultMatrix.GetLength(0);
            bool tmp = engine.CheckMatrix(ref resultMatrix);
            Console.WriteLine("hang: {0}, bool: {1}", resultMatrix.GetLength(0), tmp.ToString());
            StreamWriter sw = new StreamWriter("11061215_result.txt");
            for (int i = 0; i < resultMatrix.GetLength(0); ++i)
            {
                for (int j = 0; j < resultMatrix.GetLength(1); ++j)
                {
                    sw.Write(((char)resultMatrix[i, j]).ToString());
                }
                sw.Write('\n');
            }
            sw.Close();
        }
    }
}
