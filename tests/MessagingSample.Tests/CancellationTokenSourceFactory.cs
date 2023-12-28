using System.Diagnostics;

namespace MessagingSample.Tests;

public static class CancellationTokenSourceFactory
{
    public static CancellationTokenSource Create(int timeout) =>
        new CancellationTokenSource(Debugger.IsAttached ? TimeSpan.FromMinutes(timeout) : TimeSpan.FromSeconds(timeout));
}