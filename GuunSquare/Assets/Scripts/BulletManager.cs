using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject PlayerBullet;
    public GameObject EnemyBullet;
    public GameObject BossBullet;

    public List<BulletScript> lEnemyBullet;
    public List<BulletScript> lPlayerBullet;
    public List<BulletScript> lBossBullet;

    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < 150; i++)
        {
            lEnemyBullet.Add(Instantiate(EnemyBullet, this.transform).GetComponent<BulletScript>());
            lEnemyBullet[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < 50; i++)
        {
            lPlayerBullet.Add(Instantiate(PlayerBullet, this.transform).GetComponent<BulletScript>());
            lPlayerBullet[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < 30; i++)
        {
            lBossBullet.Add(Instantiate(BossBullet, this.transform).GetComponent<BulletScript>());
            lBossBullet[i].gameObject.SetActive(false);
        }

    }

}
