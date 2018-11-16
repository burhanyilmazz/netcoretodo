using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Core.Entities
{
    public class ServiceEndpoint
    {
        public string HostName { get; set; }
        public AccountEndpoint AccountEndpoint { get; set; }
        public TaskEndpoint TaskEndpoint { get; set; }

    }

    public class TaskEndpoint
    {
        public string GetTaskViewModel { get; set; }
        public string GetPriorities { get; set; }
        public string AddTask { get; set; }
        public string UpdateTask { get; set; }
    }

    public class AccountEndpoint
    {
        public string RefreshToken { get; set; }

        public string SignInWithForm { get; set; }

        public string SignedUserDataEndpoint { get; set; }
    }


}
