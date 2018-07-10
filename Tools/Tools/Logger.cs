using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SunHotels
{
    public class Logger
    {
        private static string ExecutionTime = DateTime.Now.ToString("yyyyMMddHHmm");

        private string logPath;
        public Logger(string directory, string file)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            logPath = Path.Combine(directory, "log_" + file + "_" + ExecutionTime + ".txt");
        }

        public static object lockObject = new object();

        public void Log(string message)
        {

            lock (lockObject)
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss:fff") + " >>>>> " + message);
                }
        }

        public void Log(string message, System.Exception exception)
        {
            lock (lockObject)
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss:fff") + " >>>>> " + message);
                    sw.WriteLine(" Error details: ");
                    sw.WriteLine(exception.ToString());
                }
        }
    }
    [Serializable]
    public class LogHierarchy
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Statistics { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LogPath { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AgentId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TotalTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Ch")]
        public Dictionary<string, LogHierarchyChild> Children { get; set; }
        public void AddStatistics(string msg)
        {
            if (Statistics == null) Statistics = new List<string>();
            if (!Statistics.Contains(msg)) Statistics.Add(msg);
        }
        public void Add(params string[] items)
        {
            if (items?.Length == 0) return;
            if (string.IsNullOrEmpty(items[0])) return;
            if (Children == null) Children = new Dictionary<string, LogHierarchyChild>();
            if (!Children.ContainsKey(items[0]))
            {
                Children.Add(items[0], new LogHierarchyChild());
            }
            Children[items[0]].Add(true, items.Skip(1).ToArray());
        }
        public LogHierarchy()
        {

        }
        public LogHierarchy(string description, string logPath, string agentId)
        {
            this.Description = description;
            this.LogPath = logPath;
            this.AgentId = agentId;
            this.StartTime = DateTime.Now;
        }
        public void SerializeToFile()
        {
            try
            {
                if (string.IsNullOrEmpty(LogPath)) return;

                if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);

                TotalTime = DateTime.Now.Subtract(this.StartTime).ToString();
                var json = JsonConvert.SerializeObject(this);
                var logPath = Path.Combine(LogPath, Description + "_" + AgentId + "_" + StartTime.ToString("yyyyMMddHHmm") + ".txt");
                System.IO.File.WriteAllText(logPath, json);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void Filter(string text)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var c = Children.Skip(i).First();
                if (!c.Key.Contains(text) && !c.Value.Filter(text))
                {
                    Children.Remove(c.Key);
                }
            }
        }
    }
    [Serializable]
    public class LogHierarchyChild
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Ch")]
        public Dictionary<string, LogHierarchyChild> Children { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "c")]
        public string SingleName { get; set; }
        public void Add(bool forceAddToChildren = false, params string[] items)
        {
            if (items?.Length == 0) return;
            if (string.IsNullOrEmpty(items[0])) return;
            if (!forceAddToChildren && items.Length == 1)
            {
                SingleName = items[0];
                return;
            }
            if (Children == null) Children = new Dictionary<string, LogHierarchyChild>();
            if (!Children.ContainsKey(items[0])) Children.Add(items[0], new LogHierarchyChild());

            Children[items[0]].Add(false, items.Skip(1).ToArray());
        }
        public bool Filter(string text)
        {
            if (SingleName != null && SingleName.Contains(text))
                return true;
            if (Children == null || Children.Count == 0)
                return false;
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var c = Children.Skip(i).First();
                if (!c.Key.Contains(text) && !c.Value.Filter(text))
                {
                    Children.Remove(c.Key);
                }
            }
            return Children.Count > 0;
        }
        public bool Contains(string text)
        {
            if (SingleName != null && SingleName.Contains(text))
                return true;
            if (Children == null || Children.Count == 0)
                return false;
            return Children.Any(c => c.Key.Contains(text) || c.Value.Contains(text));
        }
    }
}
