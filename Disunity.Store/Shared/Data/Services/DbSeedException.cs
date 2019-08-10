using System;


namespace Disunity.Store.Data.Services {

    public class DbSeedException : Exception {
        public DbSeedException(string message): base(message) {}

    }

}