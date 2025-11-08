using UnityEngine;

public class OneWayPlatorm : MonoBehaviour
{
    public bool isUp;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.parent.GetComponent<Collider2D>().enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            transform.parent.GetComponent<Collider2D>().enabled = isUp;
        }
    }
}