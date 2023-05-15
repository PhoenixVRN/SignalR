using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveNPC : MonoBehaviour
{
    public int ID;
    public bool IsServer;
    [SerializeField] private float _speedSend = 0.2f;
    [SerializeField] private int _horizontalMove = 10;
    [SerializeField] private int _verticallMove = 5;
    private ManagerMessage _managerMessage;

    void Start()
    {
        _managerMessage = ManagerMessage.Instance;
        if (IsServer)
        {
            MovementNPC();
            Invoke("SendCoordinates", 1f);
        }
    }

    private void MovementNPC()
    {
        transform.DOMove(new Vector3(Random.Range(-_horizontalMove, _horizontalMove),
            Random.Range(-_verticallMove, _verticallMove), 0), Random.Range(1, 3)).OnComplete(MovementNPC);
    }

    IEnumerator SendCoordinates()
    {
        while (true)
        {
            yield return new WaitForSeconds(_speedSend);
            _managerMessage.SendNPC(ID, transform.position.x, transform.position.y);
        }
    }
}
