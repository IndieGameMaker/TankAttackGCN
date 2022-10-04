#pragma warning disable CS0108, IDE0051

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using TMPro;

public class TankCtrl : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    private PhotonView pv;
    private CinemachineVirtualCamera cm;
    private AudioSource audio;

    private float h, v;

    // 탱크의 이동/회전 속도
    public float moveSpeed = 50.0f;
    public float turnSpeed = 200.0f;

    public GameObject cannonPrefab; //생성할 포탄 프리팹
    public Transform firePos;       //생성할 위치
    public AudioClip fireSfx;       //효과음 음원파일
    public TMP_Text userId;

    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();

        // Tank의 UserId 표시
        userId.text = pv.Owner.NickName;

        cm = GameObject.Find("CM vcam1")?.GetComponent<CinemachineVirtualCamera>();

        // PhotonView.IsMine False인 경우 => 네트워크 유저의 탱크 => 물리엔진 적용 X
        if (pv.IsMine == true)
        {
            // Rigidbody의 무게 중심을 변경
            rb.centerOfMass = new Vector3(0, 3.0f, 0);
            // 자신의 탱크인 경우 카메라에 연결
            cm.Follow = tr;
            cm.LookAt = tr;
        }
        else
        {
            // 물리 시뮬레이션을 하지 않는다...
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (pv.IsMine == true)
        {
            Move();

            if (Input.GetMouseButtonDown(0))
            {
                //Fire();
                //RPC 호출
                pv.RPC("Fire", RpcTarget.AllViaServer, null);
            }
        }
    }

    void Move()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        tr.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
        tr.Rotate(Vector3.up * Time.deltaTime * h * turnSpeed);
    }

    [PunRPC]
    void Fire()
    {
        audio.PlayOneShot(fireSfx, 0.5f);
        Instantiate(cannonPrefab, firePos.position, firePos.rotation);
    }
}


/*
    RPC (Remote Procedure Call)
*/