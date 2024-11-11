using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 3f;
    public float Yoffset = 1f;
    public float zoffSet = -15f;
    public Transform target;

    private void Awake()
    {
        transform.position = target.position;
    }
    void Update()
    {
        if (BANDIT_BOSS.instance.spottedPlayer || SpearGirl.instance.spottedPlayer ||
            Paladin_BOSS.instance.spottedPlayer || PlayerController.Instance.pState.inParkourState || Desert_BOSS.instance.spottedPlayer)
        {
            Vector3 newPoss = new Vector3(target.position.x, target.position.y + Yoffset, -30f);
            transform.position = Vector3.Slerp(transform.position, newPoss, FollowSpeed * Time.deltaTime);
        }
        else if (PlayerController.Instance.pState.isNPC)
        {
            Vector3 newPoss = new Vector3(target.position.x, target.position.y + Yoffset, -8f);
            transform.position = Vector3.Slerp(transform.position, newPoss, FollowSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 newPoss = new Vector3(target.position.x, target.position.y + Yoffset, zoffSet);
            transform.position = Vector3.Slerp(transform.position, newPoss, FollowSpeed * Time.deltaTime);
        }        
    }
}
