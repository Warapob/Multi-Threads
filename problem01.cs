using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

#pragma warning disable SYSLIB0011

namespace Problem01
{
    class Program
    {
        static byte[] Data_Global = new byte[1000000000];
        // static long Sum_Global = 0;
        // static int i = 0;

        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try
            {
                Data_Global = (byte[])bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }
        static long sum(long Start_Index, long End_Index)
        {
            // Console.WriteLine("Start = {0}, End = {1}",Start_Index,End_Index);
            long sum_result = 0;
            for (long i = Start_Index; i < End_Index; i++)
            {
                if (Data_Global[i] % 2 == 0)
                {
                    sum_result -= Data_Global[i];
                }
                else if (Data_Global[i] % 3 == 0)
                {
                    sum_result += (Data_Global[i] * 2);
                }
                else if (Data_Global[i] % 5 == 0)
                {
                    sum_result += (Data_Global[i] / 2);
                }
                else if (Data_Global[i] % 7 == 0)
                {
                    sum_result += (Data_Global[i] / 3);
                }
                Data_Global[i] = 0;

            }
            // Console.WriteLine(sum_result);
            return sum_result;
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int y;
            int ThreadNum = 64;

            long[] result = new long[ThreadNum];
            Thread[] th = new Thread[ThreadNum];

            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            int DataLen = Data_Global.Length;


            int split_len = DataLen / ThreadNum;
            // Console.WriteLine("DataLen = {0}\nsplit_len = {1}", DataLen, split_len);


            /* ---------- Start ---------- */

            Console.Write("\n\nWorking...");
            sw.Start();

            // Thread Initialization
            int temp_index = 0;
            for (int i = 0; i < ThreadNum; i++)
            {
                // Console.WriteLine("temp = {0}",temp_index);
                th[i] = new Thread(() =>
                    {
                        result[i] = sum(temp_index, temp_index + split_len);
                    }
                );
                th[i].Start();
                Thread.Sleep(1);
                temp_index += split_len;
            }

            for (int j = 0; j < ThreadNum; j++)
            {
                th[j].Join();
            }

            sw.Stop();
            Console.WriteLine("Done.");

            /* ---------- Result ---------- */
            Console.WriteLine("Summation result: {0}", result.Sum());
            // Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
            Console.WriteLine("Desire result: 888701676");
        }
    }
}