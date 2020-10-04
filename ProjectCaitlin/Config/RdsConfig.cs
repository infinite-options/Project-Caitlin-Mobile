using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Config
{
    public class RdsConfig
    {
        public const string BaseUrl = "https://3s3sftsr90.execute-api.us-west-1.amazonaws.com/dev";
        public const string listAllOtherTA = "/api/v2/listAllTA";
        public const string aboutMeUrl = "/api/v2/aboutme";
        public const string goalsAndRoutinesUrl = "/api/v2/getgoalsandroutines";
        public const string UserIdFromEmailUrl = "/api/v2/userLogin";
        public const string actionAndTaskUrl = "/api/v2/actionsTasks";
    }
}
