using PutTakeData.Interfaces;
using System.Collections.Generic;

namespace PutTakeData.ActionsQueue
{
    public class ActionsQueue : IActionsQueue
    {
        private IQueueData<string> _queue;

        public ActionsQueue(IQueueData<string> queue)
        {
            _queue = queue;
        }
        
        public void Enqueue(string str)
        {
            _queue.Enqueue(str);
        }

        public IEnumerable<string> Dequeue()
        {
            return _queue.Dequeue();
        }
    }
}
