using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public GameObject playerPrefab;
    [Space]
    public Transform spawnPoint;

    [Space]
    public GameObject roomCam;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to Server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("We're connected and in a room now");

        roomCam.SetActive(false);

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
    }
}
