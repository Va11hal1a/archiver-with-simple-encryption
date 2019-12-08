using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0].Split('.')[1] == "ar")
            {
                Console.WriteLine("Do you want to decrypt file? Write key or just press enter to continue");
                int key = 0;
                Int32.TryParse(Console.ReadLine(), out key);
                Console.WriteLine(key);
                dearchive(args[0], key);
            }
            else
            {
                Console.WriteLine("Do you want to crypt file? Write key or just press enter to continue");
                int key = 0;
                Int32.TryParse(Console.ReadLine(), out key);
                archive(args[0], key);
            }
        }
        static void archive(string path, int key)
        {
            FileStream stream = File.OpenRead(path);
            byte[] raw = new byte[stream.Length];
            byte[] extr = new byte[stream.Length * 2];
            stream.Read(raw, 0,(int) stream.Length);
            int iter = 0;
            for (int i = 0; i < stream.Length;)
            {
                int c = 1;
                for (int j = 1; !(i + j >= stream.Length); ++j)
                {
                    if (raw[i] == raw[i + j])
                    {
                        c++;
                    }
                    else
                    {
                        break;
                    }
                }
                extr[iter] = (byte)c;
                extr[iter + 1] = (byte)(raw[i] ^ key);
                i += c;
                iter += 2;
            }
            string type = path.Split('.')[1];
            byte[] load = new byte[iter + 1 + type.Length];
            for (int i = 0; i < iter; i++)
            {
                load[i] = extr[i];
            }
            load[iter] = (byte)0;
            for (int i = 0; i < type.Length; i++)
            {
                load[iter + 1 + i] = (byte)type[i];
            }
            FileStream file = File.OpenWrite(path.Split('.')[0] + ".ar");
            file.Write(load, 0, iter + 1 + type.Length);
            file.Close();
            stream.Close();
        }
        static void dearchive(string path, int key = 0)
        {
            FileStream stream = File.OpenRead(path);
            byte[] readed = new byte[stream.Length];
            byte[] extracted = new byte[stream.Length * 2];
            stream.Read(readed, 0, (int)stream.Length);
            int iter = 0;
            int end = 0;
            for (int i = 0; i < stream.Length; i += 2)
            {
                if (readed[i] == (byte)0)
                {
                    end = i;
                    break;
                }
                for (int j = 0; j < readed[i]; j++)
                {
                    extracted[iter + j] = readed[i + 1];
                }
                iter += readed[i];
            }
            byte[] load = new byte[iter];
            for (int i = 0; i < iter; i++)
            {
                load[i] = (byte)(extracted[i] ^ key);
            }
            string type = String.Empty;
            for (int k = end + 1; k < stream.Length; k++)
            {
                type += (char)readed[k];
            }
            FileStream file = File.OpenWrite(path.Split('.')[0] + '.' + type);
            file.Write(load, 0, iter) ;
            file.Close();
            stream.Close();
        }
    }
}
