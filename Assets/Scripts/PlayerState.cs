using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private void Update()
    {
        obtainedSpear = PlayerPrefs.GetInt("SPEAR") == 1;
        obtainedSLASH = PlayerPrefs.GetInt("SLASH") == 1;
        obtainedMAP = PlayerPrefs.GetInt("MAP") == 1;
        hasBOOK = PlayerPrefs.GetInt("HASBOOK") == 1;
    }
    //OTHER
    public bool isPaused = false;
    public bool Transitioning = false;
    public bool inParkourState = false;
    public bool isNPC = false;
    public bool canPause = true;

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
    public bool hasBOOK = false;

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
    public bool obtainedSLASH = false;


    //SKILLS
    public bool SpearDash = false;
    public bool SLASH = false;
    public bool HPBUFF = false;

    //locations
    public bool inIfugao = false;
    public bool inMactan = false;
    public bool inTondo = false;


}
