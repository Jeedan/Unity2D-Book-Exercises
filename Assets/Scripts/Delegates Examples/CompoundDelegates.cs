using UnityEngine;
using System.Collections;

public class CompoundDelegates
{
    public class WorkerManager
    {
        void DoWork()
        {
            DoJob1();
            DoJob2();
            DoJob3();
        }

        private void DoJob1() { }
        private void DoJob2() { }
        private void DoJob3() { }
    }

    // better way to chain functions together
    public class WorkerManager2
    {
        // workermanager delegate
        delegate void MyDelegateHook();
        MyDelegateHook ActionsToDo;

        public string WorkerType = "Peon";

        // on Startup assign jobs to the worker, this is configurable not fixed
        void Start()
        {
            if (WorkerType == "Peon")
            {
                ActionsToDo += DoJob1;
                ActionsToDo += DoJob2;
            }
            else
            {
                ActionsToDo += DoJob3;
            }
        }

        // do the actions
        void Update()
        {
            ActionsToDo();
        }

        private void DoJob1() { /* do some work*/}
        private void DoJob2() {/* do some more work*/ }
        private void DoJob3() { /* ahahhaha slack off*/ }
    }
}
