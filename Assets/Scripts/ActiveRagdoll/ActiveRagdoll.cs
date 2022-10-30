using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdoll : MonoBehaviour
{
    //animated joints (arms, legs)
    [SerializeField] ConfigurableJoint[] joints;
    //non animated joints (spine, head)
    [SerializeField] ConfigurableJoint[] passiveJoints;
    //animated rig joints
    [SerializeField] Transform[] targets;
    [SerializeField] Quaternion[] initialRotation;
    //rig and animated rig pelvises
    [SerializeField] Transform root;
    [SerializeField] Transform targetRoot;
    //spring settings
    [SerializeField] float springForce = 15000;
    [SerializeField] float damper = 10;

    [SerializeField] CharacterMovement controller;
    private bool animate = true;
    public bool hasDied = false;

    public int getRigSize()
    {
        return targets.Length;
    }

    public ConfigurableJoint GetJoint(int index)
    {
        return joints[index];
    }

    private void Start()
    {
        animate = true;
        SetSpringForce(springForce, damper);
        ConstructSkeleton();
        IgnoreInnerCollision(true);
    }

    private IEnumerator Ragdoll(float duration)
    {
        SetAnimate(false);

        yield return new WaitForSeconds(duration); //wait the full duration
        
        //reset player variables, but disable enemies
        if (!controller || controller.GetComponent<PlayerCharacter>() == null)
        {
            hasDied = true;
            if (controller.GetComponent<TestBossAI>())
            {
                transform.parent.gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
        }
        else
        {
            SetAnimate(true);
            controller.knockedOut = false;
            controller.health = controller.MaxHealth;
            controller.Director.PlayerUnDied();
            controller.animator.SetFloat("speed", 0);
        }
    }

    public void SetAnimate(bool animateRig)
    {
        if (animateRig)
        {
            SetSpringForce(springForce, damper); //reset spring duration
        }
        else
        {
            SetSpringForce(200, damper); //reduce the spring force
        }

        animate = animateRig;
    }

    //disable the animation for x amount of seconds
    public void RagdollForDuration(float duration)
    {
        StopAllCoroutines();

        StartCoroutine(Ragdoll(duration));
    }

    //return the root (pelvis) of the rig
    public Transform getRoot()
    {
        return root;
    }

    public Transform getAnimatedRoot()
    {
        return targetRoot;
    }

    public float getSpringForce()
    {
        return springForce;
    }

    public CharacterMovement getCharacter()
    {
        return controller;
    }

    //Sets whether or not colliders in the rig will collide with eachother
    private void IgnoreInnerCollision(bool enable)
    {
        Collider[] colliders = root.GetComponentsInChildren<Collider>();

        for(int i = 0; i < colliders.Length; i++)
        {
            for (int j = 0; j < colliders.Length; j++)
            {
                Physics.IgnoreCollision(colliders[i], colliders[j],enable);
            }
        }

    }

    //find the reference transforms on the animated rig
    public void ConstructSkeleton()
    {
        //Find joints decendant to root if player hasn't entered specific joints
        if (joints.Length == 0)
        {
            joints = root.GetComponentsInChildren<ConfigurableJoint>();
        }

        //Get all decendants of the target root
        Transform[] temp = targetRoot.GetComponentsInChildren<Transform>();

        //Resize arrays to fit # of joints
        targets = new Transform[joints.Length];
        initialRotation = new Quaternion[joints.Length];

        //Find the corresponding target for each joint
        for (int i = 0; i < joints.Length; i++)
        {

            //keep track of each joint's initial local rotation
            initialRotation[i] = joints[i].transform.localRotation;

            for (int j = 0; j < temp.Length; j++)
            {
                //bones all have distinct names, so we use those to find the "relatives"
                if (temp[j].name == joints[i].name)
                {
                    targets[i] = temp[j];
                    break;
                }
            }
        }

        //Finalize the targets array
    }

    //set the target rotations of the configurable joints
    private void UpdateTargetRotations()
    {
        for(int i = 0; i < joints.Length; i++)
        {
            // Calculate the joint's local rotation based on the axis and secondary axis
            var right = joints[i].axis;
            var forward = Vector3.Cross(joints[i].axis, joints[i].secondaryAxis).normalized;
            var up = Vector3.Cross(forward, right).normalized;

            //Local "joint space" rotation
            Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

            // Transform into world space
            Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

            // Counter-rotate and apply new local rotation
            resultRotation *= Quaternion.Inverse(targets[i].localRotation) * initialRotation[i];

            // Transform back to joint space and apply target rotation
            resultRotation *= worldToJointSpace;
            joints[i].targetRotation = resultRotation;
        }
    }

    //set all configurable joint forces
    public void SetSpringForce(float force,float damper)
    {
        //New configurable joint drive with the desired values
        //Unity doesn't let you modify the existing joint drive
        JointDrive newDrive = new JointDrive();
        newDrive.positionSpring = force;
        newDrive.positionDamper = damper;
        newDrive.maximumForce = Mathf.Infinity;

        foreach (ConfigurableJoint joint in joints)
        {
            //replace the values for each joint in the skeleton
            joint.angularXDrive = newDrive;
            joint.angularYZDrive = newDrive;
        }

        foreach (ConfigurableJoint joint in passiveJoints)
        {
            //replace the values for each joint in the skeleton
            joint.angularXDrive = newDrive;
            joint.angularYZDrive = newDrive;
        }

    }

    void LateUpdate()
    {
        if (animate)
        {
            UpdateTargetRotations();
        }
    }
}
