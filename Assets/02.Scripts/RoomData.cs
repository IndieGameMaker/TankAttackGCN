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
        }
    }

}
/*
    var info = RoomData.RoomInfo; //get

    RoomData.RoomInfo = {값}; //set
*/