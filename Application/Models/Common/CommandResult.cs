using System.Net;

namespace Application.Models.Common;

public class CommandResult
{
    public HttpStatusCode StatusCode { get; set; }
    public object Data { get; set; }
}