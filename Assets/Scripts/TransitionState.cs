using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class TransitionState : MonoBehaviour
{
    Animator anim;
    [SerializeField] public GameObject TransitionPIC;
    [SerializeField] GameObject DEFEATEDUI;
    [SerializeField] GameObject DEFEATEDSKILLUI;
    [SerializeField] GameObject NEWSKILL;

    //location
    [SerializeField] GameObject IFUGAO;
    [SerializeField] GameObject MACTAN;
    [SerializeField] GameObject TONDO;
    [SerializeField] GameObject BALWEG;

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
            DEFEATEDUI.SetActive(true);
        }
        if (PlayerController.Instance.pState.SkillBOSS)
        {
            DEFEATEDSKILLUI.SetActive(true);
            anim.Play("BOSSDEFEATEDSKILL");
            PlayerController.Instance.pState.SkillBOSS = false;
        }
        else
        {
            DEFEATEDSKILLUI.SetActive(false);
        }
        if (PlayerController.Instance.pState.newSkill)
        {
            NEWSKILL.SetActive(true);
            anim.Play("NEW SKILL");
            PlayerController.Instance.pState.newSkill = false;
        }
        else
        {
            NEWSKILL.SetActive(false);
        }
        if (PlayerController.Instance.pState.inIfugao)
        {
            IFUGAO.SetActive(true);
            anim.Play("IFUGAO");
            PlayerController.Instance.pState.inIfugao = false;
        }
        else
        {
            IFUGAO.SetActive(false);
        }
        if (PlayerController.Instance.pState.inMactan)
        {
            MACTAN.SetActive(true);
            anim.Play("MACTAN");
            PlayerController.Instance.pState.inMactan = false;
        }
        else
        {
            MACTAN.SetActive(false);
        }
        if (PlayerController.Instance.pState.inTondo)
        {
            TONDO.SetActive(true);
            anim.Play("TONDO");
            PlayerController.Instance.pState.inTondo = false;
        }
        else
        {
            TONDO.SetActive(false);
        }


    }

    IEnumerator MoveState(float time)
    {
        PlayerController.Instance.pState.canMove = false;        
        yield return new WaitForSeconds(time);
        PlayerController.Instance.pState.canMove = true;
    }
}
