using UnityEngine;

public class GravityButtonController : MonoBehaviour
{
    public Animator buttonAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            buttonAnimator.SetTrigger("Pressed");
            Physics.gravity = -Physics.gravity;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            buttonAnimator.SetTrigger("Released");
        }
    }
}
