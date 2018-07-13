using UnityEngine;

public class MeleeTrait : MonoBehaviour {

    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {

       KillableTrait killable = collision.gameObject.GetComponent<KillableTrait>();

       if (killable)
           killable.Damage(damage);
    }
}
