using Google.Protobuf;
using Grpc.Core;

namespace SimulationService.API.Helpers;
public static class GrpcStreamHelper
{
    /// <summary>
    /// Streamuje kolekcję elementów w porcjach (chunkach), aby uniknąć przekroczenia limitu wielkości wiadomości gRPC.
    /// </summary>
    /// <typeparam name="TItem">Element type.</typeparam>
    /// <typeparam name="TResponse">Type of message to send (np. ScoreboardsResponse).</typeparam>
    /// <param name="items">Collection to send</param>
    /// <param name="responseStream">Response grpc stream</param>
    /// <param name="responseFactory">Fuction to create a message</param>
    public static async Task StreamListAsync<TItem, TResponse>(
        IEnumerable<TItem> items,
        IServerStreamWriter<TResponse> responseStream,
        Func<IEnumerable<TItem>, TResponse> responseFactory,
        int chunkSizeBytes,
        CancellationToken cancellationToken)
        where TItem : IMessage
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

            if (cancellationToken.IsCancellationRequested)
                break;
            System.Console.WriteLine(DateTime.Now);
        }

        if (buffer.Count > 0)
        {
            var response = responseFactory(buffer);
            await responseStream.WriteAsync(response);
        }
    }
}
