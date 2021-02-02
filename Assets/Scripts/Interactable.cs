using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Interactable : MonoBehaviour
{
    protected float minForce;
    protected float maxForce;

    protected float minSideForce;
    protected float maxSideForce;

    public SPAWN_DIRECTION spawnDir;
    public INTERACTABLE_TYPE _type;
    public Rigidbody2D rb2d;

    bool first = true;

    bool rotateRight = false;
    float rotateFactor = 7f;
    float bottomForceFactor = 2f;

    [SerializeField]
    private float _timeScale = 1f;
    public float timeScale
    {
        get { return _timeScale; }
        set
        {
            if (!first)
            {
                rb2d.mass *= timeScale;
                rb2d.velocity /= timeScale;
                rb2d.angularVelocity /= timeScale;
            }
            first = false;

            _timeScale = Mathf.Abs(value);

            rb2d.mass /= timeScale;
            rb2d.velocity *= timeScale;
            rb2d.angularVelocity *= timeScale;
        }
    }

    public virtual void Awake()
    {
        timeScale = _timeScale;
    }

    public virtual void OnEnable()
    {
        DoPushForce();
    }

    public virtual void OnDisable()
    {
        rb2d.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
    }

    public virtual void Update()
    {
        AutoRotate();
    }

    void AutoRotate()
    {
        float rotateVal = rb2d.velocity.magnitude * rotateFactor;

        if (spawnDir == SPAWN_DIRECTION.LEFT)
            transform.Rotate(Vector3.back * rotateVal * Time.deltaTime);
        else if (spawnDir == SPAWN_DIRECTION.RIGHT)
            transform.Rotate(Vector3.forward * rotateVal * Time.deltaTime);
        else if (spawnDir == SPAWN_DIRECTION.BOTTOM_LEFT || spawnDir == SPAWN_DIRECTION.BOTTOM_CENTER || spawnDir == SPAWN_DIRECTION.BOTTOM_RIGHT)
            if (!rotateRight) transform.Rotate(Vector3.forward * rotateVal * Time.deltaTime);
            else transform.Rotate(Vector3.back * rotateVal * Time.deltaTime);    
    }

    public virtual void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime * timeScale;
        rb2d.velocity += Physics2D.gravity * dt;

        //Debug.Log(rb2d.velocity);
    }

    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "DeadZone")
        {
            SpawnManager.Instance.inPlayList.Remove(gameObject);
            DeactivateSelf();
        }
    }

    public virtual void DoPushForce()
    {
        float y = Random.Range(minForce, maxForce);
        float x = Random.Range(minSideForce, maxSideForce);
        //rb2d.AddForce(new Vector2(randomSideForce, 1) * randomForce, ForceMode2D.Impulse);

        if (spawnDir == SPAWN_DIRECTION.LEFT)
            rb2d.AddRelativeForce(new Vector2(x, y), ForceMode2D.Impulse);
        else if (spawnDir == SPAWN_DIRECTION.RIGHT)
            rb2d.AddRelativeForce(new Vector2(-x, y), ForceMode2D.Impulse);
        else if (spawnDir == SPAWN_DIRECTION.BOTTOM_LEFT)
        {
            rb2d.AddRelativeForce(new Vector2(3, 24), ForceMode2D.Impulse);
            rotateRight = true;
        }
        else if (spawnDir == SPAWN_DIRECTION.BOTTOM_CENTER)
        {
            rb2d.AddRelativeForce(new Vector2(0, 24), ForceMode2D.Impulse);
            if (Random.value > 0.5f) rotateRight = false;
            else rotateRight = true;
        }
        else if (spawnDir == SPAWN_DIRECTION.BOTTOM_RIGHT)
        {
            rb2d.AddRelativeForce(new Vector2(-3, 24), ForceMode2D.Impulse);
            rotateRight = false;
        }
        //Debug.Log(this.name+" | "+rb2d.velocity);

        //if (spawnLeft) rb2d.velocity = new Vector3(randomSideForce, randomForce, 0);
        //else rb2d.velocity = new Vector3(-randomSideForce, randomForce, 0);

        Vector3 dir = rb2d.velocity.normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
    }

    public virtual void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }
}
