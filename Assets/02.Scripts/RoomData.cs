using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomData : MonoBehaviour
{
    public TMP_Text roomText;

    private RoomInfo roomInfo;

    // 프로퍼티로 선언
    public RoomInfo RoomInfo
    {
        get
        {
            return roomInfo;
        }
        set
        {
            roomInfo = value;

            //룸 정보를 갱신   "MyRoom (5/20)"
            string msg = $"{roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})";
            roomText.text = msg;

            // 버튼 클릭시 호출할 이벤트를 연결 (람다식, Delegate)
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(
                () =>
                {
                    Debug.Log(roomInfo.Name + " 버튼 클릭됨");

                    // PhotonManager 검색
                    GameObject.Find("PhotonManager")?.GetComponent<PhotonManager>().SetUserId();


                    PhotonNetwork.JoinRoom(roomInfo.Name);
                }
            );
        }
    }

}
/*
    var info = RoomData.RoomInfo; //get

    RoomData.RoomInfo = {값}; //set
*/