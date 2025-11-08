using System;

namespace EngineService.Application.Consts;

public static class GrpcConsts
{
    public const int MAX_RECEIVE_MESSAGE_SIZE = 20 * 1024 * 1024; // 20 MB
    public const int MAX_SEND_MESSAGE_SIZE = 20 * 1024 * 1024;    // 20 MB
}
