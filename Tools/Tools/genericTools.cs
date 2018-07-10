using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace FCA.Tools
{
    public class PostResult
    {
        public string Uri { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public TimeSpan ResponseTime { get; set; }
    }
    public static class genericTools
    {
        //public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
        //{
        //    foreach (var item in collection)
        //    {
        //        action(item);
        //    }
        //}
        public static void ForEach(this TreeNodeCollection collection, Action<TreeNode> action)
        {
            foreach (TreeNode item in collection)
            {
                action(item);
            }
        }
        public static string Serialize<T>(T toSerialize, XmlSerializerNamespaces namespaces = null)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (var memStm = new MemoryStream())
            using (var xw = XmlWriter.Create(memStm))
            {
                if (namespaces != null)
                    xmlSerializer.Serialize(xw, toSerialize, namespaces);
                else
                    xmlSerializer.Serialize(xw, toSerialize);
                string xml = System.Text.Encoding.UTF8.GetString(memStm.ToArray());
                return xml.Replace(@"﻿<?xml version=""1.0"" encoding=""utf-8""?>", "");
            }
        }
        public static T Deserialize<T>(string xmlString)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                return (T)serializer.Deserialize(memStream);
            }
        }
        public static async Task<string> SendPostXml<T>(string requestUri, T body)
        {
            try
            {
                var serializedRQ = Serialize(body);
                var content = new StringContent(serializedRQ, Encoding.UTF8, "text/xml");
                HttpClient client = new HttpClient();
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //client.DefaultRequestHeaders.Add("SOAPAction", "http://services.agresso.com/CustomerService/CustomerV201307/UpdateCustomer");

                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, requestUri);
                message.Content = content;

                HttpResponseMessage httpResponseMessage = await client.SendAsync(message);
                HttpContent httpContent = httpResponseMessage.Content;

                string responseString = await httpContent.ReadAsStringAsync();

                return responseString;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static async Task<PostResult> SendPostJson<T>(string requestUri, T body)
        {
            var result = new PostResult() { Uri = requestUri };
            try
            {
                result.Request = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                var content = new StringContent(result.Request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                client.Timeout = new TimeSpan(0, 5, 0);
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //client.DefaultRequestHeaders.Add("SOAPAction", "http://services.agresso.com/CustomerService/CustomerV201307/UpdateCustomer");

                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, requestUri);
                message.Content = content;

                var stpw = new Stopwatch();
                HttpResponseMessage httpResponseMessage = await client.SendAsync(message);
                HttpContent httpContent = httpResponseMessage.Content;
                result.Response = await httpContent.ReadAsStringAsync();
                stpw.Stop();
                result.ResponseTime = stpw.Elapsed;
            }
            catch (Exception ex)
            {
                result.Response = ex.ToString();
            }
            return result;
        }
        public static async Task<PostResult> SendPost(string requestUri, string bodyRequest)
        {
            var result = new PostResult() { Uri = requestUri };
            try
            {
                result.Request = bodyRequest;
                var content = new StringContent(result.Request, Encoding.UTF8, "text/xml");
                HttpClient client = new HttpClient();
                client.Timeout = new TimeSpan(0, 5, 0);
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //client.DefaultRequestHeaders.Add("SOAPAction", "http://services.agresso.com/CustomerService/CustomerV201307/UpdateCustomer");

                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, requestUri);
                message.Content = content;

                var stpw = new Stopwatch();
                stpw.Start();
                HttpResponseMessage httpResponseMessage = await client.SendAsync(message);
                HttpContent httpContent = httpResponseMessage.Content;
                result.Response = await httpContent.ReadAsStringAsync();
                stpw.Stop();
                result.ResponseTime = stpw.Elapsed;
            }
            catch (Exception ex)
            {
                result.Response = ex.ToString();
            }
            return result;
        }
        public static Task<PingReply> PingForHostName(string ipaddress)
        {
            long Google = -1;
            var ping = new Ping();
            ping.PingCompleted += (s, a) => Google = a.Reply.RoundtripTime;
            return ping.SendPingAsync("google.com");
        }
        public static string GetMachineNameFromIPAddress(string ipAdress)
        {
            string machineName = string.Empty;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAdress);

                machineName = hostEntry.HostName;
            }
            catch (Exception ex)
            {
                // Machine not found...
            }
            return machineName;
        }

        private const string INDENT_STRING = "    ";

        public static string FormatJson(string json)
        {

            int indentation = 0;
            int quoteCount = 0;
            var result =
                from ch in json
                let quotes = ch == '"' ? quoteCount++ : quoteCount
                let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, indentation)) : null
                let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, ++indentation)) : ch.ToString()
                let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, --indentation)) + ch : ch.ToString()
                select lineBreak == null
                            ? openChar.Length > 1
                                ? openChar
                                : closeChar
                            : lineBreak;

            return String.Concat(result);
        }
        public static String FormatXML(String XML)
        {
            String Result = XML;

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(XML);

                writer.Formatting = System.Xml.Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                String FormattedXML = sReader.ReadToEnd();

                Result = FormattedXML;
            }
            catch (XmlException)
            {
            }

            mStream.Close();
            writer.Close();

            return Result;
        }
        public static string[] GetBetween(string text, string from, string to)
        {
            var splitted = text.Split(new[] { from }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();
            for (int i = 1; i < splitted.Length; i++)
            {
                var split2 = splitted[i].Split(new[] { to }, StringSplitOptions.RemoveEmptyEntries);
                result.Add(split2[0]);
            }
            return result.ToArray();
        }
        public enum filterOperation
        {
            Equals = 1,
            Like = 2,
            NotEquals = 3,
            NotLike = 4
        }
        public enum filterAndOrOperation
        {
            And = 1, Or = 2
        }
        public static string GetFilterString(string filterValue, string[] columnNames, filterOperation filterOperation, filterAndOrOperation filterAndOrOperation)
        {
            if (string.IsNullOrEmpty(filterValue))
                return "";

            filterValue = filterValue.Replace("'", "''");

            var sb = new List<string>();
            columnNames.All(x =>
            {
                switch (filterOperation)
                {
                    case filterOperation.Equals:
                        sb.Add($"Convert(ISNULL([{x}],''), 'System.String') = '{filterValue}'");
                        break;
                    case filterOperation.Like:
                        sb.Add($"Convert(ISNULL([{x}],''), 'System.String') LIKE '%{filterValue}%'");
                        break;
                    case filterOperation.NotEquals:
                        sb.Add($"Convert(ISNULL([{x}],''), 'System.String') <> '{filterValue}'");
                        break;
                    case filterOperation.NotLike:
                        sb.Add($"Convert(ISNULL([{x}],''), 'System.String') NOT LIKE '%{filterValue}%'");
                        break;
                    default:
                        break;
                }
                return true;
            });
            switch (filterAndOrOperation)
            {
                case filterAndOrOperation.And:
                    return string.Join(" AND ", sb.ToArray());
                case filterAndOrOperation.Or:
                    return string.Join(" OR ", sb.ToArray());
            }
            return string.Join(" OR ", sb.ToArray());
        }

        public static bool IsDesignMode()
        {
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            return designMode;
        }

        public static void OpenNotepad(string value, string fileExtension = ".xml")
        {
            var file = Path.GetTempFileName() + fileExtension;
            File.WriteAllText(file, value);
            try
            {
                Process.Start("notepad++", file);
            }
            catch (Exception)
            {
                Process.Start("notepad", file);
            }
        }
        public static IEnumerable<T> Flatten<T>(this T rootItem, Func<T, IEnumerable<T>> getChildren)
        {
            return getChildren(rootItem).Flatten(getChildren);
        }
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> getChildren)
        {
            var stack = new Stack<T>();
            foreach (var item in items)
                stack.Push(item);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                var children = getChildren(current);
                if (children == null) continue;

                foreach (var child in children)
                    stack.Push(child);
            }
        }
    }
}
