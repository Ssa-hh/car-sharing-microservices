using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Common.Domain
{
    public class Result
    {
        public Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None)
                || (!isSuccess && error == Error.None)) { 
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get;}

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            if(isSuccess && value is null) throw new ArgumentNullException(nameof(value), "The value can't be null when the result succeeded");

            _value = value;
        }

        [NotNull]
        public TValue Value => IsSuccess? _value! : throw new InvalidOperationException("The value of invalid result can't be accessed.");

        public static implicit operator Result<TValue>(TValue value) => value is not null ? Success(value) : Failure< TValue>(Error.NullValue);
    }
}
