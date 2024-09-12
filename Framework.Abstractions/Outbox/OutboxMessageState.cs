namespace Framework.Abstractions.Outbox;

public enum OutboxMessageState
{
    ReadyToSend = 1,
    SendToQueue = 2,
    Completed = 3
}