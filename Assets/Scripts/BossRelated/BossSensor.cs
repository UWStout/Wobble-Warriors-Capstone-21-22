using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BossSensor : MonoBehaviour
{
    public float distance = 10; //the distance in which the agent can see within their field of view
    public float angle = 30; //the field of view of the agent, with 0 being unable to see anything, and 180 being able to see all around them
    public float height = 1.0f; //the height at which the field of view can see
    public Color meshColor = Color.red; //the color of the sight cone
    public Color seenColor = Color.green;
    public int scanFrequency = 30; //the frequency at which scans are made
    public LayerMask layers; //the layers that the agent can see and interact with (e.g. the player)
    public LayerMask occlusionLayers; //the layers in which the agent cannot see throught (e.g. walls)
    public List<GameObject> Objects = new List<GameObject>();  //the list of objects the enemy is currently able to see

    Collider[] colliders = new Collider[50]; //a list of colliders for the sight line
    Mesh mesh; //the mesh for the sightline
    int count; //this i actually do not know
    float scanInterval; //the interval at which scans happen
    float scanTimer; //timer to know when the next scan should happen

    public GameObject boss;

    void Awake()
    {
        scanInterval = 1.0f / scanFrequency;
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        //subtract delta time from the scan timer
        scanTimer -= Time.deltaTime;
        //if the scan timer is at zero, add the scan interval value to its value, and then run the scan function
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        //set count to, I do not know
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        //clear the Objects list
        Objects.Clear();
        //for each number in count
        for (int i = 0; i < count; i++)
        {
            //create a gameobject obj, and set its value to the current i index in colliders
            GameObject obj = colliders[i].gameObject;
            //the the player in sight variable in the BossAI script to the value of isInSight with obj as the parameter
            //boss.GetComponent<BossAI>().playerInSight = IsInSight(obj);
            //boss.GetComponent<TestBossAI>().playerInSight = IsInSight(obj);
            boss.GetComponent<TestBossAI>().longRangeSight = IsInSight(obj);
            //if isInSight with obj as the parameter returns true, add obj to the Objects list
            if (IsInSight(obj))
            {
                Objects.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position; //the location of the agent
        Vector3 dest = obj.transform.position; //the position of the input obj parameter
        Vector3 direction = dest - origin; //the direction from the agent to the parameter object

        //if the y direction of the object is greater than the height of the agents sightline, or less than zero, return false
        if (direction.y > height)
        {
            return false;
        }

        //if the z direction of the object is greater than the sight distance of the agent, return false
        if (direction.magnitude > distance - 2)
        {
            return false;
        }

        //set the y direction of the object to zero, and create a float called deltaAngle, with its value set to the angle created by the direction of the object and a line pointing directly forward from the agent
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        //if the delta angle is greater than the sightline angle, return false
        if (deltaAngle > angle)
        {
            return false;
        }

        //add half of the height to the agents y position, and set the objects y position to the agents y position
        origin.y += height / 2;
        dest.y = origin.y;
        //if the line created between the agent and the object touches an occluding layer, return false
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh(); //the mesh used to represent the sight cone

        int segments = 10; //the number of segments in the sight cone
        int numTriangles = (segments * 4) + 2 + 2; //the number of triangles in the sight cone, so that angles work properly
        int numVertices = numTriangles * 3; //the number of vertices in the sight cone

        Vector3[] vertices = new Vector3[numVertices]; //the array of vertices 
        int[] triangles = new int[numVertices]; //the array of triangles

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side 
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }


        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = seenColor;
        foreach (var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}