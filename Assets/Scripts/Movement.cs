using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength=100f;
    [SerializeField] float rotationStrength=100f;
    [SerializeField] AudioClip MainEnginSfx;
    [SerializeField] ParticleSystem MainEnginParticale;
    [SerializeField] ParticleSystem RightThrustParticale;
    [SerializeField] ParticleSystem LeftThrustParticale;

    AudioSource audioSource;
    Rigidbody rb;

    private void Start()
    {
        rb=GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }


    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    { 
        if(thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(MainEnginSfx);
            }
            if(! MainEnginParticale.isPlaying){
            MainEnginParticale.Play(); // This is for thurst Particale activation of main engine Particale.
            }
        }
        else
            {
                audioSource.Stop();
                MainEnginParticale.Stop();// this is for stoping particale otherwise it will never stops.
            }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        
        if(rotationInput<0)
        {
            ApplyRotation(rotationStrength);
            
            if(! RightThrustParticale.isPlaying){
                 LeftThrustParticale.Stop();
                RightThrustParticale.Play(); // This is for thurst Particale activation of main engine Particale.
            }

        }
        else if (rotationInput>0)
        {
            ApplyRotation(-rotationStrength );

            if(! LeftThrustParticale.isPlaying){
                RightThrustParticale.Stop();
            LeftThrustParticale.Play(); // This is for thurst Particale activation of main engine Particale.
            }
            
        }
        else
        {
            RightThrustParticale.Stop();
            LeftThrustParticale.Stop();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}
