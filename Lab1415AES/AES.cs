using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1415AES
{
    public class AES
    {
        private static byte[,] cipherKey = new byte[4, 4], state = new byte[4, 4];
        private static List<byte[,]> RoundKey = new List<byte[,]>();
        private static List<byte[,]> states = new List<byte[,]>();
        private static List<byte[]> result = new List<byte[]>();
        private static int Round = 0, countOfStates = 0, lengthOfStates = 0;

        private static byte[,] sBox = {
            { 0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76 },
            { 0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0 },
            { 0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15 },
            { 0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75 },
            { 0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84 },
            { 0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf },
            { 0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8 },
            { 0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2 },
            { 0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73 },
            { 0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb },
            { 0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79 },
            { 0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08 },
            { 0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a },
            { 0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e },
            { 0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf },
            { 0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16 } };

        private static byte[,] invSBox = {
            { 0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb },
            { 0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb },
            { 0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e },
            { 0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25 },
            { 0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92 },
            { 0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84 },
            { 0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06 },
            { 0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b },
            { 0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73 },
            { 0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e },
            { 0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b },
            { 0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4 },
            { 0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f },
            { 0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef },
            { 0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61 },
            { 0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d } };

        private static byte[,] rcon = {
            {0x00, 0x00, 0x00, 0x00},
            {0x01, 0x00, 0x00, 0x00},
            {0x02, 0x00, 0x00, 0x00},
            {0x04, 0x00, 0x00, 0x00},
            {0x08, 0x00, 0x00, 0x00},
            {0x10, 0x00, 0x00, 0x00},
            {0x20, 0x00, 0x00, 0x00},
            {0x40, 0x00, 0x00, 0x00},
            {0x80, 0x00, 0x00, 0x00},
            {0x1b, 0x00, 0x00, 0x00},
            {0x36, 0x00, 0x00, 0x00} };

        private static byte[,] gf = { { 2, 3, 1, 1 }, { 1, 2, 3, 1 }, { 1, 1, 2, 3 }, { 3, 1, 1, 2 } };

        private static byte[,] invgf = { { 14, 11, 13, 9 }, { 9, 14, 11, 13 }, { 13, 9, 14, 11 }, { 11, 13, 9, 14 } };



        public AES(byte[,] _cipherKey)
        {
            cipherKey = new byte[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    cipherKey[i, j] = _cipherKey[i, j];
        }


        /// <summary>
        /// String encrypt to byte array
        /// </summary>
        /// <param name="source">String to encrypt</param>
        /// <returns></returns>
        public byte[] Encrypt(string source)
        {
            result.Clear();
            GetAllStates(source);
            for (int i = 0; i < countOfStates; i++)
            {
                Round = 0;

                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        state[j, k] = states[i][j, k];

                KeySchedule();
                AddRoundKey();

                for (int j = 0; j < 9; j++)
                {
                    Round = j + 1;
                    SubBytes();
                    ShiftRows();
                    MixColumns();
                    AddRoundKey();
                }

                Round++;
                SubBytes();
                ShiftRows();
                AddRoundKey();

                result.Add(ByteBlockToArray(state));
            }

            return ByteListToArray(result);
        }

        /// <summary>
        /// Byte array decrypt to string
        /// </summary>
        /// <param name="source">Byte array to decrypt</param>
        /// <returns></returns>
        public string Decrypt(byte[] source)
        {
            result.Clear();
            GetAllStates(source);
            for (int i = 0; i < countOfStates; i++)
            {
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        state[j, k] = states[i][j, k];

                Round = 10;
                KeySchedule();
                AddRoundKey();

                for (int j = 0; j < 9; j++)
                {
                    Round--;
                    InvShiftRows();
                    InvSubBytes();
                    AddRoundKey();
                    InvMixColumns();
                }

                Round--;
                InvShiftRows();
                InvSubBytes();
                AddRoundKey();

                result.Add(ByteBlockToArray(state));
            }

            return ByteListToString(result);
        }

        /// <summary>
        /// Generate random cipher key
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateKey()
        {
            byte[] gkey = new byte[16];
            Random rand = new Random();

            for (int i = 0; i < 16; i++)
                gkey[i] = (byte)rand.Next(0, 256);

            return gkey;
        }


        /// <summary>
        /// Get byte array from string and generate states list
        /// </summary>
        private static void GetAllStates(string source)
        {
            states.Clear();

            byte[] allBytes = UTF8Encoding.UTF8.GetBytes(source);
            countOfStates = (allBytes.Length / 16) + 1;
            lengthOfStates = allBytes.Length;

            int index = 0;
            for (int i = 0; i < countOfStates; i++)
            {
                byte[,] temp = new byte[4, 4];
                if (i != countOfStates - 1)
                {
                    for (int j = 0; j < 4; j++)
                        for (int k = 0; k < 4; k++)
                        {
                            temp[j, k] = allBytes[index];
                            index++;
                        }
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                        for (int k = 0; k < 4; k++)
                        {
                            if (index < allBytes.Length) temp[j, k] = allBytes[index];
                            else temp[j, k] = 0;
                            index++;
                        }
                }
                states.Add(temp);
            }
        }

        /// <summary>
        /// Generate states list from byte array
        /// </summary>
        private static void GetAllStates(byte[] source)
        {
            states.Clear();
            countOfStates = source.Length / 16;
            lengthOfStates = source.Length;

            int index = 0;
            for (int i = 0; i < countOfStates; i++)
            {
                byte[,] temp = new byte[4, 4];
                if (i != countOfStates - 1)
                {
                    for (int j = 0; j < 4; j++)
                        for (int k = 0; k < 4; k++)
                        {
                            temp[j, k] = source[index];
                            index++;
                        }
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                        for (int k = 0; k < 4; k++)
                        {
                            if (index < source.Length) temp[j, k] = source[index];
                            else temp[j, k] = 0;
                            index++;
                        }
                }
                states.Add(temp);
            }
        }


        /// <summary>
        /// Get byte block from 16byte array
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static byte[] ByteBlockToArray(byte[,] matrix)
        {
            int counter = 0;
            byte[] result = new byte[16];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    result[counter] = matrix[i, j];
                    counter++;
                }
            return result;
        }


        /// <summary>
        /// Convert byte list to array
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static byte[] ByteListToArray(List<byte[]> list)
        {
            byte[] res = new byte[countOfStates * 16];

            for (int i = 0; i < list.Count; i++)
                for (int j = 0; j < list[i].Length; j++)
                    res[i * 16 + j] = list[i][j];

            return res;
        }

        /// <summary>
        /// Convert byte list to string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string ByteListToString(List<byte[]> list)
        {
            string res = "";

            for (int i = 0; i < list.Count; i++)
                for (int j = 0; j < list[i].Length; j++)
                    if (list[i][j] != 0) res += (char)(list[i][j]);

            return res;
        }


        /// <summary>
        /// Get element from SBox table
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private static byte GetSBox(byte el)
        {
            if (el > 15)
                return sBox[Convert.ToInt16(Convert.ToString(el, 16)[0].ToString(), 16), Convert.ToInt16(Convert.ToString(el, 16)[1].ToString(), 16)];
            else return sBox[0, Convert.ToInt16(Convert.ToString(el, 16)[0].ToString(), 16)];
        }

        /// <summary>
        /// Get element from Inverse SBox table
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private static byte GetInvSBox(byte el)
        {
            if (el > 15)
                return invSBox[Convert.ToInt16(Convert.ToString(el, 16)[0].ToString(), 16), Convert.ToInt16(Convert.ToString(el, 16)[1].ToString(), 16)];
            else return invSBox[0, Convert.ToInt16(Convert.ToString(el, 16)[0].ToString(), 16)];
        }


        /// <summary>
        /// Generate round keys
        /// </summary>
        private static void KeySchedule()
        {
            byte[,] allRoundKeys = new byte[4, 44];

            RoundKey.Clear();

            // GET ALL ROUND KEYS MATRIX
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    allRoundKeys[i, j] = cipherKey[i, j];

            int round = 1;
            for (int index = 4; index < 44; index++)
            {
                // GET ROTWORD
                byte[] rotWord = new byte[4];
                for (int i = 0; i < 4; i++)
                    rotWord[i] = allRoundKeys[i, index - 1];

                if (index % 4 == 0)
                {
                    // SHIFT UP
                    byte temp = rotWord[0];
                    rotWord[0] = rotWord[1];
                    rotWord[1] = rotWord[2];
                    rotWord[2] = rotWord[3];
                    rotWord[3] = temp;

                    for (int i = 0; i < 4; i++)
                        rotWord[i] = GetSBox(rotWord[i]);

                    for (int i = 0; i < 4; i++)
                    {
                        allRoundKeys[i, index] = (byte)(allRoundKeys[i, index - 4] ^ rotWord[i] ^ rcon[round, i]);
                    }
                    round++;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        allRoundKeys[i, index] = (byte)(allRoundKeys[i, index - 4] ^ rotWord[i]);
                    }
                }
            }

            round = 0;
            for (int r = 0; r < 10; r++)
            {
                byte[,] temparr = new byte[4, 4];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 4 + round; j < 8 + round; j++)
                    {
                        temparr[i, j - 4 - round] = allRoundKeys[i, j];
                    }
                }
                RoundKey.Add(temparr);
                round += 4;
            }
        }

        /// <summary>
        /// Add round key to block
        /// </summary>
        private static void AddRoundKey()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Round == 0)
                        state[j, i] = (byte)(state[j, i] ^ cipherKey[j, i]);
                    else
                        state[j, i] = (byte)(state[j, i] ^ RoundKey[Round - 1][j, i]);
                }
            }
        }

        /// <summary>
        /// Swap bytes to bytes from SBox
        /// </summary>
        private static void SubBytes()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i, j] = GetSBox(state[i, j]);
                }
            }
        }

        /// <summary>
        /// Shift rows left
        /// </summary>
        private static void ShiftRows()
        {
            for (int i = 1; i < 4; i++)
            {
                if (i == 1)
                {
                    byte temp = state[i, 0];
                    for (int j = 0; j < 3; j++)
                    {
                        state[i, j] = state[i, j + 1];
                    }
                    state[i, 3] = temp;
                }
                if (i == 2)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        byte temp = state[i, 0];
                        for (int j = 0; j < 3; j++)
                        {
                            state[i, j] = state[i, j + 1];
                        }
                        state[i, 3] = temp;
                    }
                }
                if (i == 3)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        byte temp = state[i, 0];
                        for (int j = 0; j < 3; j++)
                        {
                            state[i, j] = state[i, j + 1];
                        }
                        state[i, 3] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Mix columns, multiply columns on GF
        /// </summary>
        private static void MixColumns()
        {
            byte[] temp = new byte[4];
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    byte sum = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        int t = 0;
                        if (gf[i, j] == 1) t = state[j, k];
                        if (gf[i, j] == 2) t = state[j, k] << 1;
                        if (gf[i, j] == 3) t = (state[j, k] << 1) ^ state[j, k];
                        if (t > 255) t = t ^ 0x11b;
                        sum = (byte)(sum ^ t);
                    }
                    temp[i] = sum;
                }
                for (int i = 0; i < 4; i++)
                    state[i, k] = temp[i];
            }
        }


        /// <summary>
        /// Swap bytes to bytes from Inverse SBox
        /// </summary>
        private static void InvSubBytes()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i, j] = GetInvSBox(state[i, j]);
                }
            }
        }

        /// <summary>
        /// Shift rows right
        /// </summary>
        private static void InvShiftRows()
        {
            for (int i = 1; i < 4; i++)
            {
                if (i == 1)
                {
                    byte temp = state[i, 3];
                    for (int j = 3; j > 0; j--)
                    {
                        state[i, j] = state[i, j - 1];
                    }
                    state[i, 0] = temp;
                }
                if (i == 2)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        byte temp = state[i, 3];
                        for (int j = 3; j > 0; j--)
                        {
                            state[i, j] = state[i, j - 1];
                        }
                        state[i, 0] = temp;
                    }
                }
                if (i == 3)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        byte temp = state[i, 3];
                        for (int j = 3; j > 0; j--)
                        {
                            state[i, j] = state[i, j - 1];
                        }
                        state[i, 0] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Inverse mix columns, multiply columns on inverse GF
        /// </summary>
        private static void InvMixColumns()
        {
            byte[] temp = new byte[4];
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    byte sum = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        int t = 0;
                        if (invgf[i, j] == 9)
                        {
                            t = state[j, k] << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t ^ state[j, k];
                        }
                        if (invgf[i, j] == 11)
                        {
                            t = state[j, k] << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t ^ state[j, k];

                            t = t << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t ^ state[j, k];
                        }
                        if (invgf[i, j] == 13)
                        {
                            t = state[j, k] << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t ^ state[j, k];

                            t = t << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t ^ state[j, k];
                        }
                        if (invgf[i, j] == 14)
                        {
                            t = state[j, k] << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t ^ state[j, k];

                            t = t << 1;
                            if (t > 255) t = t ^ 0x11b;

                            t = t ^ state[j, k];

                            t = t << 1;
                            if (t > 255) t = t ^ 0x11b;
                        }

                        sum = (byte)(sum ^ t);
                    }
                    temp[i] = sum;
                }
                for (int i = 0; i < 4; i++)
                    state[i, k] = temp[i];
            }
        }

    }
}