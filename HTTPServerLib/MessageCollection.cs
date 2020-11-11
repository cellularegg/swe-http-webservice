using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace HTTPServerLib
{
    public class MessageCollection
    {
        // TODO Make threadsafe
        private static MessageCollection _instance = new MessageCollection();
        public static MessageCollection GetMessageCollection()
        {
            return _instance;
        }
        // Int is ID, string is Message Content
        private Dictionary<int, string> _Messages;
        public int MaxIdx { get; private set; }
        public int Count { get { return _Messages.Count; } }
        private MessageCollection()
        {
            MaxIdx = 0;
            _Messages = new Dictionary<int, string>();
        }

        // For Testing with singleton class
        public void Reset()
        {
            MaxIdx = 0;
            _Messages = new Dictionary<int, string>();
        }


        public Tuple<int, string> GetMsgTupleFromJson(string jsonMsg)
        {
            int id;
            string content;
            try
            {
                JObject myJobject = JObject.Parse(jsonMsg);
                // id = myJobject.SelectToken("Id").Value<int>(); -- Case Insensitive!!
                id = myJobject.GetValue("Id", StringComparison.OrdinalIgnoreCase).Value<int>();
                //content = myJobject.SelectToken("Content").Value<string>(); -- Case Insensitive!!
                content = myJobject.GetValue("Content", StringComparison.OrdinalIgnoreCase).Value<string>();
                return Tuple.Create(id, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return null;
            }
        }

        

        public bool UpdateMessage(int id, string content)
        {
            if (_Messages.ContainsKey(id))
            {
                _Messages[id] = content;
                return true;
            }
            return false;
        }
        public bool DeleteMessage(int id)
        {
            if (_Messages.ContainsKey(id))
            {
                _Messages.Remove(id);
                return true;
            }
            return false;
        }

        public void AddMessage(string content)
        {
            _Messages.Add(MaxIdx, content);
            MaxIdx++;
        }

        public string GetMessageContent(int id)
        {
            if (_Messages.ContainsKey(id))
            {
                return _Messages[id];
            }
            return string.Empty;
        }
        public string GetMessageAsJson(int id)
        {
            if (MaxIdx < id)
            {
                return "";
            }
            if (_Messages.ContainsKey(id))
            {
                //Serialize
                var jsonObject = new JObject();
                jsonObject.Add("Id", id);
                jsonObject.Add("Content", _Messages[id]);
                return jsonObject.ToString();
            }
            return "";
        }

        public string GetMessagesArrayAsJson()
        {
            if(this.Count == 0)
            {
                return "";
            }
            JArray result = new JArray(from m in _Messages select new JObject(new JProperty("Id", m.Key), new JProperty("Content", m.Value)));
            return result.ToString();
        }

        public string GetMsgContentFromJson(string jsonMsg)
        {
            string content;
            try
            {
                JObject myJobject = JObject.Parse(jsonMsg);
                //content = myJobject.SelectToken("Content").Value<string>(); -- Case Insensitive!!
                content = myJobject.GetValue("Content", StringComparison.OrdinalIgnoreCase).Value<string>();
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
