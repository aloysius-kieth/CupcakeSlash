using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BladeController : MonoBehaviour
{
    public static Action OnGameOver;
    public static Action<Cupcake> OnPlayerScored;

    public GameObject bladeTrailPrefab;

    private const float minCuttingVelocity = 40f;

    public Vector2 PreviousPos { get; private set; }
    public Vector3 CutPos { get; private set; }
    public bool IsCutting { get; private set; }
    public bool useMouse = false;

    float touchTime = 0;
    const float MIN_DELAY_TOUCH = 0.1f;

    Rigidbody2D rb;
    Camera camera;
    GameObject bladeTrailInstance;
    CircleCollider2D bladeCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bladeCollider = GetComponent<CircleCollider2D>();

        camera = Camera.main;
        IsCutting = false;
        bladeCollider.enabled = false;
    }

    public void PopulateValues(GlobalSettings settings)
    {
        useMouse = settings.useMouse;
    }

    void Update()
    {
        if (useMouse)
        {
            if (Input.GetMouseButtonDown(0)) StartCutting();
            else if (Input.GetMouseButtonUp(0)) StopCutting();
        }
        else
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchTime = Time.time;
                    //Debug.Log("Touch time: " + touchTime);
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    //Debug.Log("moved");
                    float lastTouchTime = Time.time - touchTime;
                    if (lastTouchTime < MIN_DELAY_TOUCH)
                    {
                        //Debug.Log("tapping too fast");
                        return;
                    }
                    else
                    {
                        StartCutting();
                    }

                    //Debug.Log(lastTouchTime);
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    StopCutting();
                    touchTime = 0;
                }
            }
        }

        // if blade is currently cutting, update cutting status of blade
        if (IsCutting) CalculateCutting();
    }

    void StartCutting()
    {
        IsCutting = true;
        bladeTrailInstance = Instantiate(bladeTrailPrefab, transform);
        bladeTrailInstance.GetComponent<TrailRenderer>().Clear();
        PreviousPos = camera.ScreenToWorldPoint(Input.mousePosition);
        bladeCollider.enabled = true;
    }

    public void StopCutting()
    {
        IsCutting = false;
        collidedCount = 0;

        if (bladeTrailInstance != null)
        {
            bladeTrailInstance.transform.SetParent(null);
        }

        Destroy(bladeTrailInstance, 1f);
        bladeCollider.enabled = false;
    }

    bool isPlayingSwing = false;
    void CalculateCutting()
    {
        Vector2 newPos = camera.ScreenToWorldPoint(Input.mousePosition);
        rb.position = newPos;
        // Get the vector where the fruit was cut
        CutPos = (newPos - PreviousPos);
        //Debug.Log(CutPos);

        // Get the magnitude of the position that was cut
        float distance = CutPos.magnitude;
        //Debug.Log(distance);

        // Get the velocity based on dividing the magnitude by time
        float velocity = distance / Time.deltaTime;

        // Enable blade collider if velocity goes above threshold
        bladeCollider.enabled = (velocity > minCuttingVelocity) ? true : false;

        if (!isPlayingSwing && velocity > minCuttingVelocity)
        {
            isPlayingSwing = true;
            int randIndex = UnityEngine.Random.Range((int)TrinaxAudioManager.AUDIOS.SWING1, (int)TrinaxAudioManager.AUDIOS.SWING3);
            TrinaxAudioManager.Instance.PlaySFX((TrinaxAudioManager.AUDIOS)randIndex, TrinaxAudioManager.AUDIOPLAYER.SFX);
        }
        if (velocity < minCuttingVelocity)
        {
            isPlayingSwing = false;
        }

        // Update previous position of blade
        PreviousPos = newPos;
    }

    Vector2 startPos;
    Vector2 endPos;
    Vector2 calculatedPos;

    public bool hasHitBomb = false;
    int collidedCount = 0;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bomb")
        {
            collidedCount = 0;
            hasHitBomb = true;
            //StopCutting();
            ScoreManager.Instance.MinusScore(col.gameObject.GetComponent<Bomb>());
            GameManager.Instance.bgChanger.DoShake();
            //OnGameOver?.Invoke();
        }
        else if (col.gameObject.tag == "FreezeBomb")
        {
            col.gameObject.GetComponent<FreezeBomb>().OnHit();
        }
        else if (col.gameObject.tag == "FeverBomb")
        {
            col.gameObject.GetComponent<FeverBomb>().OnHit();
        }
        else if (col.gameObject.tag == "Cupcake")
        {
           // ScoreManager.Instance.StartCombo();
            ActivateExplodeParticle(col.transform.position, col);

            if (OnPlayerScored != null) OnPlayerScored(col.gameObject.GetComponent<Cupcake>());

            col.gameObject.SetActive(false);

            TrinaxAudioManager.Instance.PlaySFX(TrinaxAudioManager.AUDIOS.CUPCAKEHIT, TrinaxAudioManager.AUDIOPLAYER.SFX2);
        }
        else if (col.gameObject.tag == "CupcakeRainbow")
        {
            col.gameObject.GetComponent<CupcakeRainbow>().OnHit();
        }
        //else if (col.gameObject.tag == "Bonus")
        //{
        //    startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Debug.Log("startpos " + startPos);
        //}
        if (col.gameObject.tag == "Cupcake")
        {
            collidedCount++;
            if (SpawnManager.Instance.cupcakeList.Count == collidedCount)
            {
                if(collidedCount == 1)
                {
                    collidedCount = 0;
                    return;
                }
                ScoreManager.Instance.Score += collidedCount;
                ScoreManager.Instance.UpdateText();
                ScoreManager.Instance.comboDisplay.UpdateCombo(collidedCount, col.transform.position);
                //Debug.Log("Score w/ combo: " + ScoreManager.Instance.Score);
                collidedCount = 0;
            }
            //Debug.Log("Count: " + collidedCount);
        }
    }

    int cutCount;
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.tag == "Bonus")
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("endPos " + endPos);
            calculatedPos = endPos - startPos;

            Debug.Log("calculatedPos " + calculatedPos);
            float lengthOfCut = calculatedPos.sqrMagnitude;
            Debug.Log("lengthofcut " + lengthOfCut);
            if (lengthOfCut > col.transform.GetComponent<BonusCupcake>().diameter)
            {
                cutCount++;
                startPos = Vector2.zero;
                endPos = Vector2.zero;
                calculatedPos = Vector2.zero;
                lengthOfCut = 0;
                Debug.Log("Cut: " + cutCount);
            }
        }
    }

    void ActivateExplodeParticle(Vector2 position, Collision2D col)
    {
        GameObject spawnExplosion = null;
        GameObject spawnPointPS = null;

        spawnExplosion = ObjectPooler.Instance.GetPooledObject("CupcakeExplode");
        spawnPointPS = ObjectPooler.Instance.GetPooledObject("10PointPS");

        if (spawnExplosion != null && spawnPointPS != null)
        {
            spawnExplosion.transform.position = position;
            spawnExplosion.SetActive(true);
            spawnExplosion.GetComponentInChildren<ParticleSystem>().Play();
            spawnPointPS.SetActive(true);
            spawnPointPS.GetComponent<ParticleSystem>().Play();
        }
    }
}
