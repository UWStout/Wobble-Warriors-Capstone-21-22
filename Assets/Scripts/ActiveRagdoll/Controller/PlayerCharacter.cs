using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerCharacter : CharacterMovement
{
    //multiplayer variables
    public int playerNumber;
    private bool keyboardPlayer = false;
    [SerializeField] SkinnedMeshRenderer mesh; //for changing color
    //used for ready up menu communication
    //public static UnityEvent<int> playerReady = new UnityEvent<int>();
    PlayerJoin playerRef; 
    //regen rate for all players
    static float regenRate = 3f;
    private float timeSinceDamage = 1;
    //jump variables
    [SerializeField] float jumpHeight;
    [SerializeField] Transform toes;
    [SerializeField] float maxGroundedDistance;

    [SerializeField] float dashLength = 1;
    [SerializeField] float dashCooldown = 3;
    private bool isDashing = false;
    private bool canDash = true;

    private float timeSinceRotateInput = 1;
    private bool useLook = false;
    private float timeHeldAttack = 0;

    private ControllerRumble rumbler;
    private bool groundpound = false;
    private Vector3 lastInput;
    private float chargeTime;
    public float minCargeTime = .75f;

    static bool doPlayerHealthScaling = true;
    static float[] healthValuesPerPlayer = { 150, 130, 115, 100 };
    public CircleHealthBar healthBar;

    //Boss related variables
    bool canTakeBossDmg = true;

    //pause related stuff
    PauseScript pauseMenu = null;

    //VFX stuff
    [SerializeField] GameObject reviveVFX;
    [SerializeField] GameObject gpVFX;
    [SerializeField] GameObject dashVFX;
    [SerializeField] GameObject ChargeVFX;

    [SerializeField] DragonSounds soundManager;

    public void OnEnable()
    {
        rumbler = GetComponent<ControllerRumble>();
        playerNumber = GetComponent<PlayerInput>().playerIndex;
        string playerLink = "player" + (playerNumber + 1);
        playerRef = FindObjectOfType<PlayerJoin>();
        findHealthBar();
        reloadHealth();
    }

    private void findHealthBar()
    {
        HealthbarManager manager = FindObjectOfType<HealthbarManager>();
        if(manager == null)
        {
            Debug.LogWarning("[PlayerCharacter] Health bar not found!");
            return;
        }
        healthBar = manager.getHealthbar(playerNumber);
        healthBar.character = this;
    }

    public void reloadHealth()
    {
        if (!Director)
        {
            Debug.LogWarning("[PlayerCharacter] Director not found!");
            return;
        }
        if (!isEnemy && doPlayerHealthScaling) //player health scaling
        {
            MaxHealth = healthValuesPerPlayer[Director.PlayerList.Count - 1];
            health = MaxHealth;
        }
        if (!healthBar)
        {
            findHealthBar();
        }
        healthBar.setMaxValue((int)health);
    }


    public void SetTimeSinceDamage(float value)
    {
        timeSinceDamage = value;
    }

    public void Revive(bool doVFX)
    {
        if (reviveVFX && doVFX)
        {
            Destroy(Instantiate(reviveVFX, this.transform), 2);
        }
        knockedOut = false;
        health = MaxHealth;
        ragdoll.SetAnimate(true);
        ragdoll.SetAnimate(true);
        Director.PlayerUnDied();
        soundManager.playRandomSound(soundManager.getReviveSounds(),1,1,.125f);
    }

    private void Update()
    {

        if(health <= 0 && !knockedOut)
        {
            soundManager.playRandomSound(soundManager.getDeathSounds(),1,1,.125f);
        }

        if (timeSinceRotateInput < 1 && !useLook)
        {
            timeSinceRotateInput += 1 * Time.deltaTime;
            if (GetComponent<PlayerInput>().currentControlScheme == "Keyboard") //MOUSE LOOKING
            {
                keyboardPlayer = true;
                MouseLook();
            }
        }

        if (isDashing)
        {
            SetMoveDirection(transform.forward * 10);
        }

        if(groundpound)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, Physics.gravity.y * 3, 0);

            if (isGrounded())
            {
                groundpound = false;

                if (rumbler)
                {
                    rumbler.Rumble(.5f, 1, 1);
                }
                if (gpVFX)
                {
                    Destroy(Instantiate(gpVFX, this.transform.position, Quaternion.identity), 2);
                }
                soundManager.playRandomSound(soundManager.GetGroundpoundSound(), 1,.75f,.25f);
                getSwingScript().weapon.SphereAttack(transform.position, 5,0,true);

                SetMoveDirection(lastInput);
            }
        }

        if (Director) //HEALTH REGEN
        {
            if(timeSinceDamage < 2)
            {
                timeSinceDamage += .25f * Time.deltaTime;
            }

            float playerCount = Director.PlayerList.Count;
            float newRegenRate = regenRate / Director.PlayerList.Count;

            if (health < MaxHealth) //regen
            {
                health += newRegenRate * Time.deltaTime * timeSinceDamage;
            }
            else if (health > MaxHealth) //Clamp health
            {
                health = MaxHealth;
            }
        }

    }

    public void EnvHit()
    {
        soundManager.playRandomSound(soundManager.getOofSounds(),.75f,1,.125f);
        if (rumbler)
        {
            rumbler.Rumble(.2f, .25f, .25f);
        }
    }

    public void WeaponHit()
    {
        soundManager.playRandomSound(soundManager.getDamageSounds(),1,1,.125f);
        if (rumbler)
        {
            rumbler.Rumble(.5f, 1, 1);
        }
    }

    //Sets the color of the dragon
    public void SetColor(Color color)
    {
        mesh.material.SetColor("Color_7ee8491a53b840cd84b0d170783f9866", color);
    }

    //handle inputs for moving
    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 v = value.ReadValue<Vector2>();
        Vector3 input = new Vector3(v.x, 0, v.y); //get input

        if (!knockedOut && !isDashing && !groundpound && timeSinceRotateInput >= 1 && !useLook && input.magnitude > .25f) //if it has been at least one second since rotated last, the joystick is in a neutral state, and the move joystic is not neutral
        {
            LookAt(transform.position + input);
        }

        if (!knockedOut && !isDashing && !groundpound) //dont move if not knocked out
        {
            SetMoveDirection(input);
        }
        else
        {
            input = lastInput;
        }
    }

    public bool isGrounded()
    {
        //raycast to see how far the player is from the ground
        RaycastHit hit;
        Ray downRay = new Ray(toes.position, -Vector3.up);

        if (Physics.Raycast(downRay, out hit))//do not jump if the player is already in the air
        {
            if (hit.distance > maxGroundedDistance) {
                return false;
            }
            else
            {
                return true;
            }
        }
            return false;
    }

    //handle inputs for jumping
    public void OnJump(InputAction.CallbackContext value)
    {
        //Director.shake(.1f, .5f);
        if (!value.performed || !isGrounded() || knockedOut) { return; }

        if (rumbler)
        {
            rumbler.Rumble(.125f, 1, 1);
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
    }

    public void OnGroundpound(InputAction.CallbackContext value)
    {
        if (knockedOut) { return; }
        if (!value.performed || isGrounded()) { return; }
        groundpound = true;
    }

    //handle inputs for changing character direction (controller only)
    public void OnLook(InputAction.CallbackContext value)
    {
        if (!isDashing && !groundpound)
        {
            Vector2 v = value.ReadValue<Vector2>();
            Vector3 input = new Vector3(v.x, 0, v.y); //get input

            timeSinceRotateInput = 0;

            if (GetComponent<PlayerInput>().currentControlScheme != "Keyboard")
            {
                if (input.magnitude < .25f)
                {
                    useLook = false;
                }
                else
                {
                    useLook = true;
                }

                if (!knockedOut && input != Vector3.zero && !keyboardPlayer) //no spin when ded
                {
                    LookAt(transform.position + input);
                }
            }
            else
            {
                useLook = false;
            }
        }
    }

    public void OnInteract()
    {
        //playerReady.Invoke(playerNumber + 1); //calls the event that allows the PlayerJoin script to register that a player is ready
    }

    //changing player direction (keyboard player only)
    private void MouseLook()
    {
        //raycast from mouse
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && !knockedOut)
        {
            LookAt(hit.point); //look at where the mouse is in worldspace
        }
    }

    //handle input for attack
    public void OnAttack(InputAction.CallbackContext value)
    {
        if (groundpound || knockedOut) { return; }

        if (value.started)
        {
            chargeTime = Time.time;
        }

        if (!value.canceled)
        {
            Telegraph(true);
            return;
        }

        if (rumbler)
        {
            rumbler.Rumble(.2f, .25f, .25f);
        }

        soundManager.playRandomSound(soundManager.getRawrSounds(),.5f,1,.25f);

        Telegraph(false);
        if (Time.time - chargeTime > minCargeTime)
        {

            getSwingScript().weapon.SphereAttack(transform.position + transform.forward, 3,1,false);
            Destroy(Instantiate<GameObject>(ChargeVFX, this.transform.position, this.transform.rotation), 2.0f);
        }
        
        Attack(GetTarget());

    }

    IEnumerator Dash()
    {
        isDashing = true;
        soundManager.playRandomSound(soundManager.getDashSound(),1,.75f,.25f);
        yield return new WaitForSeconds(dashLength);
        isDashing = false;
        SetMoveDirection(Vector3.zero);
    }
    IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void OnDash()
    {
        if (knockedOut) { return; }
        if (!isDashing && canDash && !groundpound)
        {
            Destroy(Instantiate<GameObject>(dashVFX, this.transform.position, this.transform.rotation), 2.0f);
            if (rumbler)
            {
                rumbler.Rumble(dashLength, .5f, .5f);
            }
            StartCoroutine(DashCooldown());
            StartCoroutine(Dash());
        }
    }

    //handle input for pausing
    public void OnPause()
    {
        if(pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseCanvas").GetComponent<PauseScript>();
        }
        pauseMenu.PauseGame(playerNumber);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            if (collision.gameObject.GetComponent<TestBossAI>().charging && canTakeBossDmg)
            {
                health -= 50;
                canTakeBossDmg = false;
                StartCoroutine(BossDamageCooldown());
            }
        }
    }

    IEnumerator BossDamageCooldown()
    {
        yield return new WaitForSeconds(2f);
        canTakeBossDmg = true;
    }
}
