using System.Collections.Generic;

namespace PutTakeData.Interfaces
{
    public interface IActionsQueue
    {
        void Enqueue(string str);
        IEnumerable<string> Dequeue();
    }
}
