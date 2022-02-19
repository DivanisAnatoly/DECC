using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace DECC.Infrastructure
{
    public class DBContext
    {
        public static NpgsqlConnection GetConnection()
        {
            return new(@"Server=localhost; Port=5432; Database=academy; User ID=postgres; password=Qazwsx123!;");
        }
    }
}
