using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManger : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private float switchDelay = 1;
    private bool isSwitching;
    private int index = 0;
    public Animator anim;
    public PlayerMovement player;
    public bool canUse = true;
    public void Start()
    {
        InitializeWeapons();
    }

    void Update()
    {
        if (canUse)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            if (x == 0f && y == 0f)
            {
                anim.SetBool("Breath", true);
                anim.SetBool("Run", false);
                anim.SetBool("Walk", false);
            }
            else
            {
                if (player.isWalking == false)
                {
                    anim.SetBool("Breath", false);
                    anim.SetBool("Run", true);
                    anim.SetBool("Walk", false);
                }
                if (player.isWalking == true)
                {
                    anim.SetBool("Breath", false);
                    anim.SetBool("Run", false);
                    anim.SetBool("Walk", true);
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
            {
                index++;
                if (index >= weapons.Length)
                {
                    index = 0;
                }
                StartCoroutine(SwitchAfterDelay(index));
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
            {
                index--;
                if (index < 0)
                {
                    index = weapons.Length - 1;
                }
                StartCoroutine(SwitchAfterDelay(index));
            }

        }
    }
    private void InitializeWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
    }
    private IEnumerator SwitchAfterDelay(int newIndex)
    {
        isSwitching = true;
        yield return new WaitForSeconds(switchDelay);
        isSwitching = false;
        SwitchWeapons(newIndex);
    }
    private void SwitchWeapons(int newIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[newIndex].SetActive(true);
    }
}
