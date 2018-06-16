using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrait : MonoBehaviour {

    public GameObject bullet;

    GameObject m_Bullet;

    public void Shoot(bool positive)
    {
        if (bullet)
            m_Bullet = Instantiate(bullet, transform.position, Quaternion.identity);

        if (m_Bullet)
            m_Bullet.GetComponent<BulletBasic_Ai>().SetDirection(positive);
    }
}
