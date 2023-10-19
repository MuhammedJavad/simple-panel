using System.Text.RegularExpressions;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Persistence.Extensions;

public static class DbExtensions
{
    public static bool IsUniqueConstraintException(this DbUpdateException ex, out string columnName)
    {
        columnName = string.Empty;
        if (ex.InnerException is not MySqlException mySqlException) return false;
        if (mySqlException.Number != 1062) return false;
        
        // Unique constraint violation error occurred (MySQL error code 1062)
        var errorMessage = mySqlException.Message;

        // Use a regular expression to extract the column name from the error message
        var pattern = @"Duplicate entry '(.*?)' for key '(.*?)'";
        var match = Regex.Match(errorMessage, pattern);
        if (!match.Success) return false;
        
        // 'columnName' will contain the name of the column causing the error
        columnName = SanitizeIndexName(match.Groups[2].Value);
        return true;

        string SanitizeIndexName(string name)
        {
            var pointer = name.AsSpan();
            var index = pointer.LastIndexOf('_');
            return index > -1 
                ? pointer[(index + 1)..].ToString().Humanize() 
                : name.Humanize();
        }
    }
}