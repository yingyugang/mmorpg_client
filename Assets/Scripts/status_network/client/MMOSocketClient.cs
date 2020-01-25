using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace MMO {

    [System.Serializable]
    public class MessageRequest
    {
        public int requestId;
        public string requestMessage;
    }


    public class MMOSocketClient : SingleMonoBehaviour<MMOSocketClient>
    {
        //メセージを監視用スレッド
        public Thread thread;
        public Thread thread1;
        static TcpClient client;
        public static bool isRunning = true;

        public static Dictionary<string, float> ips;
        static float time; 
        static Int32 port = 10000;
        static bool logined = false;
        public static Queue<string> messageQueue = new Queue<string>();
        public static UnityAction<string> onAction;

        protected override void Awake()
        {
            base.Awake();
            //監視しているポート
            ips = new Dictionary<string, float>();
            //Int32 port = 10000;
            //client = new TcpClient("34.85.109.164", port);
            //client = new TcpClient("127.0.0.1", port);
            //return;//TODO
            thread = new Thread(new ThreadStart(ThreadMethod));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Send(short msgId,string message) {
            MessageRequest request = new MessageRequest();
            request.requestId = msgId;
            request.requestMessage = message;
            string messageJson = JsonUtility.ToJson(request) + "\r\n";
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(messageJson);
            client.Client.Send(data, data.Length, SocketFlags.None);
        }

        public static void SendLogin()
        {
            Debug.Log("SendLogin");
            logined = true;
            MessageRequest request = new MessageRequest();
            request.requestId = 70000;
            request.requestMessage = "";
            string messageJson = JsonUtility.ToJson(request) + "\r\n";
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(messageJson);
            client.Client.Send(data, data.Length, SocketFlags.None);
        }

        void StartRead()
        {
            thread1 = new Thread(new ThreadStart(ThreadMethod1));
            thread1.IsBackground = true;
            thread1.Start();
        }

        void Update()
        {
            if (client!=null && client.Connected && !logined) {
                StartRead();
                SendLogin();
            }
            while (messageQueue.Count>0) {
                string message = messageQueue.Dequeue();
                if (onAction!=null && message!=null)
                {
                    onAction(message);
                }
            }
            //Test Send
            if (Input.GetKeyDown(KeyCode.H))
            {
                MessageRequest request = new MessageRequest();
                request.requestId = 111;
                request.requestMessage = "abc";
                string message = JsonUtility.ToJson(request) + "\r\n";
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                client.Client.Send(data, data.Length, SocketFlags.None);
            }
        }

        public void StopReceive()
        {
            thread.Abort();
        }

        private static void ThreadMethod()
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer 
            // connected to the same address as specified by the server, port
            // combination.
            client = new TcpClient("127.0.0.1", port);
           
            while (isRunning)
            {
                //メセージを受け取っていない時、読み取ない。
                //Send("dddd\r\n");
                Thread.Sleep(100);
            }
            Debug.Log("Thread Done!");
        }

        private static void ThreadMethod1()
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer 
            // connected to the same address as specified by the server, port
            // combination.
            while (isRunning)
            {
                //メセージを受け取っていない時、読み取ない。
                Read();
            }
            Debug.Log("Thread Done!");
        }


        void OnDestroy()
        {
            client.Close();
            if (thread != null)
                thread.Abort();
            if (thread1 != null)
                thread1.Abort();
            isRunning = false;
        }

        void OnApplicationQuit()
        {
            client.Close();
            if(thread!=null)
                thread.Abort();
            if (thread1 != null)
                thread1.Abort();
            isRunning = false;
        }

        static void Send(string message)
        {
            try
            {
                //client = new TcpClient("127.0.0.1", port);
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                //NetworkStream stream = client.GetStream();
                // Send the message to the connected TcpServer. 
                //stream.Write(data, 0, data.Length);

                client.Client.Send(data, data.Length, SocketFlags.None);
                Debug.Log("Send");
                // client.Client.Send(data);
                //stream.Flush();
                 // Receive the TcpServer.response.
                 // Buffer to store the response bytes.
                data = new Byte[256];
                // String to store the response ASCII representation.
                String responseData = String.Empty;
                // Read the first batch of the TcpServer response bytes.
                //stream.Close();
                //client.Close();
            }
            catch (ArgumentNullException e)
            {
                //Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                //Console.WriteLine("SocketException: {0}", e);
            }

            //Console.WriteLine("\n Press Enter to continue...");
            //.Read();
        }

        static void Read()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                StreamReader serverReader = new StreamReader(stream, Encoding.UTF8);
                string responseData = serverReader.ReadLine();
                Debug.Log(responseData);
                messageQueue.Enqueue(responseData);
            }
            catch (ArgumentNullException e)
            {
                //Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                //Console.WriteLine("SocketException: {0}", e);
            }
        }
    }

}