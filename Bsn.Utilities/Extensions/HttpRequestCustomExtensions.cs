using Bsn.Utilities.Constants;
using Core.Utilities.Ensures;
using Core.Utilities.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Model.Dto.Table;

namespace Bsn.Utilities.Extensions
{
    public static class HttpRequestCustomExtensions
    {
        public static TableParams GetTableParams(this HttpRequestData httpRequest)
        {
            Ensure.That(httpRequest, nameof(httpRequest)).IsNotNull();
            string? orderBy = httpRequest.GetValue<string>(TableParam.orderBy);
            int skipValue = httpRequest.GetValue<int>(TableParam.skip);
            int takeValue = httpRequest.GetValue<int>(TableParam.take);
            takeValue = takeValue == 0 ? 10 : takeValue;
            bool isAscValue = httpRequest.GetValue<bool>(TableParam.isAsc);
            int pag = httpRequest.GetValue<int>(TableParam.pag);
            TableParams dataTableParams = new()
            {
                OrderBy = string.IsNullOrWhiteSpace(orderBy) ? string.Empty : orderBy,
                Take = takeValue,
                Skip = skipValue,
                IsAsc = isAscValue,
                Pag = pag
            };
            return dataTableParams;

        }
    }
}
