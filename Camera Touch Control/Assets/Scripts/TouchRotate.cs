using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    //Screen width variable
    float width;

    //To control which side we rotate our camera (-1 and 1, left and right)
    float axis = 0f;

    [Header("Release Speed")]
    public float releaseSpeedValue;
    float releaseSpeed;

    //Set a vector target
    [Header("Vector Variables")]
    public Transform target;
    public Vector3 camStartPos;
    public Vector3 camStartRot;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
    
        //Set camera position 
        transform.position = camStartPos;

        //Set camera rotation
        transform.eulerAngles = camStartRot;

        //If you want, you can find the target with "Target" tag
        //target = GameObject.FindGameObjectWithTag("Target").transform;
    }

    private void Update() {

        //If we are touching the screen,
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //Where is touch on world space
            Vector2 pos = touch.position;

            //This purePos is something between (0, x) (x is your max screen size)
            float purePos = pos.x;

            //This pos.x is something between (-1, 1)
            pos.x = (pos.x - width) / width;

            //Get where the touch begins, then set the axis and turn your camera to the right direction
            if(touch.phase == TouchPhase.Began){

                if(Mathf.Sign(pos.x) == 1 || Mathf.Sign(pos.x) == 0){

                    axis = -1;
                }

                else if(Mathf.Sign(pos.x) == -1){
                    
                    axis = 1;
                }
            }

            //Move the camera if the screen has the finger moving
            if (touch.phase == TouchPhase.Moved)
            {
                float absPosX = Mathf.Abs(pos.x);

                CheckTouch(purePos);

                //For rotating
                transform.RotateAround(target.transform.position, Vector3.up, axis * absPosX * 5f);

                releaseSpeed = releaseSpeedValue;
            }
        }

        //If there is no touch, start the slowly stop coroutine
        else if (Input.touchCount == 0){

            //If our releaseSpeed is 0, that means we don't have to keep Coroutine working, stop it
            if(releaseSpeed < 0.1f){

                StopCoroutine(SlowlyStop());
            }

            else if(releaseSpeed == releaseSpeedValue){

                StartCoroutine(SlowlyStop());
            }
        }
    }

    //See the pervious posx value, this means you can change the turn axis at any point on the screen
    float pervValue = 0;
    void CheckTouch(float currentValue){

        if(currentValue > pervValue){

            axis = 1f;
        }

        else if(currentValue < pervValue){

            axis = -1f;
        }

        pervValue = currentValue;
    }    

    //When you stop rotating with touch, rotation will stops slowly
    IEnumerator SlowlyStop(){

        while(releaseSpeed > 0){

            transform.RotateAround(target.transform.position, Vector3.up, axis * releaseSpeed * Time.deltaTime);

            releaseSpeed = releaseSpeed - (100f * Time.deltaTime);

            yield return null;
        }
    }
}
