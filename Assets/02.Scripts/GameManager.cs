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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        CreateTank();
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
}
