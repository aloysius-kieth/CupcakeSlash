using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    float rotateFactor = 30f;
    bool rotateLeft;
    Rigidbody2D rb2d;

    public string type;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (Random.value > 0.5f) rotateLeft = true;
        else rotateLeft = false;
    }

    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "DeadZone") DeactivateSelf();
    }

    public virtual void OnHit()
    {
        GameObject particleEffect = null;
        if (type == POWERUPS.freezebomb)
        {
            particleEffect = ObjectPooler.Instance.GetPooledObject("FreezeExplosion");
        }
        else if (type == POWERUPS.feverbomb)
        {
            particleEffect = ObjectPooler.Instance.GetPooledObject("FeverExplosion");
        }
        else if (type == POWERUPS.cupcakeRainbow)
        {
            particleEffect = ObjectPooler.Instance.GetPooledObject("RainbowExplosion");
        }

        if (particleEffect != null)
        {
            particleEffect.transform.position = transform.position;
            particleEffect.SetActive(true);
        }
        DeactivateSelf();
    }

    public virtual void Update()
    {
        float rotateVal = /*rb2d.velocity.magnitude * */rotateFactor;
        if (rotateLeft) transform.Rotate(Vector3.back * rotateVal * Time.deltaTime);
        else transform.Rotate(Vector3.forward * rotateVal * Time.deltaTime);
    }

    public virtual void DeactivateSelf()
    {

        gameObject.SetActive(false);
    }
}
