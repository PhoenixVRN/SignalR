using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using TMPro;
using UnityEngine;

public class ConnectToHub : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI uiText;
    void Start()
    {
       var con = ConnectToHubAsync();
       
    }
    void DisplayMessage(string message)
    {
        uiText.text += $"\n{message}";
    }


    public async void SendMessage()
    {
        
    }
    
 public async Task<HubConnection> ConnectToHubAsync()
 {
    Debug.Log("ConnectToHubAsync start");
 
    //Создаем соединение с нашим написанным тестовым хабом
    var connection = new HubConnectionBuilder()
        .WithUrl("http://109.195.51.60:5555/chatHub")
        .WithAutomaticReconnect()
        .Build();

    
    
    Debug.Log("connection handle created");
   
    //подписываемся на сообщение от хаба, чтобы проверить подключение
    // connection.On<string, string>("ReceiveMessage",
        // (user, message) => LogAsync($"{user}: {message}").Forget());
        // var r = LogAsync($"{user}: {message}").Forget());
        connection.On<string, string>("ReceiveMessage", (string user , string message) =>
        {
            // var json = JsonUtility.FromJson<JsonPayload>(message);
            // DisplayMessage($"ReceiveMessage: {json.test} + {json.id} + {user}");
            DisplayMessage($"ReceiveMessage: {user} + {message}");
            
        }); 
      
    while (connection.State != HubConnectionState.Connected)
    {
        try
        {
            if (connection.State == HubConnectionState.Connecting)
            {
                await Task.Delay(TimeSpan.FromSeconds(10000));
                continue;
            }
 
            Debug.Log("start connection");
            await connection.StartAsync();
            Debug.Log("connection finished");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
     //  var json1 = new JsonPayload
     // {
     //     test = "Poehali",
     //     id = "55555"
     // };
     //  connection.InvokeAsync("SendMessage", JsonUtility.ToJson(json1));
      // connection.InvokeAsync("SendMessage", "JsonUtility.ToJson(json1)", "hffjjf");
    return connection;
 }
 
 public class JsonPayload
 {
     public string test;
     public string id;
 }
}
