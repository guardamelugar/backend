using System;
using System.Data;

namespace GuardameLugar.DataAccess.Extensions
{
    public static class DbCommandParameters
    {
        public static void AddParameter(this IDbCommand oCommand, string name, DbType type, object value)
        {
            IDbDataParameter oParameter = oCommand.CreateParameter();
            oParameter.ParameterName = name;
            oParameter.DbType = type;

            if (value == null)
                oParameter.Value = DBNull.Value;
            else
            {
                switch (type)
                {
                    case DbType.String:
                        oParameter.Value = (string)value;
                        break;
                    case DbType.DateTime:
                        oParameter.Value = (DateTime)value;
                        break;
                    case DbType.Int32:
                        oParameter.Value = (int)value;
                        break;
                    case DbType.Int64:
                        oParameter.Value = (long)value;
                        break;
                    case DbType.Decimal:
                        oParameter.Value = (decimal)value;
                        break;
                    case DbType.Boolean:
                        oParameter.Value = (bool)value;
                        break;
                }
            }
            oCommand.Parameters.Add(oParameter);
        }
    }
}
