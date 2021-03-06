﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEvents : MonoBehaviour {
    bool bunnyIsTapped;
    GameObject touchedBunny;
    Vector3 touchPosWorld;
    Vector3 offset;

	// Use this for initialization
	void Start () {
        bunnyIsTapped = false;
	}

    // Update is called once per frame
    void Update() {

        // TAP BUNNY //

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //transform the touch position into world space from screen space and store it.
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

            //raycast with this information. If we have hit something we can process it.
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            if (hitInformation.collider != null)
            {
                //We should have hit something with a 2D Physics collider!
                //touchedBunny should be the object someone touched.
                if (hitInformation.transform.gameObject.tag == "Bunny")
                {
                    touchedBunny = hitInformation.transform.gameObject;
                    bunnyIsTapped = true;
                    offset = touchPosWorld - touchedBunny.transform.position;
                    //Debug.Log("Touched " + touchedBunny.name);

                    // Activate the haptics mesh
                    //Change this code later, not a good idea to call the child by index
                    GameObject haptic = touchedBunny.transform.GetChild(0).gameObject;
                    haptic.SetActive(true);

                }
            }
        }

        // MOVE BUNNY //

        if (Input.touchCount > 0 && bunnyIsTapped && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector3 newBunnyPosition = touchPosWorld - offset;
            touchedBunny.transform.position = new Vector3(newBunnyPosition.x, newBunnyPosition.y, newBunnyPosition.z);
            //Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            //float speed = 0.1f;

            // Move object across XY plane
            //touchedBunny.transform.Translate(touchDeltaPosition.x * speed, touchDeltaPosition.y * speed, 0);

        }

        // END MOVE BUNNY & DETECT IF SAT ON A SEAT//
        if (Input.touchCount > 0 && bunnyIsTapped && Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            // De-activate the haptics mesh
            //Change this code later, not a good idea to call the child by index
            GameObject haptic = touchedBunny.transform.GetChild(0).gameObject;
            haptic.SetActive(false);


            bunnyIsTapped = false; //end the touch

            //if (GlobalVariables.collidedSeat != null)
            touchedBunny.GetComponent<BunnyPlayer>().touchUp();
        }
    }
}
