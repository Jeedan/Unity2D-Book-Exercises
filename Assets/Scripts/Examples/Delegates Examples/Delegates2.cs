using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Delegates2
{
    public class Worker
    {
        List<string> WorkCompletedfor = new List<string>();
        public void DoSomething(string ManagerName, Action myDelegate)
        {
            // audits that work was done
            WorkCompletedfor.Add(ManagerName);

            // begin
            myDelegate();
        }
    }

    public class Manager
    {
        private Worker myWorker = new Worker();

        public void PieceofWork()
        {
            // some work
        }

        public void PieceOfWork2()
        {
            // more work
        }

        public void DoWork()
        {
            // send worker to do job 1
            myWorker.DoSomething("Manager1", PieceofWork);
            // send worker to do job 2
            myWorker.DoSomething("Manager2", PieceOfWork2);
        }

        public void DoWork2()
        {
            // or using lambdas
            myWorker.DoSomething("Manager1", () =>
            {
                // some piece of work
            });

            // send worker to do job 2
            myWorker.DoSomething("Manager2", () =>
            {
                // some piece of work
            });
        }
    }
}
