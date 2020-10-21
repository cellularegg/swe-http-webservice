using System;
using System.Collections.Generic;
using System.Text;
// Hopefully I am allowed to use Newtensoft.Json
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HTTPServerLib
{
    public class MessageCollection
    {
        // Int is ID, string is Message Content
        private Dictionary<int, string> _Messages;
        public int MaxIdx { get; private set; }
        public int Count { get { return _Messages.Count; } }
        public MessageCollection()
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
                id = myJobject.SelectToken("Id").Value<int>();
                content = myJobject.SelectToken("Content").Value<string>();
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
    }
}
