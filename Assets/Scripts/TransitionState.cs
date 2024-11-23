using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class TransitionState : MonoBehaviour
{
    Animator anim;
    [SerializeField] public GameObject TransitionPIC;
    [SerializeField] GameObject DEFEATEDUI;
    float time;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (PlayerController.Instance.pState.Transitioning)
        {
            TransitionPIC.SetActive(true);
            StartCoroutine(MoveState(4f));
            anim.Play("Transition");
        }
        else
        {
            TransitionPIC.SetActive(false);
        }
        if (PlayerController.Instance.pState.killedABoss)
        {
            DEFEATEDUI.SetActive(true);
            anim.Play("BOSSDEFEATED");
            PlayerController.Instance.pState.killedABoss = false;

        }
        else
        {
            time = 0f;
            DEFEATEDUI.SetActive(true);
        }
    }

    IEnumerator MoveState(float time)
    {
        PlayerController.Instance.pState.canMove = false;        
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.canMove = true;
    }
}
