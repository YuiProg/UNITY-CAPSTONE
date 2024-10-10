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
            anim.SetTrigger("Transition");
        }
        else
        {
            TransitionPIC.SetActive(false);
        }
    }
}
