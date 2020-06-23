using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void HelloWorld()
    {
        Microsoft.SqlServer.Server.SqlMetaData columnInfo
                = new Microsoft.SqlServer.Server.SqlMetaData("Column1", SqlDbType.NVarChar, 12);
        SqlDataRecord greetingRecord
            = new SqlDataRecord(new Microsoft.SqlServer.Server.SqlMetaData[] { columnInfo });
        greetingRecord.SetString(0, "Hello world!");
        SqlContext.Pipe.Send(greetingRecord);
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PrintText()
    {
        SqlPipe sqlP = SqlContext.Pipe;
        sqlP.Send("This is a first stored procedure using CLR database object");
    }
}
