using Google.Protobuf.Reflection;

namespace SimPitch.ContractTests.Infrastructure;

/// <summary>
/// Schema level of the contract: every service keeps its own copy of the SimPitch protos, so the copy compiled
/// into the client must describe exactly the same wire format as the copy compiled into the server.
/// Compares RPC methods and every message reachable from them (field numbers, names, types, cardinality).
/// File names are ignored on purpose - services use different ProtoRoot settings, which does not affect the wire.
/// </summary>
public static class ProtoSchemaContract
{
    public static void AssertCompatible(ServiceDescriptor server, ServiceDescriptor client)
    {
        var differences = new List<string>();

        if (server.FullName != client.FullName)
            differences.Add($"service name: server '{server.FullName}' vs client '{client.FullName}'");

        var serverMethods = server.Methods.ToDictionary(m => m.Name);
        var clientMethods = client.Methods.ToDictionary(m => m.Name);

        foreach (var name in clientMethods.Keys.Except(serverMethods.Keys))
            differences.Add($"rpc {name}: called by the client but not implemented by the server");
        foreach (var name in serverMethods.Keys.Except(clientMethods.Keys))
            differences.Add($"rpc {name}: exists on the server but is missing in the client copy");

        var visited = new HashSet<string>();
        foreach (var (name, serverMethod) in serverMethods)
        {
            if (!clientMethods.TryGetValue(name, out var clientMethod))
                continue;

            if (serverMethod.IsClientStreaming != clientMethod.IsClientStreaming || serverMethod.IsServerStreaming != clientMethod.IsServerStreaming)
                differences.Add($"rpc {name}: streaming mode differs");

            CompareMessage(serverMethod.InputType, clientMethod.InputType, $"{name} request", differences, visited);
            CompareMessage(serverMethod.OutputType, clientMethod.OutputType, $"{name} response", differences, visited);
        }

        Assert.True(differences.Count == 0,
            $"Proto contract of {server.FullName} differs between client and server copies:{Environment.NewLine}- " +
            string.Join(Environment.NewLine + "- ", differences));
    }

    private static void CompareMessage(MessageDescriptor server, MessageDescriptor client, string path, List<string> differences, HashSet<string> visited)
    {
        if (server.FullName != client.FullName)
        {
            differences.Add($"{path}: message '{server.FullName}' (server) vs '{client.FullName}' (client)");
            return;
        }
        if (!visited.Add(server.FullName))
            return;

        var serverFields = server.Fields.InFieldNumberOrder().ToDictionary(f => f.FieldNumber);
        var clientFields = client.Fields.InFieldNumberOrder().ToDictionary(f => f.FieldNumber);

        foreach (var number in serverFields.Keys.Union(clientFields.Keys).OrderBy(n => n))
        {
            var fieldPath = $"{server.FullName} #{number}";
            if (!serverFields.TryGetValue(number, out var serverField))
            {
                differences.Add($"{fieldPath} '{clientFields[number].Name}': only in the client copy");
                continue;
            }
            if (!clientFields.TryGetValue(number, out var clientField))
            {
                differences.Add($"{fieldPath} '{serverField.Name}': only in the server copy");
                continue;
            }

            if (serverField.Name != clientField.Name)
                differences.Add($"{fieldPath}: name '{serverField.Name}' (server) vs '{clientField.Name}' (client)");
            if (serverField.FieldType != clientField.FieldType)
                differences.Add($"{fieldPath} '{serverField.Name}': type {serverField.FieldType} (server) vs {clientField.FieldType} (client)");
            if (serverField.IsRepeated != clientField.IsRepeated)
                differences.Add($"{fieldPath} '{serverField.Name}': repeated differs");
            if (serverField.HasPresence != clientField.HasPresence)
                differences.Add($"{fieldPath} '{serverField.Name}': optional (presence) differs");

            if (serverField.FieldType == FieldType.Message && clientField.FieldType == FieldType.Message)
                CompareMessage(serverField.MessageType, clientField.MessageType, fieldPath, differences, visited);
            if (serverField.FieldType == FieldType.Enum && clientField.FieldType == FieldType.Enum)
                CompareEnum(serverField.EnumType, clientField.EnumType, fieldPath, differences);
        }
    }

    private static void CompareEnum(EnumDescriptor server, EnumDescriptor client, string path, List<string> differences)
    {
        var serverValues = server.Values.Select(v => $"{v.Name}={v.Number}").ToHashSet();
        var clientValues = client.Values.Select(v => $"{v.Name}={v.Number}").ToHashSet();
        if (!serverValues.SetEquals(clientValues))
            differences.Add($"{path}: enum {server.FullName} values differ");
    }
}
