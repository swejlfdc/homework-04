using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace WordSearch
{
    class WordMatrixEngine
    {
        string[] excuteArray;
        bool[] used;
        int miniSize, bestSize, excuteSize;
        public int[,] matrixA, matrixB;
        public int[,] matrixM;
        int[] dx = new int[4] { 0, 0, 1, -1 };
        int[] dy = new int[4] { 1, -1, 0, 0 };
        public WordMatrixEngine()
        {
        }
        private void Initialization(string[] inArray, int bstSize)
        {
            excuteArray = inArray;
            used = new bool[inArray.Length];
            miniSize = int.MaxValue;//find min value for matrix size
            foreach (string str in excuteArray)
                miniSize = Math.Min(miniSize, str.Length);
            miniSize++;
            this.bestSize = bstSize;//best size now
            miniSize = Math.Min(miniSize, bestSize);
        }//Well down
        private void SplitPosition(ref int x, ref int y, int i, int j)
        {
            if ((i + j) % 2 == 0)
            {
                x = (i + j) / 2;
                y = (j - i) / 2 + (int)Math.Floor((double)(excuteSize - 1) / 2);
            }
            else
            {
                x = (i - j + 1) / 2 + (int)Math.Floor((double)(excuteSize - 2) / 2);
                y = (i + j - 1) / 2;
            }
        }//Well down
        private void SplitMatrix()
        {
            if (matrixA == null || matrixA.GetLength(0) != excuteSize)
                matrixA = new int[excuteSize, excuteSize];
            if (matrixB == null || matrixB.GetLength(0) != excuteSize)
                matrixB = new int[excuteSize, excuteSize];
            for (int i = 0; i != excuteSize; ++i)
                for (int j = 0; j != excuteSize; ++j)
                {
                    matrixA[i, j] = -1;
                    matrixB[i, j] = -1;
                }
            int tmpx = 0, tmpy = 0;
            for (int i = 0; i != excuteSize; ++i)
                for (int j = 0; j != excuteSize; ++j)
                {
                    SplitPosition(ref tmpx, ref tmpy, i, j);
                    if ((i + j) % 2 == 0) matrixA[tmpx, tmpy] = matrixM[i, j];
                    else matrixB[tmpx, tmpy] = matrixM[i, j];
                }
        }//Well down
        private void UnionPosition(ref int x, ref int y, int i, int j, bool isMatrixA)
        {
            if (isMatrixA)
            {
                x = i - j + (int)Math.Floor((double)(excuteSize - 1) / 2);
                y = i + j - (int)Math.Floor((double)(excuteSize - 1) / 2);
            }
            else
            {
                x = i + j - (int)Math.Floor((double)(excuteSize - 2) / 2);
                y = j - i + (int)Math.Floor((double)(excuteSize - 2) / 2) + 1;
            }
        }//Well down
        private void UnionMatrix()
        {
            if (matrixM == null || matrixM.GetLength(0) != excuteSize)
                matrixM = new int[excuteSize, excuteSize];
            int tmpx = 0, tmpy = 0;
            for (int i = 0; i != excuteSize; ++i)
                for (int j = 0; j != excuteSize; ++j)
                {
                    if (matrixA[i, j] != -1)
                    {
                        UnionPosition(ref tmpx, ref tmpy, i, j, true);
                        matrixM[tmpx, tmpy] = matrixA[i, j];
                    }
                    if (matrixB[i, j] != -1)
                    {
                        UnionPosition(ref tmpx, ref tmpy, i, j, false);
                        matrixM[tmpx, tmpy] = matrixB[i, j];
                    }
                }
        }//Well down
        private bool Prepare()
        {
            for (int i=0;i!=used.Length;++i) used[i]=false;
            matrixM = new int[excuteSize, excuteSize];
            for (int i=0;i!=5;++i) used[i]=true;//fill 4 corners and put 1 in the 0.618 area
            matrixM[0, 0] = (int)excuteArray[0][0];
            matrixM[0,excuteSize-1] = (int)excuteArray[1][0];
            matrixM[excuteSize - 1, excuteSize - 1] = (int)excuteArray[2][0]; 
            matrixM[excuteSize-1,0] = (int)excuteArray[3][0];
            for (int i = 0; i != excuteArray[0].Length; ++i)
                if (CheckRange(0, i)) matrixM[0, i] = (int)excuteArray[0][i];//0
                else return false;

            if (matrixM[0, excuteSize - 1] != (int)excuteArray[1][0]) return false;//1
            for (int i = 0; i != excuteArray[1].Length; ++i)
                if (CheckRange(i, excuteSize - 1)) matrixM[i, excuteSize - 1] = (int)excuteArray[1][i];
                else return false;

            if (matrixM[excuteSize - 1, excuteSize - 1] != (int)excuteArray[2][0]) return false; //2
            for (int i = 0; i != excuteArray[2].Length; ++i)
                if (CheckRange(excuteSize - 1, excuteSize - 1 - i)) matrixM[excuteSize - 1, excuteSize - 1 - i] = (int)excuteArray[2][i];
                else return false;

            if (matrixM[excuteSize - 1, 0] != (int)excuteArray[3][0]) return false;//3
            for (int i = 0; i != excuteArray[3].Length; ++i)
                if (CheckRange(excuteSize - 1 - i, 0)) matrixM[excuteSize - 1 - i, 0] = (int)excuteArray[3][i];
                else return false;

            if (matrixM[0, 0] != (int)excuteArray[0][0]) return false;//0
            
            int tx = (excuteSize - excuteArray[4].Length) / 2;//Put one in the middle
            int ty = (int)(excuteSize * 0.618);
            if (tx > 0) tx--;
            if (tx < 0) tx = 0;
            while (CheckRange(tx + 1, ty) && matrixM[tx, ty] != 0 && matrixM[tx, ty] != excuteArray[4][0]) ++tx;
            for (int i = 0; i != excuteArray[4].Length; ++i) 
                if (CheckRange(tx+i,ty) && (matrixM[tx+i,ty]==0 || matrixM[tx+i,ty]==excuteArray[4][i]))
                    matrixM[tx + i, ty] = (int)excuteArray[4][i];
                else return false;
            return true;
        }//Well down
        private bool CheckRange(int x, int y)
        {
            return !(x < 0 || y < 0 || x >= excuteSize || y >= excuteSize);
        }//Well down
        private bool CheckFill(int x, int y, string str, ref int[,] matrix, ref int mode,ref double estVal)
        {
            int tmpx=x,tmpy=y,iter,tmpVal,tmpNum;
            estVal = 0;
            for (int i = 0; i != 4; ++i)//4 directions
            {
                int tx = dx[i], ty = dy[i];
                tmpx = x;
                tmpy = y;
                tmpNum = 0;
                for (iter = 0; iter != str.Length; ++iter)
                {
                    if (!CheckRange(tmpx, tmpy)) break;
                    if (matrix[tmpx, tmpy] != 0 && matrix[tmpx, tmpy] != (int)str[iter]) break;
                    if (matrix[tmpx, tmpy] == (int)str[iter]) ++tmpNum;
                    tmpx += tx;
                    tmpy += ty;
                }
                if (iter != str.Length) continue;
                tmpx += tx;
                tmpy += ty;
                tmpVal = 0;
                while (CheckRange(tmpx,tmpy) && matrix[tmpx, tmpy] == 0)
                {
                    tmpVal++;
                    tmpx += tx;
                    tmpy += ty;
                }
                tmpx = x - tx;
                tmpy = y - ty;
                while (CheckRange(tmpx,tmpy) && matrix[tmpx, tmpy] == 0)
                {
                    tmpVal++;
                    tmpx -= tx;
                    tmpy -= ty;
                }
                if ((double)(tmpNum*6.18+1)/(tmpVal+1)>estVal)
                {
                    estVal = (double)(tmpNum*6.18 + 1) / (tmpVal + 1);
                    mode = i;
                }
            }
            return (estVal!=0);
        }
        private void FillWords(ref int[,] matrix)
        {
            int bestx=0, besty=0, bestmode=0, tmpmode;
            double bestVal, tmpVal;
            for (int itStr1 = 0; itStr1 != used.Length; ++itStr1)
            {
                if (used[itStr1]) continue;
                tmpmode = 0;
                bestVal = 0;
                for (int i = 0; i != excuteSize; ++i)
                    for (int j = 0; j != excuteSize; ++j)
                    {
                        tmpVal = 0;
                        if (!CheckFill(i, j, excuteArray[itStr1], ref matrix, ref tmpmode, ref tmpVal)) continue;
                        if (bestVal < tmpVal)
                        {
                            bestVal = tmpVal;
                            bestmode = tmpmode;
                            bestx = i;
                            besty = j;
                        }
                    }
                if (bestVal != 0)
                {
                    for (int k = 0; k != excuteArray[itStr1].Length; ++k)
                    {
                        matrix[bestx, besty] = (int)excuteArray[itStr1][k];
                        bestx += dx[bestmode];
                        besty += dy[bestmode];
                    }
                    used[itStr1] = true;
                }
            }
        }
        private void FillLetters(ref int[,] matrix)
        {
            Random rng = new Random();
            for(int i = 0; i < matrix.GetLength(0); ++i)
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    if (matrix[i, j] == 0)
                    {
                        for(matrix[i,j] = 'A' + rng.Next(26); 
                        (new int[]{'A', 'O', 'E', 'U', 'N', 'B', 'C', 'D', 'G', 'K', 'L', 'M', 'P', 'S', 'T'}).Contains(matrix[i,j]);
                        matrix[i,j] = 'A' + rng.Next(26));
                    }
                }
        }
        private bool Search()
        {
            if (!Prepare()) return false;
            SplitMatrix();
            FillWords(ref matrixA);
            FillWords(ref matrixB);
            UnionMatrix();
            FillWords(ref matrixM);
            if (used.Contains(false)) return false;
            //FillLetters(ref matrixM);
            return true;
        }
        public bool Run(string[] inArray, int bstSize)
        {
            Initialization(inArray, bstSize);
            excuteSize = bestSize;
            if (!Search()) return false;
            do
            {
                excuteSize = (miniSize + bestSize) / 2;
                if (Search()) bestSize = excuteSize;
                else miniSize = excuteSize;
                if (miniSize + 1 == bestSize)
                {
                    excuteSize = miniSize;
                    if (Search()) bestSize = excuteSize;
                    else break;
                }
            } while (bestSize != miniSize);
            excuteSize = bestSize;
            Search();
            return true;
        }
        private void _DebugMatrixTransform(int msize)
        {
            this.excuteSize = msize;
            matrixM = new int[msize, msize];
            for (int i = 0; i != msize; ++i)
                for (int j = 0; j != msize; ++j)
                    matrixM[i, j] = (i + j) % 2;
            SplitMatrix();
            for (int i = 0; i != msize; ++i)
                for (int j = 0; j != msize; ++j)
                {
                    if (matrixA[i, j] != -1) matrixA[i, j] = 1;
                    if (matrixB[i, j] != -1) matrixB[i, j] = 0;
                }
            UnionMatrix();
        }
        private void _DebugPrepare()
        {
            excuteArray = new string[5];
            excuteArray[0] = "ABCD";
            excuteArray[1] = "EFGH";
            excuteArray[2] = "IJKL";
            excuteArray[3] = "MNOP";
            excuteArray[4] = "QRST";
            Initialization(excuteArray, 10);
            excuteSize = 6;
            bool rect = Prepare();
        }
        private void _DebugRun(int msize)
        {
            string[] excuteArray = new string[7];
            excuteArray[0] = "ABCD";
            excuteArray[1] = "EFGH";
            excuteArray[2] = "IJKL";
            excuteArray[3] = "MNOP";
            excuteArray[4] = "QRST";
            excuteArray[5] = "PQQRNG";
            excuteArray[6] = "CQBTJ";
            this.Run(excuteArray, msize);
        }
        public void Debug(int msize)
        {
            //_DebugMatrixTransform(miniSize);
            //_DebugPrepare();
            _DebugRun(6);
        }

    }
}
