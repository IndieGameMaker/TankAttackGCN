using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
// TextMeshPro 네임스페이스
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 04f2788e-b8a4-4cb2-b720-8a578d3509de
    // 포톤 서버에 접속할 때 사용할 게임 버전
    private readonly string gameVersion = "1.0";
    // 유저ID
    public string userId = "Zackiller";

    // 입력 필드
    public TMP_InputField userId_IF;
    public TMP_InputField roomName_IF;

    public GameObject roomPrefab;
    public Transform contentTr;

    void Awake()
    {
        // 방장이 로딩한 씬을 자동으로 로딩시켜주는 옵션
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.NickName = userId;

        if (PhotonNetwork.IsConnected == false)
        {
            // 포톤서버에 접속
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void Start()
    {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 1000):000}");
        userId_IF.text = userId;
    }

    // 포톤 서버에 접속완료되었을 때 호출되는 콜백함수(이벤트)
    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 서버에 접속");

        // 로비에 입장 요청
        PhotonNetwork.JoinLobby();
    }

    // 로비에 입장한 후 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 입장 완료");

        // 무작위 룸에 입장 요청
        // PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤 조인에 실패 했을 경우에 호출되는 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code : {returnCode} , message : {message}");

        // 룸 생성
        PhotonNetwork.CreateRoom("My Room");
    }

    // 룸 생성이 완료된 후 호출되는 콜백
    public override void OnCreatedRoom()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;
        Debug.Log(roomName + " 생성 완료");
    }

    // 룸에 입장완료된 후에 호출되는 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("룸에 입장 완료");

        // 방장일 경우에만 베틀필드 씬을 로딩
        if (PhotonNetwork.IsMasterClient == true)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }
        //UnityEngine.SceneManagement.SceneManager.LoadScene("BattleField");

        // 방 입장완료 후 탱크 생성
        // PhotonNetwork.Instantiate("Tank", new Vector3(0, 3.0f, 0), Quaternion.identity, 0);
    }

    // 로그인 버튼 클릭 이벤트(콜백함수)
    public void OnLoginButtonClick()
    {
        SetUserId();
        PhotonNetwork.JoinRandomRoom();
    }

    // 룸 생성 버튼 클릭 이벤트
    public void OnMakeRoomButtonClick()
    {
        SetUserId();

        // 룸 이름이 없을 경우 
        if (string.IsNullOrEmpty(roomName_IF.text))
        {
            roomName_IF.text = $"ROOM_{Random.Range(0, 1000):000}";
        }

        // 룸 속성을 정의
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 20,
            IsOpen = true,
            IsVisible = true
        };

        // 룸 생성
        PhotonNetwork.CreateRoom(roomName_IF.text, ro);
    }

    void SetUserId()
    {
        // 유저아이디 필드가 Null 또는 "" 일 경우를 확인
        if (string.IsNullOrEmpty(userId_IF.text))
        {
            userId = $"USER_{Random.Range(0, 1000):000}"; // 15 -> USER_015
            userId_IF.text = userId;
        }

        // 입력필드가 Null이 아닌 경우
        userId = userId_IF.text;

        // 유저 아이디를 저장
        PlayerPrefs.SetString("USER_ID", userId);

        PhotonNetwork.NickName = userId;
    }

    // 룸 목록이 갱신될때마다 호출되는 콜백
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            Debug.Log(room.Name);
        }
    }
}
