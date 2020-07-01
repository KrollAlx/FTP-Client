using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace FTP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;

            Console.Write("Введите IP адрес сервера: ");
            string ip = Console.ReadLine();

            Console.Write("Введите логин: ");
            string login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string pass = Console.ReadLine();

            while (!exit)
            {
                int choice;
                do
                {
                    Console.WriteLine("1)Получить список файлов и папок в корневой директории");
                    Console.WriteLine("2)Выход");
                    Console.Write("Ваш выбор: ");
                    choice = Convert.ToInt32(Console.ReadLine());
                } while (choice < 1 || choice > 2);

                if (choice == 1)
                {              
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{ip}");
                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.UsePassive = true;
                    request.Credentials = new NetworkCredential(login, pass);

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    byte[] buffer = new byte[1024];
                    responseStream.Read(buffer, 0, buffer.Length);

                    Console.WriteLine(Encoding.UTF8.GetString(buffer).Trim('\0'));

                    string stat = response.StatusCode.ToString() + response.StatusDescription;
                    FtpWebRequest loadRequest = (FtpWebRequest)WebRequest.Create($"ftp://{ip}");
                    loadRequest.Method = WebRequestMethods.Ftp.UploadFileWithUniqueName;
                    loadRequest.UsePassive = true;
                    loadRequest.Credentials = new NetworkCredential(login, pass);

                    Stream ftpStream = loadRequest.GetRequestStream();
                    ftpStream.Write(Encoding.UTF8.GetBytes(stat), 0, stat.Length);
                    ftpStream.Close();
                }
                else
                {
                    exit = true;
                }
            }

            //bool exit = false;
            //byte[] receiveBuffer = new byte[1024];
            //string answer;
            //int readed;
            //while (!exit)
            //{
            //    Console.Write("Введите IP адрес сервера: ");
            //    string ip = Console.ReadLine();

            //    IPHostEntry ipHost = Dns.GetHostEntry(ip);
            //    IPAddress ipAddr = ipHost.AddressList[0];
            //    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 21);

            //    Socket socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //    socket.Connect(ipEndPoint);

            //    readed = socket.Receive(receiveBuffer);
            //    answer = Encoding.UTF8.GetString(receiveBuffer, 0, readed);
            //    Console.WriteLine(answer);

            //    //Console.Write("Введите логин: ");
            //    //string login = Console.ReadLine();

            //    //socket.Send(Encoding.UTF8.GetBytes($"user {login}"));

            //    //readed = socket.Receive(receiveBuffer);
            //    //answer = Encoding.UTF8.GetString(receiveBuffer, 0, readed);
            //    //Console.WriteLine(answer);


            //    //Console.Write("Введите пароль: ");
            //    //string pass = Console.ReadLine();

            //    //socket.Send(Encoding.UTF8.GetBytes($"pass {pass}"));

            //    socket.Send(Encoding.UTF8.GetBytes("help"));

            //    readed = socket.Receive(receiveBuffer);
            //    answer = Encoding.UTF8.GetString(receiveBuffer, 0, readed);
            //    Console.WriteLine(answer);

            //    socket.Send(Encoding.UTF8.GetBytes("pasv"));

            //    readed = socket.Receive(receiveBuffer);
            //    answer = Encoding.UTF8.GetString(receiveBuffer, 0, readed);
            //    Console.WriteLine(answer);

            //    int choice;
            //    do
            //    {
            //        Console.WriteLine("1)Получить список файлов а папок в корневой директории");
            //        Console.WriteLine("2)Выход");
            //        Console.Write("Ваш выбор: ");
            //        choice = Convert.ToInt32(Console.ReadLine());
            //    } while (choice< 1 || choice> 2);

            //    if (choice == 2)
            //    {
            //        exit = true;
            //        socket.Shutdown(SocketShutdown.Both);
            //        socket.Close();
            //    }
            // }                             
        }
    }
}
