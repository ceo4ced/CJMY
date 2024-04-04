using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamageScript : MonoBehaviour
{
    [SerializeField] private float launchPower = 15.0f;
    private CharacterController character;
    Vector3 impact = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }
    void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0)
        {
            dir.y = -dir.y; // reflect down force on the ground
        }
        impact += dir.normalized * force;
        
    }

    // Update is called once per frame
    void Update()
    {
        // apply the impact force:
        if (impact.magnitude > 0.2) character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    public void ReceiveDamage(Vector3 dir)
    {
        Debug.Log("took damage");
        Rigidbody rb = GetComponent<Rigidbody>();
        CharacterController cc = GetComponent<CharacterController>();
        if (rb != null && cc != null)
        {
            //Debug.Log("Fly");
            //Debug.Log(rb);
            AddImpact(dir, launchPower);
        }
    }
}
