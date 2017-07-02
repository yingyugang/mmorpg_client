using UnityEngine;
using System.Collections;

public class BodyMove : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        minY = transform.position.y;
        maxY = minY + 1920 * this.transform.localScale.y;
	}

    float moveSpeed = 5.0f;
    float minY;
    float maxY;

    bool down = false;
    float pcCurY = 0;
    float touchCurY = 0;

    float accSpeed = 5f;
    float beginy = 0;
    float begintime = 0;
    float curSpeed;
	
	// Update is called once per frame
	void Update () 
    {
        dealPcMove();
        dealTouch();
	}

    void dealPcMove()
    {
        if (down)
        {
            if (Input.GetMouseButton(0))//拖动
                move(Input.mousePosition.y - pcCurY);
            else //放开
            {
                down = false;
                pcCurY = 0f;
                curSpeed = (Input.mousePosition.y - beginy) / (Time.time - begintime);
                dealAcc();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))//按下
            {
                down = true;
                pcCurY = Input.mousePosition.y;
                begintime = Time.time;
                beginy = pcCurY;
            }
            else
                dealAcc();
        }
    }

    void dealTouch()
    {
        if(Input.touchCount!=1)
        {
            dealAcc();
            touchCurY = 0;
            return;
        }
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            touchCurY = touch.position.y;
            begintime = Time.time;
            beginy = touchCurY;
        }
        else if (touch.phase == TouchPhase.Moved)
            move(touch.position.y - touchCurY);
        else 
        { 
            touchCurY = 0;
            if(touch.phase == TouchPhase.Ended)
            {
                curSpeed = (touch.position.y - beginy) / (Time.time - begintime);
                dealAcc();
            }
        }
    }

    void dealAcc()
    {
        if (curSpeed == 0f)
            return;
        float newspeed=0;
        float moveValue = 0;

        if (curSpeed > 0)
        {
            newspeed = curSpeed - accSpeed * Time.deltaTime;
            if (newspeed < 0)
                newspeed = 0;
        }
        else
        {
            newspeed = curSpeed + accSpeed * Time.deltaTime;
            if (newspeed > 0)
                newspeed = 0;
        }
        moveValue = Time.deltaTime * (curSpeed + newspeed) / 2;
        move(moveValue);
        curSpeed = newspeed;
    }

    void move(float move)
    {
        if (move != 0.0f)
        {
            float offset = Time.deltaTime * moveSpeed * move * this.transform.localScale.y;
            transform.Translate(0, offset, 0);
            if (transform.position.y < minY)
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);
            else if(transform.position.y > maxY)
                transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
    }
}
