namespace PutTakeData.Interfaces
{
    public interface IQueueData<T>
    {
        public void Enqueue(T obj);

        public T[] Dequeue();
    }
}
