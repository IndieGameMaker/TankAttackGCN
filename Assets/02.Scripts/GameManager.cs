using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomInfo;
    public TMP_Text playerList;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
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
    }

    // 플레이어가 퇴장했을 때 호출되는 콜백
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DisplayRoomInfo();
    }
}




// https://drive.google.com/file/d/1HFsD7wgnMyreZa2mE4L9U7q4WcUDptmD/view?usp=sharing