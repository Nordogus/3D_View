using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Sockets;
using System.IO;

public class TwitchChat : MonoBehaviour
{
    //public UnityEvent<string, string> OnChatMessage;
    public UnityEvent<string> OnChatMessage;

    TcpClient Twitch;
    StreamReader Reader;
    StreamWriter Writer;

    const string URL = "irc.chat.twitch.tv";
    const int PORT = 6667;

    string user = "NotoRoboto";
    // get OAth from https://twitchapps.com/tmi
    string oAuth = "oauth:u0kkaqe0h1uksufegpp6hlljiyt579";
    [SerializeField] string channel = "Nordogus";

    float pingConter = 0;

    private void ConnectToTwitch()
    {
        Twitch = new TcpClient(URL, PORT);
        Reader = new StreamReader(Twitch.GetStream());
        Writer = new StreamWriter(Twitch.GetStream());

        Writer.WriteLine("PASS " + oAuth);
        Writer.WriteLine("Nick " + user.ToLower());
        Writer.WriteLine("JOIN #" + channel.ToLower());
        Writer.Flush();
    }

    private void Awake()
    {
        ConnectToTwitch();
    }

    void Update()
    {
        pingConter += Time.deltaTime;
        if (pingConter > 60)
        {
            Writer.WriteLine("PING " + URL);
            Writer.Flush();
            pingConter = 0;
        }

        if (!Twitch.Connected)
        {
            //return;
            ConnectToTwitch();
        }

        if (Twitch.Available > 0)
        {
            string message = Reader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                //:nordogus!nordogus@nordogus.tmi.twitch.tv PRIVMSG #nordogus :msg
                int splitPoint = message.IndexOf("!");
                string chatter = message.Substring(1, splitPoint - 1);

                splitPoint = message.IndexOf(":", 1);
                string msg = message.Substring(splitPoint + 1);

                //OnChatMessage?.Invoke(chatter, msg);
                OnChatMessage?.Invoke(msg);
            }
            //print(message);
        }
    }
}