using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerMessage : MonoBehaviour
{
    public Button send;
    [SerializeField] private TextMeshProUGUI _textStatus;
    [SerializeField] private TMP_InputField _inputFieldName;
    [SerializeField] private TMP_InputField _inputFieldMessage;
    

    private SignalR signalR;
    
    private string signalRHubURL = "http://109.195.51.60:5555/chatHub";
    void Start()
    {
        send.interactable = false;
        Signal();
    }

    public void Signal()
    {
        DisplayMessage("Awaiting Connection..."); 
        
        signalR = new SignalR();
        signalR.Init(signalRHubURL);
        
        signalR.On("ReceiveMessage", (string user , string message) =>
        {
            DisplayMessage($"{user} says {message}");
        });

        // signalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        // {
        //     Debug.Log($"Connected: {e.ConnectionId}");
        //     DisplayMessage("Connection Started");
        //
        //     signalR.Invoke("SendMessage", "My name", "My message");
        // };
        
        signalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
        {
            DisplayMessage("Connection successful");
            send.interactable = true;
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
        var arg1 = _inputFieldName.text;
        var arg2 = _inputFieldMessage.text;
        signalR.Invoke("SendMessage", arg1, arg2);
    }

    void DisplayMessage(string message)
    {
        _textStatus.text += $"\n{message}";
    }
}
