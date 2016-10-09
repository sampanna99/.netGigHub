using NUnit.Framework;
using System;
using System.Transactions;

namespace GigHub.IntegrationTest
{
    public class Isolated : Attribute, ITestAction
    {
        private TransactionScope _transactionScope;

        public void BeforeTest(TestDetails testDetails)
        {
            //throw new NotImplementedException();
            _transactionScope = new TransactionScope();
        }

        public void AfterTest(TestDetails testDetails)
        {
            //throw new NotImplementedException();
            _transactionScope.Dispose();
        }

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
        }




    }
}
