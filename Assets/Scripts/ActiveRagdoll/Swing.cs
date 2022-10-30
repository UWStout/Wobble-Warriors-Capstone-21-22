using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    //joints in the arm
    [SerializeField] ConfigurableJoint[] armBones;
    [SerializeField] Quaternion[] initialRotation;
    //joints of the swing rig (enabled when attacking)
    [SerializeField] Transform[] targetBones;
    //joints of the animated rig (enabled when not attacking)
    [SerializeField] Transform[] animatedBones;

    //attacking variables
    [SerializeField] float swingDuration = .5f;
    public Weapon weapon;
    [SerializeField] Transform root;

    private bool attacking = false;
    private bool telegraphing = false;
   [SerializeField] Transform telegraphTarget;
    private Vector3 currentTarget;
    public Transform TargetObject;

    private void Start()
    {
        //get initial rotation for all arm bones
        List<Quaternion> temp = new List<Quaternion>();
        foreach (ConfigurableJoint bone in armBones)
        {
            temp.Add(bone.transform.localRotation);
        }
        initialRotation = temp.ToArray();
    }

    void Update()
    {
        //Point arm towards target
            for (int i = 0; i < targetBones.Length; i++)
            {
                targetBones[i].rotation = Quaternion.FromToRotation(Vector3.right, targetBones[i].position - currentTarget);
                if (i == targetBones.Length - 1)
                {
                    targetBones[i].rotation = Quaternion.FromToRotation(Vector3.down, targetBones[i].position - currentTarget);
                }
            }

        //Update Target Rotations based on whether or not the player is swinging
        if (telegraphing)
        {
            if (telegraphTarget)
            {
                currentTarget = telegraphTarget.position;
            }
            else
            {
                currentTarget = root.position + Vector3.up * 2;
            }
           

            for (int i = 0; i < armBones.Length; i++)
            {
                //point towards target
                UpdateTargetRotations(armBones[i], targetBones[i], initialRotation[i]);
            }
        }
        else if (attacking)
        {

            //if the function did not find a proper target, it will return the root
            if (TargetObject == root)
            {
                //set the target to be straight in front of the player
                currentTarget = root.position + root.forward;
            }
            else
            {
                //set the target when a proper target was returned
                currentTarget = TargetObject.position;
            }

            currentTarget = new Vector3(currentTarget.x, armBones[0].transform.position.y, currentTarget.z);

            for (int i = 0; i < armBones.Length; i++)
            {
                //point towards target
                UpdateTargetRotations(armBones[i], targetBones[i], initialRotation[i]);
            }
        }
        else
        {
            for (int i = 0; i < armBones.Length; i++)
            {
                //animate with the rest of the rig
                UpdateTargetRotations(armBones[i], animatedBones[i], initialRotation[i]);
            }
        }

    }

    public void setTelegraphing(bool value)
    {
        telegraphing = value;
    }

    private IEnumerator SwingForDuration(Transform target)
    {
        TargetObject = target;
        //enable the weapon
        attacking = true;
        weapon.SetCanHit(true);

        //wait for the swing to be over
        yield return new WaitForSeconds(swingDuration);

        //reset attacking vars
        weapon.SetCanHit(false);
        attacking = false;
    }

    //Start the attack coroutine
    public void Attack(Transform target)
    {
        if (!attacking)
        {
            StartCoroutine(SwingForDuration(target));
        }
    }

    private void UpdateTargetRotations(ConfigurableJoint joint, Transform target, Quaternion initialRot)
    {
        // Calculate the joint's local rotation based on the axis and secondary axis
        var right = joint.axis;
        var forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
        var up = Vector3.Cross(forward, right).normalized;

        //Local "joint space" rotation
        Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

        // Transform into world space
        Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

        // Counter-rotate and apply new local rotation
        resultRotation *= Quaternion.Inverse(target.localRotation) * initialRot;

        // Transform back to joint space and apply target rotation
        resultRotation *= worldToJointSpace;
        joint.targetRotation = resultRotation;
    }
}
