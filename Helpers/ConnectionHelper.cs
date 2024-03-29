﻿using Npgsql;


namespace ContactPro.Helpers
{
    public static class ConnectionHelper
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("pgSettings")["pgConnection"];
            //Heroku Environment Values
            string? databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return String.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);

        }

        // build a connection string from the environment - Heroku
        private static string BuildConnectionString(string databaseUrl)
        {
            Uri databaseUri = new Uri(databaseUrl);
            string[] userInfo = databaseUri.UserInfo.Split(':');
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();

        }
 

    }
}
