using Google.Protobuf;
using Grpc.Core;

namespace StatisticsService.API.Helpers;
public static class GrpcStreamHelper
{
    public static async Task StreamListAsync<TItem, TResponse>(
        IEnumerable<TItem> items,
        IServerStreamWriter<TResponse> responseStream,
        Func<IEnumerable<TItem>, TResponse> responseFactory,
        int chunkSizeBytes,
        CancellationToken cancellationToken)
        where TItem : IMessage // WAŻNE: protobuf typ
        where TResponse : class
    {
        var buffer = new List<TItem>();
        int currentSize = 0;

        foreach (var item in items)
        {
            int estimatedSize = item.ToByteArray().Length;

            if (currentSize + estimatedSize > chunkSizeBytes && buffer.Count > 0)
            {
                var response = responseFactory(buffer);
                await responseStream.WriteAsync(response);
                buffer.Clear();
                currentSize = 0;
            }

            buffer.Add(item);
            currentSize += estimatedSize;
            System.Console.WriteLine($"Currentsize: {currentSize} )) esitmatedSize: {estimatedSize}");
            if (cancellationToken.IsCancellationRequested)
                break;
        }

        if (buffer.Count > 0)
        {
            var response = responseFactory(buffer);
            await responseStream.WriteAsync(response);
        }
    }
}
