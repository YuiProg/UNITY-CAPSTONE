using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suriken_Trap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform bulletpos;
    [SerializeField] public float AttackDistance;
    public float timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        timer += Time.deltaTime;
        if (_dist < AttackDistance)
        {
            if (timer > 2)
            {
                timer = 0;
                shoot();
            }
        }
        
    }

    void shoot()
    {
        Instantiate(bullet, bulletpos.position, Quaternion.identity);
    }

}
