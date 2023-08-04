using System.Net;

namespace DataSelector.Common.Dtos;

public class RedditMockupResponseDto<T>
{
    public T? Data { get; set; }

    public bool IsSuccess { get; set; }

    public string? Message { get; set; }

    public HttpStatusCode HttpStatusCode { get; set; }
}

