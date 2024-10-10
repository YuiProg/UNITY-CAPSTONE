using UnityEngine;

public class PlayerState : MonoBehaviour
{
    //OTHER
    public bool isPaused = false;
    public bool Transitioning = false;

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

    //parry,block
    public bool blocking = false;
    public bool parry = false;

    //recoil
    public bool recoilingX, recoilingY;
    public bool lookingRight;


    //KEY CHECK
    public bool hasKey = false;


    //SKILLS
    public bool SLASH = false;
    public bool HPBUFF = false;

}
