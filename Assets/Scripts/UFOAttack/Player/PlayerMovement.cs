using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;

    public Coroutine speedCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(-7f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -8.1)
        {
            transform.Translate(-speed * Time.deltaTime,0,0);
        } else if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 8.1)
        {
            transform.Translate(speed * Time.deltaTime,0,0);
        }

        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < 2.9)
        {
            transform.Translate(0,speed * Time.deltaTime,0);
        } else if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -4.44)
        {
            transform.Translate(0,-speed * Time.deltaTime, 0);
        }
    }

    public void IncreaseSpeed(float multiplier, float duration)
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
        }

        speedCoroutine = StartCoroutine(SpeedBonus(multiplier, duration));
    }

    private IEnumerator SpeedBonus(float speedMultiplier, float duration)
    {
        speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        speed /= speedMultiplier;
    }
}