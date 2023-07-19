using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float speed = 8;
    [SerializeField] float rotateSpeed;

    [SerializeField] Swing SwingScript;
    public ActiveRagdoll ragdoll;

    public Director Director;

    public float MaxHealth = 100;
    public float health = 100;
    public bool knockedOut = false;
    [SerializeField] float ragdollDuration = 3;

    public bool isEnemy = false;

    public Vector3 moveVector;

    private Quaternion offset;

    private Quaternion targetRotation;
    private Rigidbody rb;

    public List<Transform> targets;
    [SerializeField] float range = 5;
    [SerializeField] float FOV = 360;
    [SerializeField] float attackCooldown = 0f;
    bool iCanAttack = true;
    private AudioSource source;
    [SerializeField] AudioClip swingSound;

    public Animator animator;

    public ActiveRagdoll getRagdoll()
    {
        return ragdoll;
    }

    public Swing getSwingScript()
    {
        return SwingScript;
    }

    public void getRB()
    {
        rb = GetComponent<Rigidbody>();
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
        set { targetRotation = value; }
    }

    private void Start()
    {
        offset = transform.rotation;
        source = GetComponentInParent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        if (animator && GetComponent<TestBossAI>() == null)
        {
            animator.SetFloat("speed", 0);
        }
        Director = FindObjectOfType<Director>();

        UpdateTargets();
    }

    public void Telegraph(bool isTelegraphing)
    {
        SwingScript.setTelegraphing(isTelegraphing);
    }

    void FixedUpdate()
    {

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation * offset, rotateSpeed * Time.deltaTime); //smoothly rotate to target rotation
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);

        if (health <= 0)
        {
            if (!knockedOut)
            {
                if (isEnemy)
                {
                    Director.GobDied();
                }
                else
                {
                    Debug.Log("IM DED");
                    Director.PlayerDied();
                }

                knockedOut = true;
                moveVector = Vector3.up * Physics.gravity.y;
                ragdoll.RagdollForDuration(ragdollDuration);
                Debug.Log("Dead...");
            }
        }
    }

    public Transform GetTarget()
    {
        UpdateTargets();
        if (targets.Count == 0)
        {
            return transform;
        }

        float closestDistance = range;
        int closest = -1;

        for (int i = 0; i < targets.Count; i++)
        {
            CharacterMovement target = targets[i].GetComponent<CharacterMovement>();

            //target must be the closest non-knocked-out target within FOV
            if (Vector3.Distance(transform.position, targets[i].position) < closestDistance //is closer
                && (target == null || !target.knockedOut)//isnt knocked out
                && Mathf.Abs(Vector3.Angle(targets[i].position - transform.position, transform.forward)) < FOV //within fov
                && targets[i].gameObject.activeSelf) //is active
            {
                closest = i;
                closestDistance = Vector3.Distance(transform.position, targets[i].position);
            }
        }

        if(closest == -1)
        {
            return transform;
        }

        //return target if found
        return targets[closest];
    }

    public void UpdateTargets()
    {
        bool enemySearch = !transform.GetComponent<CharacterMovement>().isEnemy; //true if targets are enemies

        CharacterMovement[] characters = GameObject.FindObjectsOfType<CharacterMovement>();
        List<Transform> temp = new List<Transform>();

        for (int i = 0; i < characters.Length; i++)
        {
            if (enemySearch && characters[i].isEnemy)//if target is an enemy and we are looking for enemies
            {
                temp.Add(characters[i].transform);
            }
            else if (!enemySearch && !characters[i].isEnemy) //if target isnt an enemy and we arent looking for enemies
            {
                temp.Add(characters[i].transform);
            }
        }

        targets = temp;
    }

    public void Attack(Transform target)
    {
        if (iCanAttack)
        {
            if (!knockedOut)
            {
                if (source && swingSound)
                {
                    source.pitch = Random.Range(.75f, 1.25f);
                    source.PlayOneShot(swingSound);
                    source.pitch = 1;
                }
                SwingScript.Attack(target);
                StartCoroutine(ManageAttackCooldown());
            }
        }
        else
        {
            iCanAttack = true;
        }
    }

    public void SetMoveDirection(Vector3 moveDirection)
    {
        //Ignore y direction and multiply by speed
        moveVector = new Vector3(moveDirection.x * speed,rb.velocity.y,moveDirection.z *speed);
        if (animator && GetComponent<TestBossAI>() == null)
        {
            animator.SetFloat("speed", moveVector.magnitude);
        }
    }

    public void MoveTowards(Vector3 target)
    {
        Vector3 dirVec = Vector3.Normalize(target - transform.position);
        moveVector = new Vector3(dirVec.x * speed, rb.velocity.y, dirVec.z * speed);
        if (animator && GetComponent<TestBossAI>() == null)
        {
            animator.SetFloat("speed", moveVector.magnitude);
        }
    }

    public void SetOrientation(Vector3 targetVector)
    {
        //ignore y movement -- only rotate on y axis
        targetRotation = Quaternion.LookRotation(new Vector3(targetVector.x,0,targetVector.z), Vector3.up);

        updateRootJoint(targetRotation);
    }

    public void LookAt(Vector3 target)
    {
        Vector3 dirVec = Vector3.Normalize(target - transform.position);
        //ignore y movement -- only rotate on y axis
        targetRotation = Quaternion.LookRotation(new Vector3(dirVec.x, 0, dirVec.z), Vector3.up);

        if (GetComponent<ConfigurableJoint>())
        {
            updateRootJoint(targetRotation);
        }
        
    }
    public IEnumerator ManageAttackCooldown()
    {
        iCanAttack = false;
        float cooldown = attackCooldown;
        while(cooldown>0)
        {
            cooldown -= Time.fixedDeltaTime;
        }
        iCanAttack = true;
        yield return 0;
    }
    private void updateRootJoint(Quaternion targetRotation)
    {
        // Calculate the joint's local rotation based on the axis and secondary axis
        var right = GetComponent<ConfigurableJoint>().axis;
        var forward = Vector3.Cross(GetComponent<ConfigurableJoint>().axis, GetComponent<ConfigurableJoint>().secondaryAxis).normalized;
        var up = Vector3.Cross(forward, right).normalized;

        //Local "joint space" rotation
        Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

        // Transform into world space
        Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

        // Counter-rotate and apply new local rotation
        resultRotation *= Quaternion.Inverse(targetRotation) * Quaternion.identity;

        // Transform back to joint space and apply target rotation
        resultRotation *= worldToJointSpace;
        GetComponent<ConfigurableJoint>().targetRotation = resultRotation;
    }
}
