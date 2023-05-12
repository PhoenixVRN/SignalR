using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerMessage : MonoBehaviour
{
    public int ID;
    public Button send;
    [SerializeField] private TextMeshProUGUI _textStatus;
    [SerializeField] private TMP_InputField _inputFieldName;
    [SerializeField] private TMP_InputField _inputFieldMessage;
    [SerializeField] private TextMeshProUGUI _inputID;

    [SerializeField] private MovePlayer _movePlayer;
    [SerializeField] private GameObject _enamy;
    // [SerializeField] private GameObject _blue;
    
    private SignalR signalR;
    private bool IsSend;
    
    private string signalRHubURL = "http://109.195.51.60:5555/chatHub";
    void Start()
    {
        send.interactable = false;
        Signal();
    }

    private void Update()
    {
        if (IsSend)
        {
            StartCoroutine(Send());
        }
    }

    public void Signal()
    {
        DisplayMessage("Awaiting Connection..."); 
        
        signalR = new SignalR();
        signalR.Init(signalRHubURL);
        
        signalR.On("ReceiveMessage", (int ID, float args1, float args2 ) =>
        {
            // DisplayMessage($"{user} says {message} + {ID}");
            ResponseDTO(ID, args1, args2);
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

    public void InitID()
    {
        ID = int.Parse(_inputFieldName.text);
        _inputID.text = _inputFieldName.text;
        send.gameObject.SetActive(false);
        IsSend = true;
    }
    public IEnumerator Send()
    {
        IsSend = false;
        
        var arg1 = ID;
        var arg2 = _movePlayer.gameObject.transform.position.x;
        var arg3 = _movePlayer.gameObject.transform.position.y;
        signalR.Invoke("SendMessage", arg1, arg2, arg3);
        yield return new WaitForSeconds(0.02f);
        IsSend = true;
    }
    
    void DisplayMessage(string message)
    {
        _textStatus.text += $"\n{message}";
    }

    private void ResponseDTO(int ID, float args1, float args2)
    {
        if (ID == this.ID) return;
        _enamy.transform.position = new Vector3(args1, args2, 0f);
    }
}
