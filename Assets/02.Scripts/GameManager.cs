using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;

    public TMP_Text roomInfo;
    public TMP_Text playerList;
    public TMP_Text chatMsg;
    public TMP_InputField msg_IF;

    private PhotonView pv;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pv = GetComponent<PhotonView>();

        CreateTank();
        DisplayRoomInfo();
    }

    void CreateTank()
    {
        Vector3 pos = new Vector3(Random.Range(-100, 100),
                                  3.0f,
                                  Random.Range(-100, 100));

        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity);
    }

    // Exit 버튼 클릭 이벤트
    public void OnExitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    // 포톤 서버에서 룸에서 나갔을 때 호출된는 콜백
    public override void OnLeftRoom()
    {
        // 로비씬을 로딩
        SceneManager.LoadScene("Lobby");
    }

    void DisplayRoomInfo()
    {
        Room currRoom = PhotonNetwork.CurrentRoom;
        // 룸 정보 표시
        string roomInfoText = $"{currRoom.Name} ({currRoom.PlayerCount}/{currRoom.MaxPlayers})";
        roomInfo.text = roomInfoText;

        string _players = "";

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient) // 방장 : Red
            {
                _players += $"<color=#ff0000>{player.NickName}</color>\n";
            }
            else
            {
                _players += $"<color=#00ff00>{player.NickName}</color>\n";
            }
        }

        playerList.text = _players;
    }

    // 플레이어가 입장했을 때 호출되는 콜백
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DisplayRoomInfo();

        string msg = $"<color=#00ff00>[{newPlayer.NickName}]</color> is joined.";
        ChatMessage(msg);
    }

    // 플레이어가 퇴장했을 때 호출되는 콜백
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DisplayRoomInfo();
        string msg = $"<color=#ff0000>[{otherPlayer.NickName}]</color> is left room.";
        ChatMessage(msg);
    }

    public void OnSendChatMessage(string msg)
    {
        //Debug.Log(msg);
        msg = $"[<color=#00ff00>{PhotonNetwork.NickName}</color>] {msg_IF.text}";
        pv.RPC("ChatMessage", RpcTarget.AllBufferedViaServer, msg);
    }

    public void SendChatMessage(string msg)
    {
        pv.RPC("ChatMessage", RpcTarget.AllBufferedViaServer, msg);
    }

    // Chatting RPC Function
    [PunRPC]
    public void ChatMessage(string msg)
    {
        chatMsg.text += $"{msg}\n"; // msg + "\n";
    }



    // 방장이 변경됐을경우 호출 되는 콜백 함수
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 자신이 방장 지위를 인계했을 경우 다른 유저에게 RPC로 메시지를 전달하는 로직
        if (pv.IsMine && newMasterClient.ActorNumber == pv.OwnerActorNr)
        {
            SendChatMessage(newMasterClient.NickName + " is new Master.");
        }

        // 방장권한을 다른 유저에 위임 하는 코드
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     Player newMaster = PhotonNetwork.CurrentRoom.GetPlayer(_actorNumber);
        //     PhotonNetwork.SetMasterClient(newMaster);
        // }
    }

}




// https://drive.google.com/file/d/1HFsD7wgnMyreZa2mE4L9U7q4WcUDptmD/view?usp=sharing