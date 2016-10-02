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
        public int GetHash(Stream stream)
        {
            UInt32[] X = CreateAndPreprocessUInt32ArrayFromStream(stream);

            //initialization of vars and consts
            UInt32 A, AA, B, BB, C, CC, D, DD;
            A = 0x67452301; B = 0xEFCDAB89; C = 0x98BADCFE; D = 0x10325476;

            UInt32[] T = new UInt32[64];
            for (int i = 0; i < T.Length; i++)
            {
                T[i] = (UInt32)(Math.Pow(2, 64) * Math.Abs(Math.Sin(i + 1)));
            }

            for (long i = 0; i < X.LongLength / 16; i++) // over the blocks of 512 bits (512/32=16)
            {
                AA = A; BB = B; CC = C; DD = D;
                // [abcd k s i] := a = b + ((a+Func(b,c,d)+X[i*16+k] + T[i]).RotateLeftCircularly(s);
                int h = 0;
                //==========================round 1=====================================
                long k; // X indexer, different depending on the round
                for (int j = 0; j < 16; j++)
                {
                    k = j;
                    switch (j % 4)
                    {
                        case 0:
                            {
                                A = B + ((A + FuncF(B, C, D)) + X[k+i*16] + T[h++]).ShiftLeftCircularly(7);
                                break;
                            }
                        case 1:
                            {
                                D = A + ((D + FuncF(A, B, C)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(12);
                                break;
                            }
                        case 2:
                            {
                                C = D + ((C + FuncF(D, A, B)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(17);
                                break;
                            }
                        case 3:
                            {
                                B = C + ((B + FuncF(C, D, A)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(22);
                                break;
                            }
                    }

                }

                //==========================round 2=====================================
                k = -4;
                for (int j = 0; j < 16; j++)
                {
                    k = (k + 5) % 16;
                    switch (j % 4)
                    {
                        case 0:
                            {
                                A = B + ((A + FuncG(B, C, D)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(5);
                                break;
                            }
                        case 1:
                            {
                                D = A + ((D + FuncG(A, B, C)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(9);
                                break;
                            }
                        case 2:
                            {
                                C = D + ((C + FuncG(D, A, B)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(14);
                                break;
                            }
                        case 3:
                            {
                                B = C + ((B + FuncG(C, D, A)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(20);
                                break;
                            }
                    }

                }

                //==========================round 3=====================================
                k = 2;
                for (int j = 0; j < 16; j++)
                {
                    k = (k + 3) % 16;
                    switch (j % 4)
                    {
                        case 0:
                            {
                                A = B + ((A + FuncH(B, C, D)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(5);
                                break;
                            }
                        case 1:
                            {
                                D = A + ((D + FuncH(A, B, C)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(9);
                                break;
                            }
                        case 2:
                            {
                                C = D + ((C + FuncH(D, A, B)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(14);
                                break;
                            }
                        case 3:
                            {
                                B = C + ((B + FuncH(C, D, A)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(20);
                                break;
                            }
                    }

                }

                //==========================round 4=====================================
                k = -7;
                for (int j = 0; j < 16; j++)
                {
                    k = (k + 7) % 16;
                    switch (j % 4)
                    {
                        case 0:
                            {
                                A = B + ((A + FuncI(B, C, D)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(6);
                                break;
                            }
                        case 1:
                            {
                                D = A + ((D + FuncI(A, B, C)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(10);
                                break;
                            }
                        case 2:
                            {
                                C = D + ((C + FuncI(D, A, B)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(15);
                                break;
                            }
                        case 3:
                            {
                                B = C + ((B + FuncI(C, D, A)) + X[k + i * 16] + T[h++]).ShiftLeftCircularly(21);
                                break;
                            }
                    }

                }


                //adding BBs etc
                A += AA;
                B += BB;
                C += CC;
                D += DD;


            }

            return 0;
        }

        UInt32[] CreateAndPreprocessUInt32ArrayFromStream(Stream stream)
        {
            uint numOfPaddingBytes = (120 - (uint)((stream.Length) % 64)) % 64;
            UInt32[] words = new UInt32[(stream.Length + numOfPaddingBytes) / 4];


            int i = 0;
            int currentByte = stream.ReadByte();
            while (currentByte != -1)
            {
                words[i / 4] += (uint)currentByte << ((3 - i++ % 4) * 8); //this magical line creates a uint array from a byte stream byte by byte
                currentByte = stream.ReadByte();
            }
            words[i / 4] += (uint)0x80 << ((3 - i++ % 4) * 8);

            //appending the length of the stream. Might be in wrong order
            words[words.LongLength - 2] = (uint)(stream.Length & 0xFFFFFFFF);
            words[words.LongLength - 1] = (uint)((stream.Length >> 32) & 0xFFFFFFFF);

            return words;

        }


        UInt32 FuncF(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & y) | (~x & z);
        }
        UInt32 FuncG(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & z) | (~z & y);
        }
        UInt32 FuncH(UInt32 x, UInt32 y, UInt32 z)
        {
            return x ^ y ^ z;
        }
        UInt32 FuncI(UInt32 x, UInt32 y, UInt32 z)
        {
            return y ^ (~z | x);
        }

    }
}
