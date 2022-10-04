using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
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
}
