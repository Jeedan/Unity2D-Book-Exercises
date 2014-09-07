using UnityEngine;
using System.Collections;

public class Delegates
{
    // define delegate
    delegate void RobotAction();

    RobotAction myRobotAction;

    void Start()
    {
        // default method for the delegate
        myRobotAction = RobotWalk;
    }

    void Update()
    {
        myRobotAction();
    }

    public void DoRobotWalk()
    {
        myRobotAction = RobotWalk;
    }

    void RobotWalk()
    {
        Debug.Log("Robot Walking");
    }

    public void DoRobotRun()
    {
        myRobotAction = RobotRun;
    }

    void RobotRun()
    {
        Debug.Log("Robot running");
    }
}
