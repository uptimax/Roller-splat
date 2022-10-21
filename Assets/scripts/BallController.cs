using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 1;
    private bool isTraveling;
    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;

    public int minSwipeRecognition = 500;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;

    private bool InitialSwipe;
    private Color solveColor;

    private void Start(){
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
        // audioManager = FindObjectOfType<AudioManager>();

    }


    private void FixedUpdate(){
        if(isTraveling){
        rb.velocity = speed * travelDirection;
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 3), 0.15f);
        int i = 0;
        while(i < hitColliders.Length){
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if(ground && !ground.isColored){
                ground.ChangeColor(solveColor);
            }
            i++;
        }

        if(nextCollisionPosition != Vector3.zero){
            if(Vector3.Distance(transform.position, nextCollisionPosition) < 1){
                isTraveling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
                if(InitialSwipe)
                GameManager.singleton.scoreMultiplier = 0;
                GetComponent<AudioSource>().Play();
            }
        }
             
        
        if(isTraveling)
           return;

        if(Input.GetMouseButton(0)){
            InitialSwipe = true;
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if(swipePosLastFrame != Vector2.zero){
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

            if(currentSwipe.sqrMagnitude < minSwipeRecognition){
                return;
            }

            currentSwipe.Normalize();

            //UP/Down
            if(currentSwipe.x > -0.5f && currentSwipe.x < 0.5){
                //GO UP/Down
                SetDesitination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
            }

            if(currentSwipe.y > -0.5f && currentSwipe.y < 0.5){
                //GO Left/right
                // Debug.Log("setting distination");
                SetDesitination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
            }

            }

        swipePosLastFrame = swipePosCurrentFrame;

        }

        if(Input.GetMouseButtonUp(0)){
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }


    }

    private void SetDesitination(Vector3 direction){
        travelDirection = direction;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 100f)){
            nextCollisionPosition = hit.point;
        }

        isTraveling = true;
    }

}
