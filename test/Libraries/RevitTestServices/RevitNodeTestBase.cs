using DynamoUnits;

using NUnit.Framework;

using RevitServices.Persistence;
using RevitServices.Transactions;

using TestServices;

namespace RevitTestServices

{
    /// <summary>
    /// Base class for units tests of Revit nodes.
    /// </summary>
    public class RevitNodeTestBase : GeometricTestBase
    {
        public const string ANALYSIS_DISPLAY_TESTS = "AnalysisDisplayTests";

        public RevitNodeTestBase()
        {
            AssemblyResolver.Setup();
        }

        [SetUp]
        public override void Setup()
        {
            SetupTransactionManager();
            DisableElementBinder();
            base.Setup();
            SetUpHostUnits();
        }

        public void SetupTransactionManager()
        {
            // create the transaction manager object
            TransactionManager.SetupManager(new AutomaticTransactionStrategy());

            // Tests do not run from idle thread.
            TransactionManager.Instance.DoAssertInIdleThread = false;

            // Start a transaction
            TransactionManager.Instance.EnsureInTransaction(DocumentManager.Instance.CurrentDBDocument);
        }

        public void DisableElementBinder()
        {
            ElementBinder.IsEnabled = false;
        }

        [TearDown]
        public virtual void ShutDownTransactionManager()
        {
            // Automatic transaction strategy requires that we 
            // close the transaction if it hasn't been closed by 
            // by the end of an evaluation. It is possible to 
            // run the test framework without running Dynamo, so
            // we ensure that the transaction is closed here.
            TransactionManager.Instance.ForceCloseTransaction();
        }

        private static void SetUpHostUnits()
        {
            BaseUnit.HostApplicationInternalAreaUnit = AreaUnit.SquareFoot;
            BaseUnit.HostApplicationInternalLengthUnit = LengthUnit.DecimalFoot;
            BaseUnit.HostApplicationInternalVolumeUnit = VolumeUnit.CubicFoot;
        }
    }
}
