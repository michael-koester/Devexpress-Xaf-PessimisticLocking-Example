using System;

namespace PessimisticLocker
{
    public interface IPessimisticLocking
    {
        Type GetType();
        string Id { get; }
    }
}
