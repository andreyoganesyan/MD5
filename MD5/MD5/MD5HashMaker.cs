using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MD5
{
    class MD5HashMaker
    {
        public string GetHash(Stream stream)
        {
            UInt32[] X = CreateAndPreprocessUInt32ArrayFromStream(stream);
            stream.Position = 0;
            //initialization of vars and consts
            UInt32 A, B, C, D;
            A = 0x67452301; B = 0xEFCDAB89; C = 0x98BADCFE; D = 0x10325476;

            PerformTransformations(X, ref A, ref B, ref C, ref D);

            return HashToString(A, B, C, D);
        }

        void PerformTransformations(UInt32[] X, ref UInt32 A, ref UInt32 B, ref UInt32 C, ref UInt32 D)
        {
        

        int[] s = new int[]
        {
        7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
        5,  9, 14, 20, 5,  9, 14, 20, 5,  9, 14, 20, 5,  9, 14, 20,
        4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
        6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        };

        UInt32[] T = new UInt32[64];
            for (int i = 0; i < T.Length; i++)
            {
                T[i] = (UInt32)(Math.Pow(2, 32) * Math.Abs(Math.Sin(i + 1)));
            }

            for (long i = 0; i < X.LongLength / 16; i++) //over X.LongLength/16 blocks
            {


                UInt32 AA = A, BB = B, CC = C, DD = D;
                int ti = 0; //index in T Array
                //ROUND 1
                #region
                {
                    int xi = 0; //index of word in current block
                    for (int j = 0; j < 4; j++)
                    {
                        /* [abcd k s i] a = b + ((a + F(b,c,d) + X[k] + T[i]) <<< s). */
                        /*[ABCD  0 7  1][DABC  1 12  2][CDAB  2 17  3][BCDA  3 22  4]
                          [ABCD  4 7  5][DABC  5 12  6][CDAB  6 17  7][BCDA  7 22  8]
                          [ABCD  8 7  9][DABC  9 12 10][CDAB 10 17 11][BCDA 11 22 12]
                          [ABCD 12 7 13][DABC 13 12 14][CDAB 14 17 15][BCDA 15 22 16]*/

                        A = B + ((A + FuncF(B, C, D) + X[i * 16 + (xi++)] + T[ti++]).ShiftLeftCircularly(7));
                        D = A + ((D + FuncF(A, B, C) + X[i * 16 + (xi++)] + T[ti++]).ShiftLeftCircularly(12));
                        C = D + ((C + FuncF(D, A, B) + X[i * 16 + (xi++)] + T[ti++]).ShiftLeftCircularly(17));
                        B = C + ((B + FuncF(C, D, A) + X[i * 16 + (xi++)] + T[ti++]).ShiftLeftCircularly(22));
                    }
                }
                #endregion

                //ROUND 2
                #region
                {
                    int xi = -4; //index of word in current block
                    for (int j = 0; j < 4; j++)
                    {
                        /* [abcd k s i] a = b + ((a + G(b,c,d) + X[k] + T[i]) <<< s). 
                           [ABCD  1 5 17][DABC  6 9 18][CDAB 11 14 19][BCDA  0 20 20]
                           [ABCD  5 5 21][DABC 10 9 22][CDAB 15 14 23][BCDA  4 20 24]
                           [ABCD  9 5 25][DABC 14 9 26][CDAB  3 14 27][BCDA  8 20 28]
                           [ABCD 13 5 29][DABC  2 9 30][CDAB  7 14 31][BCDA 12 20 32]*/

                        A = B + ((A + FuncG(B, C, D) + X[i * 16 + (xi = (xi + 5) % 16)] + T[ti++]).ShiftLeftCircularly(5));
                        D = A + ((D + FuncG(A, B, C) + X[i * 16 + (xi = (xi + 5) % 16)] + T[ti++]).ShiftLeftCircularly(9));
                        C = D + ((C + FuncG(D, A, B) + X[i * 16 + (xi = (xi + 5) % 16)] + T[ti++]).ShiftLeftCircularly(14));
                        B = C + ((B + FuncG(C, D, A) + X[i * 16 + (xi = (xi + 5) % 16)] + T[ti++]).ShiftLeftCircularly(20));
                    }
                }
                #endregion

                //ROUND 3
                #region
                {
                    int xi = 2; //index of word in current block
                    for (int j = 0; j < 4; j++)
                    {
                        /*[abcd k s i] a = b + ((a + H(b,c,d) + X[k] + T[i]) <<< s).
                          [ABCD  5 4 33][DABC  8 11 34][CDAB 11 16 35][BCDA 14 23 36]
                          [ABCD  1 4 37][DABC  4 11 38][CDAB  7 16 39][BCDA 10 23 40]
                          [ABCD 13 4 41][DABC  0 11 42][CDAB  3 16 43][BCDA  6 23 44]
                          [ABCD  9 4 45][DABC 12 11 46][CDAB 15 16 47][BCDA  2 23 48]*/

                        A = B + ((A + FuncH(B, C, D) + X[i * 16 + (xi = (xi + 3) % 16)] + T[ti++]).ShiftLeftCircularly(4));
                        D = A + ((D + FuncH(A, B, C) + X[i * 16 + (xi = (xi + 3) % 16)] + T[ti++]).ShiftLeftCircularly(11));
                        C = D + ((C + FuncH(D, A, B) + X[i * 16 + (xi = (xi + 3) % 16)] + T[ti++]).ShiftLeftCircularly(16));
                        B = C + ((B + FuncH(C, D, A) + X[i * 16 + (xi = (xi + 3) % 16)] + T[ti++]).ShiftLeftCircularly(23));
                    }
                }
                #endregion

                //ROUND 4
                #region
                {
                    int xi = -7; //index of word in current block
                    for (int j = 0; j < 4; j++)
                    {
                        /* [abcd k s i] a = b + ((a + I(b,c,d) + X[k] + T[i]) <<< s). 
                           [ABCD  0 6 49][DABC  7 10 50][CDAB 14 15 51][BCDA  5 21 52]
                           [ABCD 12 6 53][DABC  3 10 54][CDAB 10 15 55][BCDA  1 21 56]
                           [ABCD  8 6 57][DABC 15 10 58][CDAB  6 15 59][BCDA 13 21 60]
                           [ABCD  4 6 61][DABC 11 10 62][CDAB  2 15 63][BCDA  9 21 64]*/

                        A = B + ((A + FuncI(B, C, D) + X[i * 16 + (xi = (xi + 7) % 16)] + T[ti++]).ShiftLeftCircularly(6));
                        D = A + ((D + FuncI(A, B, C) + X[i * 16 + (xi = (xi + 7) % 16)] + T[ti++]).ShiftLeftCircularly(10));
                        C = D + ((C + FuncI(D, A, B) + X[i * 16 + (xi = (xi + 7) % 16)] + T[ti++]).ShiftLeftCircularly(15));
                        B = C + ((B + FuncI(C, D, A) + X[i * 16 + (xi = (xi + 7) % 16)] + T[ti++]).ShiftLeftCircularly(21));
                    }
                }
                #endregion

                A += AA;
                B += BB;
                D += DD;
                C += CC;
            }


        }



        string HashToString(UInt32 A, UInt32 B, UInt32 C, UInt32 D)
        {
            string output = "";
            for (int i = 0; i < 4; i++)
            {
                output += ((byte)(A >> i * 8)).ToString("X2");
            }
            for (int i = 0; i < 4; i++)
            {
                output += ((byte)(B >> i * 8)).ToString("X2");
            }
            for (int i = 0; i < 4; i++)
            {
                output += ((byte)(C >> i * 8)).ToString("X2");
            }
            for (int i = 0; i < 4; i++)
            {
                output += ((byte)(D >> i * 8)).ToString("X2");
            }
            return output;
        }
        UInt32[] CreateAndPreprocessUInt32ArrayFromStream(Stream stream)
        {


            uint numOfPaddingBytes = (uint)(64 - stream.Length % 64);
            UInt32[] words = new UInt32[(stream.Length + numOfPaddingBytes) / 4];


            int i = 0;
            int currentByte = stream.ReadByte();
            while (currentByte != -1)
            {
                words[i / 4] += (uint)currentByte << ((i++ % 4) * 8); //this magical line creates a uint array from a byte stream byte by byte
                currentByte = stream.ReadByte();
            }
            words[i / 4] += (uint)0x80 << ((i++ % 4) * 8);

            //appending the length of the stream
            words[words.LongLength - 2] = (uint)((stream.Length * 8) & 0xFFFFFFFF);
            words[words.LongLength - 1] = (uint)(((stream.Length * 8) >> 32) & 0xFFFFFFFF);
            return words;

        }


        UInt32 FuncF(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & y) | (~x & z);
        }
        UInt32 FuncG(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & z) | ((~z) & y);
        }
        UInt32 FuncH(UInt32 x, UInt32 y, UInt32 z)
        {
            return x ^ y ^ z;
        }
        UInt32 FuncI(UInt32 x, UInt32 y, UInt32 z)
        {
            return y ^ ((~z) | x);
        }


    }
}
