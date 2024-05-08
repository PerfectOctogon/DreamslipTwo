using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsomniaBeamEmitter : MonoBehaviour
{
    public float maxMana = 100f;
    public Transform gunTip;
    public LayerMask triggers;
    public float manaIncreaseRate = 5f;
    public GameObject blasterShot;
    public GameObject precisionShotEffect;
    public LineRenderer lazerBeam;
    public Transform gunNozzle;

    public AudioClip blasterShoot;
    public AudioClip precisionShoot;
    public AudioClip continuousShoot;

    AudioSource audioSource;

    public float mana;
    bool canShoot = true;
    float timer = 0f;

    public enum BlasterMode { Continuous, Blaster, Precision };
    public BlasterMode blasterMode = BlasterMode.Blaster;

    public float continuousManaDecrease = 30f;
    public float blasterManaDecrease = 7f;
    public float precisionManaDecrease = 25f;

    public float continuousShotDelay = 0.001f;
    public float blasterShotDelay = 0.5f;
    public float precisionShotDelay = 1.2f;

    public float continuousShotDamage = 1.5f;
    public float precisionShotDamage = 30f;
    public float blasterShotDamage = 10f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = blasterShoot;
        mana = maxMana;
        lazerBeam.SetPosition(0, gunNozzle.position);
        lazerBeam.SetPosition(1, gunNozzle.position);
        lazerBeam.gameObject.SetActive(false);
    }

    private void Update()
    {
        lazerBeam.SetPosition(0, gunNozzle.position);
        timer += Time.deltaTime;
        if (timer >= 1 && mana < maxMana) {
            mana += manaIncreaseRate;
            timer = 0;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            print("changed blaster mode");
            ChangeBlasterMode();
        }

        if (Input.GetButton("Fire1")) {
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }

        if (Input.GetButtonUp("Fire1") && blasterMode == BlasterMode.Continuous) {
            lazerBeam.gameObject.SetActive(false);
            GetComponent<Animation>().Stop();
            audioSource.Stop();
        }
    }

    IEnumerator Shoot() {
        if (mana > 0)
        {
            var camera = Camera.main;
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            switch (blasterMode)
            {
                case BlasterMode.Continuous:
                    if (mana >= continuousManaDecrease)
                    {
                        GetComponent<Animation>().Play("ContinuousAnimation");
                        if (!audioSource.isPlaying) { audioSource.Play(); }
                        mana -= continuousManaDecrease;
                        canShoot = false;
                        if (Physics.Raycast(ray, out hit, 1000f, ~triggers))
                        {
                            lazerBeam.gameObject.SetActive(true);

                            lazerBeam.SetPosition(1, hit.point);
                            if (hit.collider.transform.CompareTag("Enemy"))
                            {
                                hit.collider.transform.GetComponent<EnemyAI>().Damage(continuousShotDamage);
                            }
                        }
                        yield return new WaitForSeconds(continuousShotDelay);
                        canShoot = true;
                    }
                    break;
                case BlasterMode.Blaster:
                    if (mana >= blasterManaDecrease)
                    {
                        GetComponent<Animation>().Play();
                        audioSource.Play();
                        mana -= blasterManaDecrease;
                        GameObject bullet = Instantiate(blasterShot, gunTip.position, camera.transform.rotation);
                        BlasterBullet blasterBullet = bullet.GetComponent<BlasterBullet>();
                        blasterBullet.SetBulletDamage(blasterShotDamage);
                        blasterBullet.SetDirection(gunTip.forward);
                        canShoot = false;
                        yield return new WaitForSeconds(blasterShotDelay);
                        canShoot = true;
                    }
                    break;
                case BlasterMode.Precision:
                    if (mana >= precisionManaDecrease)
                    {
                        GetComponent<Animation>().Play("PrecisionAnimation");
                        audioSource.Play();
                        mana -= precisionManaDecrease;
                        canShoot = false;
                        if (Physics.Raycast(ray, out hit, 1000f, ~triggers))
                        {
                            SpawnPrecisionShotEffect(hit.point);
                            if (hit.collider.transform.CompareTag("Enemy"))
                            {
                                hit.collider.transform.GetComponent<EnemyAI>().Damage(precisionShotDamage);
                            }
                        }
                        yield return new WaitForSeconds(precisionShotDelay);
                        canShoot = true;
                    }
                    break;
            }
        }
        else {
            if (blasterMode == BlasterMode.Continuous)
            {
                lazerBeam.gameObject.SetActive(false);
                GetComponent<Animation>().Stop();
                audioSource.Stop();
            }
        }
    }

    void ChangeBlasterMode() {
        if (blasterMode == BlasterMode.Continuous) {
            lazerBeam.gameObject.SetActive(false);
            GetComponent<Animation>().Stop();
            audioSource.Stop();
            blasterMode = BlasterMode.Blaster;
            audioSource.clip = blasterShoot;
        }
        else if (blasterMode == BlasterMode.Blaster)
        {
            blasterMode = BlasterMode.Precision;
            audioSource.clip = precisionShoot;
        }
        else if (blasterMode == BlasterMode.Precision)
        {
            audioSource.clip = continuousShoot;
            blasterMode = BlasterMode.Continuous;
        }
    }

    void SpawnPrecisionShotEffect(Vector3 t) {
        Instantiate(precisionShotEffect, t, Quaternion.identity);
    }

}
