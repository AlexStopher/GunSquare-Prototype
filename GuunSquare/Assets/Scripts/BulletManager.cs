using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject PlayerBullet;
    public GameObject EnemyBullet;

    public List<BulletScript> lEnemyBullet;
    public List<BulletScript> lPlayerBullet;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 150; i++)
        {
            lEnemyBullet.Add(Instantiate(EnemyBullet, this.transform).GetComponent<BulletScript>());
            lEnemyBullet[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < 30; i++)
        {
            lPlayerBullet.Add(Instantiate(PlayerBullet, this.transform).GetComponent<BulletScript>());
            lPlayerBullet[i].gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
