using FDScanSystem.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FDScanSystem.Listen.Logic
{
    public class CommandQueueManager
    {
        private readonly Dictionary<string, CommandQueue> _dict;
        private object _lock = new object();

        public CommandQueueManager()
        {
            _dict = new Dictionary<string, CommandQueue>(10);
        }

        public void Add(string connectionKey, CommandBase cmd)
        {
            lock (_lock)
            {
                CommandQueue queue = null;
                bool exists = false;
                if (!(exists = _dict.TryGetValue(connectionKey, out queue)))
                {
                    queue = new CommandQueue();
                    queue.CmdQueue = new Queue<CommandBase>(3);
                }

                queue.CmdQueue.Enqueue(cmd);

                if (exists)
                {
                    _dict[connectionKey] = queue;
                }
                else
                {
                    _dict.Add(connectionKey, queue);
                }
            }
        }

        public CommandBase DequeueCommand(string connectionKey)
        {
            lock (_lock)
            {
                CommandQueue queue = null;
                if (_dict.TryGetValue(connectionKey, out queue))
                {
                    if (queue.CurrentProcess == null && queue.CmdQueue != null && queue.CmdQueue.Count > 0)
                    {
                        queue.CurrentProcess = queue.CmdQueue.Dequeue();
                        return queue.CurrentProcess;
                    }
                    else if (queue.CurrentProcess != null)
                    {
                        return queue.CurrentProcess;
                    }
                }
                return null;
            }
        }

        public CommandBase GetCurrent(string connectionKey)
        {
            lock (_lock)
            {
                CommandQueue queue = null;
                if (_dict.TryGetValue(connectionKey, out queue))
                {
                    return queue.CurrentProcess;
                }
                return null;

            }
        }

        public void CommandIsDone(string connectionKey)
        {
            lock (_lock)
            {
                CommandQueue queue = null;
                if (_dict.TryGetValue(connectionKey, out queue))
                {
                 //   queue.CurrentProcess.Sended();
                    queue.CurrentProcess = null;
                    _dict[connectionKey] = queue;
                    
                }
            }
        }


        public void Rollback(string connectionKey)
        {
            CommandQueue queue = null;
            var db = DataContextFactory.Create();
            if (_dict.TryGetValue(connectionKey, out queue))
            {
                var realQueue = queue.CmdQueue.ToList();
                foreach (var queueItem in realQueue)
                {
                    if (queueItem.ID > 0)
                    {
                        var commandEntit = db.FD_Command.FirstOrDefault(u => u.ID == queueItem.ID);
                        if (commandEntit != null)
                        {
                            commandEntit.Status = (int)CommandStatus.New;
                        }
                    }

                }
                if (queue.CurrentProcess != null && queue.CurrentProcess.ID > 0)
                {
                    var commandEntit = db.FD_Command.FirstOrDefault(u => u.ID == queue.CurrentProcess.ID);
                    if (commandEntit != null)
                    {
                        commandEntit.Status = (int)CommandStatus.New;
                    }
                }
                db.SubmitChanges();

                Remove(connectionKey);
            }
        }

        protected void Remove(string key)
        {
            lock (_lock)
            {
                _dict.Remove(key);
            }
        }


        public List<string> ListAllHash()
        {
            return _dict.Select(u => u.Key).ToList();
        }


        public CommandQueue this[string key]
        {

            get
            {
                CommandQueue queue = null;
                if (this._dict.TryGetValue(key, out queue))
                {
                    return queue;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
