using System.Collections;
using UnityEngine;

public class TransitionState : MonoBehaviour
{
    Animator anim;
    [SerializeField] public GameObject TransitionPIC;

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
            anim.SetTrigger("Transition");
        }
        else
        {
            TransitionPIC.SetActive(false);
        }
    }

    IEnumerator MoveState(float time)
    {
        PlayerController.Instance.pState.canMove = false;        
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.canMove = true;
    }
}
