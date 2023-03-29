using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerMessage : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _textMessage;
    [SerializeField] private TextMeshProUGUI _textStatus;

    private SignalR signalR;
    
    private string signalRHubURL = "http://109.195.51.60:5555/chatHub";
    void Start()
    {
        Signal();
    }

    public void Signal()
    {
        DisplayMessage("Awaiting Connection..."); 
        
        signalR = new SignalR();
        signalR.Init(signalRHubURL);
        
        signalR.On("ReceiveMessage", (string user , string message) =>
        {
            DisplayMessage($"ReceiveMessage: {user} + {message}");
        });

        signalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        {
            Debug.Log($"Connected: {e.ConnectionId}");
            DisplayMessage("Connection Started");

            signalR.Invoke("SendMessage", "My name", "My message");
        };
        
        signalR.ConnectionClosed += (object sender, ConnectionEventArgs e) =>
        {
            Debug.Log($"Disconnected: {e.ConnectionId}");
            DisplayMessage("Connection Disconnected");
        };

        signalR.Connect();
    }

    public void Send()
    {
        signalR.Invoke("SendMessage", "My name2", "My message2");
    }

    void DisplayMessage(string message)
    {
        _textStatus.text += $"\n{message}";
    }
}
