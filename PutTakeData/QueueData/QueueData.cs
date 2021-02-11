using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using PutTakeData.Interfaces;

public class QueueData<T> : IEnumerable<T>, IQueueData<T>
{
    public static T[] _arrayData;
    public int _version;
    private const int defaultSize = 5;
    public int _index;
    static readonly object _object = new object();

    public QueueData()
    {
        _arrayData = new T[defaultSize];
        _index = 0;
        _version = 0;
    }

    public QueueData(int size)
    {
        if (size < 0)
            throw new ArgumentOutOfRangeException(nameof(size), "Size must be a positive number");

        _arrayData = new T[size];
        _index = 0;
        _version = 0;
    }

    public void Enqueue(T obj)
    {
        Monitor.Enter(_object);

        try
        {
            if (_index == _arrayData.Length)
            {
                T[] newArray = new T[2 * _arrayData.Length];
                Array.Copy(_arrayData, 0, newArray, 0, _index);
                _arrayData = newArray;
            }

            _arrayData[_index] = obj;
            _version++;
            _index++;
        }
        finally
        {
            Monitor.Exit(_object);
        }
    }

    public T[] Dequeue()
    {
        Monitor.Enter(_object);

        try
        {
            T[] newArray = new T[_arrayData.Length];

            if (_arrayData[0] != null)
            {
                for (int i = 0; i < _arrayData.Length - 1; i++)
                {
                    _arrayData[i] = _arrayData[i + 1];
                }

                Array.Copy(_arrayData, 0, newArray, 0, _index - 1);
                _arrayData = newArray;
            }

            return _arrayData;
        }
        finally
        {
            Monitor.Exit(_object);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new Enumerator<T>(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new Enumerator<T>(this);
    }
}

public class Enumerator<T> : IEnumerator<T>
{
    private QueueData<T> _queue;
    private int _index;
    private int _version;

    internal Enumerator(QueueData<T> queue)
    {
        _queue = queue;
        _index = queue._index;
        _version = queue._version;
    }

    public T Current
    {
        get
        {
            if (_index < 0)
                throw new InvalidOperationException("Enumerator Ended");

            return QueueData<T>._arrayData[_index];
        }
    }

    object IEnumerator.Current
    {
        get
        {
            if (_index < 0)
                throw new InvalidOperationException("Enumerator Ended");

            return QueueData<T>._arrayData[_index];
        }
    }

    public void Dispose()
    {
        _index = -1;
    }

    public bool MoveNext()
    {
        if (_version != _queue._version)
            throw new InvalidOperationException("Collection modified");

        _index = _index - 1;

        if (_index < 0)
        {
            return false;
        }

        return true;
    }

    public void Reset()
    {
        if (_version != _queue._version)
            throw new InvalidOperationException("Collection modified");

        _index = _queue._index;
    }
}
