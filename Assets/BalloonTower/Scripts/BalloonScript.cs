using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonScript : MonoBehaviour
{
    [SerializeField] Material[] materials;
    [SerializeField] private float growthValue;
    private Rigidbody balloonRb;
    private MeshRenderer meshRenderer;
    private LineRenderer lineRenderer;
    private SpringJoint joint;
    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.parent.position;
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        balloonRb= GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        joint = GetComponent<SpringJoint>();
        lineRenderer = GetComponent<LineRenderer>();
        
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = startPos;
        joint.maxDistance = 4f;
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;
        
        meshRenderer.material = materials[Random.Range(0, 5)];
    }

    // Update is called once per frame
    void Update()
    {
        balloonRb.AddForce(new Vector3(0f, .3f, 0f), ForceMode.Force);
        
        if(transform.localScale.x < 1f)
        {
            transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime * growthValue;
        }
        
        CreateRope();
    }

    private void CreateRope()
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, transform.position);

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }
}
