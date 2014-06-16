using System;
using System.Transactions;
using NUnit.Framework;

namespace Sample.Core.Test
{
    public class RollbackAttribute : Attribute, ITestAction
    {
        private TransactionScope tx;

        public void AfterTest(TestDetails testDetails)
        {
            if (tx != null)
            {
                try
                {
                    tx.Dispose();
                }
                catch (Exception) { }
            }
        }

        public void BeforeTest(TestDetails testDetails)
        {
            tx = new TransactionScope();
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }
    }
}
