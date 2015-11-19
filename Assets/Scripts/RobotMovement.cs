using UnityEngine;
using System.Collections;
using System;

public class RobotMovement : Singleton<RobotMovement>
{
    public enum Leg
    {
        Left,
        Right,
    }

    public enum LegPosition
    {
        Down,
        Up,
        Forward
    }

    public LegPosition rightLegPos;
    public LegPosition leftLegPos;

    public float stepSize;
    public float stepTime;

    bool fell;

    public float rotationSpeed;

    Animator animator;

	void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("LeftLegDown", true);
        animator.SetBool("LegRightDown", true);
    }

    public void MoveLeg(string side, string direction)
    {
        Leg leg = Leg.Left;

        if (side == "right")
        {
            leg = Leg.Right;
        }

        switch (direction)
        {
            case ("up"):
            {
                LegUp(leg);
                break;
            }
            case ("down"):
            {
                LegDown(leg);
                break;
            }
            case ("forward"):
            {
                LegForward(leg);
                break;
            }
        }
    }

    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * stepSize);
    }

    public void Rotate(int degrees)
    {

        var rotation = new Vector3(0, transform.eulerAngles.y + degrees, 0);
        Debug.Log("Want to rotate to y: " + degrees);

        StartCoroutine(RotateLerp(rotation, Math.Abs(degrees)));
    }

    IEnumerator RotateLerp(Vector3 eulers, int rotateSize)
    {
        var original = transform.eulerAngles;
        float step = 0;

        animator.SetInteger("Rotating", rotateSize);

        while (step < 1)
        {
            step += Time.deltaTime * rotationSpeed / rotateSize;
            transform.eulerAngles = Vector3.Lerp(original, eulers, step);

            yield return null;
        }

        animator.SetInteger("Rotating", 0);
    }

    IEnumerator MoveForward()
    {
        var time = Time.time;

        var original = transform.position;
        var target = transform.position + Vector3.forward * stepSize;

        while (Time.time < time + stepTime)
        {
            var step = (Time.time - time) / stepTime;
            Debug.Log(step);
            transform.position = Vector3.Lerp(original, target, step);
            yield return null;
        }
    }

    public void StandUp()
    {
        if (fell)
        {
            fell = false;
            animator.SetBool("FallDown", false);

            leftLegPos = LegPosition.Down;
            rightLegPos = LegPosition.Down;

            animator.SetBool("LeftLegDown", true);
            animator.SetBool("LegRightDown", true);

            animator.SetBool("LeftLegForward", false);
            animator.SetBool("LegRightForward", false);

            animator.SetBool("LeftLegUp", false);
            animator.SetBool("LegRightUp", false);
        }
    }

    void FallDown()
    {
        fell = true;
        animator.SetBool("FallDown", true);
    }

    void LegDown(Leg leg)
    {
        if (leg == Leg.Left) // Left Leg
        {
            if (leftLegPos == LegPosition.Up)
            {
                animator.SetBool("LeftLegUp", false);
                leftLegPos = LegPosition.Down;
            }
            else if (leftLegPos == LegPosition.Forward)
            {
                animator.SetBool("LeftLegDown", true);
                animator.SetBool("LeftLegUp", false);
                animator.SetBool("LeftLegForward", false);

                // Step Forward Left
                StartCoroutine(MoveForward());

                leftLegPos = LegPosition.Down;
            }
        }
        else // Right Leg
        {
            if (rightLegPos == LegPosition.Up)
            {
                animator.SetBool("LegRightUp", false);
                rightLegPos = LegPosition.Down;
            }
            else if (rightLegPos == LegPosition.Forward)
            {
                animator.SetBool("LegRightDown", true);
                animator.SetBool("LegRightUp", false);
                animator.SetBool("LegRightForward", false);

                // Step Forward Right
                StartCoroutine(MoveForward());

                rightLegPos = LegPosition.Down;
            }
        }
    }

    void LegForward(Leg leg)
    {
        if (leg == Leg.Left)
        {
            if (leftLegPos == LegPosition.Up)
            {
                animator.SetBool("LeftLegForward", true);
                leftLegPos = LegPosition.Forward;
            }
        }
        else
        {
            if (rightLegPos == LegPosition.Up)
            {
                animator.SetBool("LegRightForward", true);
                rightLegPos = LegPosition.Forward;
            }
        }
    }

    void LegUp(Leg leg)
    {
        if (leg == Leg.Left)
        {
            if (rightLegPos == LegPosition.Up || rightLegPos == LegPosition.Forward)
            {
                FallDown();
            }
            else if (leftLegPos == LegPosition.Down)
            {
                animator.SetBool("LeftLegUp", true);
                animator.SetBool("LeftLegDown", false);
                leftLegPos = LegPosition.Up;
            }
            else if (leftLegPos == LegPosition.Forward)
            {
                animator.SetBool("LeftLegForward", false);
                leftLegPos = LegPosition.Up;
            }
        }
        else
        {
            if (leftLegPos == LegPosition.Up || leftLegPos == LegPosition.Forward)
            {
                FallDown();
            }
            else if (rightLegPos == LegPosition.Down)
            {
                animator.SetBool("LegRightUp", true);
                animator.SetBool("LegRightDown", false);
                rightLegPos = LegPosition.Up;
            }
            else if (rightLegPos == LegPosition.Forward)
            {
                animator.SetBool("LegRightForward", false);
                rightLegPos = LegPosition.Up;
            }
        }
    }
}
