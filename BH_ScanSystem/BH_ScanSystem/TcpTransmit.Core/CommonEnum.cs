namespace TcpTransmit.Core {
    /// <summary>
    /// Defines the host type.
    /// </summary>
    public enum HostType {
        Server,
        Client
    }

    public enum CallbackThreadType {
        WorkerThread,
        IoThread
    }
}