using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private void Update()
    {
        obtainedSpear = PlayerPrefs.GetInt("HERALD GOLEM") == 1;
        obtainedMAP = PlayerPrefs.GetInt("MAP") == 1;
    }
    //OTHER
    public bool isPaused = false;
    public bool Transitioning = false;
    public bool inParkourState = false;
    public bool isNPC = false;

    //movement bool
    public bool jumping = false;
    public bool dashing = false;
    public bool running = false;
    public bool walking = false;

    //status bool
    public bool invincible = false;
    public bool canMove = true;
    public bool isAlive = true;
    public bool canAttack = true;
    public bool killedABoss = false;
    public bool SkillBOSS = false;

    //parry,block
    public bool blocking = false;
    public bool parry = false;

    //recoil
    public bool recoilingX, recoilingY;
    public bool lookingRight;


    //KEY CHECK
    public bool hasKey = false;

    //obtainskills
    public bool obtainedMAP = false;
    public bool obtainedSpear = false;


    //SKILLS
    public bool SpearDash = false;
    public bool SLASH = false;
    public bool HPBUFF = false;


}
