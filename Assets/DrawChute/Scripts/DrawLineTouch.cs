using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class DrawLineTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Material lrMaterial;
    [SerializeField] Camera cam;
    [SerializeField] Transform cubePrefabTransform;
    [SerializeField] Transform parachuteParentTransform;

    private GameObject parachuteObject;
    private int drawCount;
    private bool startDrawing;
    private Vector3 touchPosition;
    private LineRenderer lineRenderer;
    private int lrIndex;
    private Transform lastObjectTransform;
    private GameObject gameObjectLast;
    
    
    
    public void OnPointerDown(PointerEventData eventData)
    {
        startDrawing = true;
        
        if (gameObjectLast != null)
        {
            gameObjectLast.SetActive(false);
        }
        
        touchPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)).z);

        lineRenderer = parachuteObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 1f;
        lineRenderer.material = lrMaterial;
        drawCount++;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        startDrawing = false;
        
        Rigidbody rb = parachuteObject.AddComponent<Rigidbody>();
        MeshRenderer meshRenderer = parachuteObject.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = parachuteObject.AddComponent<MeshCollider>();
        
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;

        if (lineRenderer != null)
        { 
            lineRenderer.useWorldSpace = false;
        }

        if (lastObjectTransform != null)
        {
            Destroy(lastObjectTransform.gameObject);
        }
        
        parachuteObject = new GameObject();
        parachuteObject.gameObject.name = "ParachuteObject";
        parachuteObject.transform.parent = parachuteParentTransform;
        
        lrIndex = 0;

    }
    void Start()
    {
        parachuteObject = new GameObject();
        parachuteObject.gameObject.name = "ParachuteObject" + (drawCount);
        parachuteObject.transform.parent = parachuteParentTransform;
    }
    void FixedUpdate()
    {
            if (startDrawing)
            {
                if (Input.touchCount > 0)
                {
                Touch touch = Input.GetTouch(0);
                Vector3 dist = touchPosition - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                float distSqr = dist.sqrMagnitude;
                if (distSqr > 1000f)
                {
                    lineRenderer.SetPosition(lrIndex, cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 5f)));
                    if (lastObjectTransform != null)
                    {
                        Vector3 currentLinePos = lineRenderer.GetPosition(lrIndex);
                        lastObjectTransform.gameObject.SetActive(true);
                        lastObjectTransform.LookAt(currentLinePos);

                        if (lastObjectTransform.rotation.y == 0)
                        {
                            lastObjectTransform.eulerAngles = new Vector3(lastObjectTransform.rotation.eulerAngles.x, 90, lastObjectTransform.rotation.eulerAngles.z);
                        }

                        lastObjectTransform.localScale = new Vector3(lastObjectTransform.localScale.x, lastObjectTransform.localScale.y, Vector3.Distance(lastObjectTransform.position, currentLinePos) * 0.6f);
                    }

                    lastObjectTransform = Instantiate(cubePrefabTransform, lineRenderer.GetPosition(lrIndex), Quaternion.identity, parachuteObject.transform);

                    Debug.Log(drawCount);
                    parachuteObject.gameObject.name = "ParachuteObject" + drawCount;
                    gameObjectLast = GameObject.Find("ParachuteObject" + (drawCount));
                    lastObjectTransform.position = new Vector3(lastObjectTransform.position.x, lastObjectTransform.position.y +30f, lastObjectTransform.position.z + 20f);
                    //lastObjectTransform.gameObject.SetActive(false);
                    touchPosition = touch.position;

                    lrIndex++;
                    lineRenderer.positionCount = lrIndex + 1;
                    lineRenderer.SetPosition(lrIndex, cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y,10f)));
                }
            }
        }
    }
}
