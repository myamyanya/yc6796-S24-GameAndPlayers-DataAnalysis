using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    Animator animator;

    // For gift position offset
    public float giftOffsetX;
    public float giftOffsetZ;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetInteger("AnimationState", 1);

            if (Input.GetKey(KeyCode.F))
            {
                //Debug.Log("Heeeet");
                animator.SetInteger("AnimationState", 2);
            }
        }
    }
}
