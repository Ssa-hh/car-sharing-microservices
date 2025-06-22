using Microsoft.AspNetCore.Http;
using Ssa.CarSharing.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Presentation.Helpers;


public static class ApiResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound => Results.NotFound(new Error(result.Error.Description)),
            ErrorType.Conflict => Results.Conflict(new Error(result.Error.Description)),
            ErrorType.Forbidden => Results.Forbid(),
            _ => Results.BadRequest(new Error(result.Error.Description))
        };
    }

    private record Error(string Detail);
}
