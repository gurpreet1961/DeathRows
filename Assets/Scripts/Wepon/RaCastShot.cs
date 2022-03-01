using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RaCastShot : MonoBehaviour
{
    public Camera Playercamera;
    public Transform shootPoint;
    public float FireRate = 10f;
    private float timeBetweenNextShot;
    public float Damage = 20f;
    public float range = 100f;
    public GameObject hitEffect;

    public mouseLook mouse;
    // public Animator moveEffect;
    private AudioSource AudioSource;
    public AudioClip shootSound;
    public enum ShootMode { Auto, Semi }
    [Header("ShootMode")]
    public ShootMode shootingMode;
    private bool shootInput;
    [Header("Recoil")]
    public float vRecoil = 1f;
    public float hRecoil = 1f;
    public Animator AimAnim;
    public bool aim;



    //Ammo Part
    [Header("Ammo Mangement")]
    public int ammocount = 25;
    public int availableammo = 100;
    public int maxAmmo = 25;
    public Animator anim;

    public Text currentammotext;

    public bool canUse = true;
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        // if (Input.GetButton("Fire2"))
        // {
        //     AimAnim.SetBool("Aim", true);
        // }
        // else
        // {
        //     AimAnim.SetBool("Aim", false);
        // }

        if (canUse)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                aim = !aim;
            }
            if (aim == true)
            {
                AimAnim.SetBool("Aim", true);
            }
            else
            {
                AimAnim.SetBool("Aim", false);
            }
            if (canUse)
            {
                currentammotext.text = ammocount.ToString();
            }
            if (Input.GetKeyDown(KeyCode.R) && ammocount < maxAmmo)
            {
                mouse.AddRecoil(0, 0);
                anim.SetBool("Reload", true);
                anim.SetBool("Shoot", false);

            }
            if (ammocount <= 0)
            {
                mouse.AddRecoil(0, 0);
                anim.SetBool("Reload", true);
                anim.SetBool("Shoot", false);

                return;
            }
        }
        switch (shootingMode)
        {
            case ShootMode.Auto:

                shootInput = Input.GetButton("Fire1");
                break;
            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
                break;
        }
        if (canUse)
        {
            if (shootInput && Time.time >= timeBetweenNextShot)
            {
                timeBetweenNextShot = Time.time + 1f / FireRate;
                float h = Random.Range(-hRecoil, hRecoil);
                float v = Random.Range(0, vRecoil);
                if (shootInput == Input.GetButtonDown("Fire1"))
                {
                    anim.SetBool("Shoot", false);
                }
                else
                {
                    anim.SetBool("Shoot", true);
                }

                mouse.AddRecoil(h, v);
                weapon();

            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            mouse.AddRecoil(0, 0);
            anim.SetBool("Shoot", false);

        }
    }
    public void weapon()
    {

        ammocount--;
        RaycastHit hit;
        PlayShootSound();
        if (Physics.Raycast(Playercamera.transform.position, Playercamera.transform.forward, out hit, range))
        {
            GameObject HitSparks = Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); //use to swap any component(here we are adding hiteffect on ground when bullet hit)
            Destroy(HitSparks, 2f);
            if (hit.transform.tag == "Enemy")
            {
                Health enemy = hit.transform.GetComponent<Health>();
                enemy.Damage(Damage);
            }

        }
    }
    public void Enemyweapon()
    {
        RaycastHit hit;
        if (Time.time >= timeBetweenNextShot)
        {
            PlayShootSound();
            timeBetweenNextShot = Time.time + 1f / FireRate;
            if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
            {
                GameObject HitSparks = Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); //use to swap any component(here we are adding hiteffect on ground when bullet hit)
                Destroy(HitSparks, 2f);
                if (hit.transform.tag == "Player")
                {
                    Health enemy = hit.transform.GetComponent<Health>();
                    enemy.Damage(Damage);
                }
            }
        }
    }
    public void Reload()
    {
        mouse.AddRecoil(0, 0);
        availableammo -= (maxAmmo - ammocount);
        ammocount = maxAmmo;
        anim.SetBool("Shoot", false);

        anim.SetBool("Reload", false);
    }
    private void PlayShootSound()
    {
        AudioSource.PlayOneShot(shootSound);
        // AudioSource.clip = shootSound;
        // AudioSource.Play();
    }
}
