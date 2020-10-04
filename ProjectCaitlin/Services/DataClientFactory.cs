using ProjectCaitlin.Config;
using ProjectCaitlin.Services.Rds;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Services
{
    class DataClientFactory
    {
        private static readonly RdsClient RdsClientInstance = new RdsClient(RdsConfig.BaseUrl);
        private static readonly IDataClient DefaultClient = RdsClientInstance;
        public static IDataClient GetDataClient(String type)
        {
            return type switch
            {
                "RDS" => RdsClientInstance,
                _ => DefaultClient,
            };
        }

        public static IDataClient GetDataClient()
        {
            return DefaultClient;
        }
    }
}
