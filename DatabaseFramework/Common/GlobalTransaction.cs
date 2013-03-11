using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BrainWhizzDatabaseFramework
{
    /// <summary>
    /// Represents a global transaction class as well as manages it. It maintains a database transation for each different databases
    /// we try to connect. By doing so DBProvider can use the same connection and transaction if they are being created in
    /// a Global Transaction scope. Sharing the transaction among different commands gives us lot of performance boost.
    /// </summary>
    public class GlobalTransaction : IDisposable
    {
        #region Private Fields

        // This dictionary contains connection string and the corresponding transaction
        private Dictionary<string, IDbTransaction> openTransactions = new Dictionary<string, IDbTransaction>();

        // In case of nested tranaction, we create a dummy global transaction and pass it the real one which it will use to
        // forward calls.
        private GlobalTransaction realGlobalTransaction = null;

        [ThreadStatic]
        private static GlobalTransaction currentGlobalTransaction;

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor to control object creation.
        /// </summary>
        private GlobalTransaction()
        {
        }

        /// <summary>
        /// Private constructor to create global transaction with the given parent transaction
        /// </summary>
        private GlobalTransaction(GlobalTransaction realGlobalTransaction)
        {
            this.realGlobalTransaction = realGlobalTransaction;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Represents currently active global transaction in the calling thread context, You should always use this property to interact
        /// with currently active Global Transaction
        /// </summary>
        internal static GlobalTransaction CurrentGlobalTransaction
        {
            get
            {
                return currentGlobalTransaction;
            }
            private set
            {
                currentGlobalTransaction = value;
            }
        }

        #endregion

        #region Internal Static Methods

        /// <summary>
        /// Checks to see if a global transaction is in progress. 
        /// </summary>
        /// <returns></returns>
        internal static bool IsGlobalTransactionInProgress()
        {
            return GlobalTransaction.CurrentGlobalTransaction != null;
        }

        /// <summary>
        /// Begins a global transaction. Creates a new GlobalTransaction if none is already started or returns
        /// a wrapper around existing one if one is already started. DO NOT CALL THIS METHOD DIRECTLY, USE DataAcessManager.StartGlobalTransaction
        /// API instead.
        /// </summary>
        internal static GlobalTransaction BeginGlobalTransaction()
        {
            GlobalTransaction transaction = null;
            if (GlobalTransaction.CurrentGlobalTransaction == null)
            {
                GlobalTransaction.CurrentGlobalTransaction = new GlobalTransaction();
                transaction = GlobalTransaction.CurrentGlobalTransaction;
            }
            else
            {
                // This will only happen when somebody starts a nested global transaction
                // returned global transaction is just a dummy wrapper around main global transaction
                transaction = new GlobalTransaction(GlobalTransaction.CurrentGlobalTransaction);
            }

            return transaction;
        }

        #endregion

        #region Internal Instance Methods

        /// <summary>
        /// Gets database  transaction associated with the given connection string. This is the actual database transaction that is to be
        /// shared for all communication with this database.
        /// </summary>
        internal IDbTransaction GetDbTransaction(string connectionString, IDBProvider dbProvider)
        {
            // if this in not the real transaction, forward the call to real transaction
            if (this.realGlobalTransaction == null)
            {
                if (!this.openTransactions.ContainsKey(connectionString))
                {
                    IDbConnection connection = dbProvider.CreateConnection(connectionString);
                    connection.Open();
                    this.openTransactions.Add(connectionString, connection.BeginTransaction());
                }
                return this.openTransactions[connectionString];
            }
            else
            {
                return this.realGlobalTransaction.GetDbTransaction(connectionString, dbProvider);
            }
        }

        #endregion

        #region IDisposable Members

        private bool isDisposed = false;

        /// <summary>
        /// Disposes this transaction. Commits the transaction if this is the parent transaction.
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                // if this is the real transaction that is being disposed, null the current transaction
                if (this.realGlobalTransaction == null)
                {
                    GlobalTransaction.CurrentGlobalTransaction = null;
                }

                // commit all transactions and close the connection
                foreach (IDbTransaction transaction in this.openTransactions.Values)
                {
                    using (IDbConnection connection = transaction.Connection)
                    {
                        using (transaction)
                        {
                            transaction.Commit();
                        }
                    }
                }
                this.openTransactions.Clear();

                isDisposed = true;
            }
        }

        #endregion
    }
}
