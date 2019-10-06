using klaviyo.net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SG2.CORE.MODAL.klaviyo
{
    public class KlaviyoAPI
    {

        public List<KlaviyoList> GetKlaviyoLists()
        {
            List<KlaviyoList> lists = null;
            string jsonResponse = string.Empty;

            string endpoint = string.Format("{0}?api_key={1}",
                "https://a.klaviyo.com/api/v2/lists", "pk_66fa816af1389fa030a03c9991430ed2c7");

            HttpWebRequest request = GetWebRequestObject(endpoint);

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            lists = JsonConvert.DeserializeObject<List<KlaviyoList>>(new StreamReader(stream).ReadToEnd());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lists;
        }

        public List<KlaviyoProfileRes> Klaviyo_AddtoList(KlaviyoProfile profile, string klaviyoUrl, string apiKey, string listId)
        {
            List<KlaviyoProfileRes> result = new List<KlaviyoProfileRes>();
            string jsonResponse = string.Empty;
            string endpoint = string.Format(
                "{0}/{1}/members",
                klaviyoUrl,
                listId);

            HttpWebResponse httpResponse;

            var hDTO = new KlaviyoListSubscription()
            {
                api_key = apiKey,
                profiles = new List<KlaviyoProfile>()
            };

            hDTO.profiles.Add(new KlaviyoProfile()
            {
                email = profile.email// "sraza@crovtech.com"
            });

            string json = JsonConvert.SerializeObject(
                hDTO,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });

            try
            {
                HttpWebRequest request = GetWebRequestObject(endpoint);
                request.ContentType = "application/json";
                request.Method = "POST";
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byte1 = encoding.GetBytes(json);
                request.ContentLength = byte1.Length;

                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                using (httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        string response1 = new StreamReader(stream).ReadToEnd().Replace("{}", string.Empty);
                        object myClass = Newtonsoft.Json.JsonConvert.DeserializeObject(response1);

                        result = JsonConvert.DeserializeObject<List<KlaviyoProfileRes>>(myClass.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return result;
        }

        public List<KlaviyoProfileRes> Klaviyo_AddtoSegment(KlaviyoProfile profile, string segmentId = "H6fnAh")
        {
            List<KlaviyoProfileRes> result;
            string jsonResponse = string.Empty;
            string endpoint = string.Format(
                "{0}/{1}/members",
                "https://a.klaviyo.com/api/v2/list",
                segmentId);


            HttpWebResponse httpResponse;

            var hDTO = new KlaviyoListSubscription()
            {
                api_key = "pk_66fa816af1389fa030a03c9991430ed2c7",
                profiles = new List<KlaviyoProfile>()
            };
            hDTO.profiles.Add(new KlaviyoProfile()
            {
                email = profile.email// "sraza@crovtech.com"
            });

            string json = JsonConvert.SerializeObject(
                hDTO,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });

            try
            {
                HttpWebRequest request = GetWebRequestObject(endpoint);
                request.ContentType = "application/json";
                request.Method = "POST";
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byte1 = encoding.GetBytes(json);
                request.ContentLength = byte1.Length;

                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                using (httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        string response1 = new StreamReader(stream).ReadToEnd().Replace("{}", string.Empty);
                        object myClass = Newtonsoft.Json.JsonConvert.DeserializeObject(response1);

                        result = JsonConvert.DeserializeObject<List<KlaviyoProfileRes>>(myClass.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public bool Klaviyo_DeleteFromList(string email, string klaviyoUrl, string apiKey, string listId)
        {
            bool Success = false;           
            string jsonResponse = string.Empty;
            string endpoint = string.Format(
                "{0}/{1}/members",
                klaviyoUrl,
                listId);

            HttpWebResponse httpResponse;
            var hDTO = new KlaviyoDeleteProfileList()
            {
                api_key = apiKey,
                emails = email
            };

            string json = JsonConvert.SerializeObject(
                hDTO,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });

            try
            {
                HttpWebRequest request = GetWebRequestObject(endpoint);
                request.ContentType = "application/json";
                request.Method = "DELETE";
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byte1 = encoding.GetBytes(json);
                request.ContentLength = byte1.Length;

                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                using (httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        string response1 = new StreamReader(stream).ReadToEnd().Replace("{}", string.Empty);
                        object myClass = Newtonsoft.Json.JsonConvert.DeserializeObject(response1);

                        Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Success;
        }
        
        public List<KlaviyoProfileRes> Klaviyo_SubscribetoLists()
        {
            List<KlaviyoProfileRes> result;
            string jsonResponse = string.Empty;
            string endpoint = string.Format(
                "{0}/{1}/subscribe",
                "https://a.klaviyo.com/api/v2/list",
                "H6fnAh");

            HttpWebResponse httpResponse;

            var hDTO = new KlaviyoListSubscription()
            {
                api_key = "pk_66fa816af1389fa030a03c9991430ed2c7",
                profiles = new List<KlaviyoProfile>()
            };
            hDTO.profiles.Add(new KlaviyoProfile()
            {
                email = "sraza@crovtech.com"
            });

            string json = JsonConvert.SerializeObject(
                hDTO,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });

            try
            {
                HttpWebRequest request = GetWebRequestObject(endpoint);
                request.ContentType = "application/json";
                request.Method = "POST";
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byte1 = encoding.GetBytes(json);
                request.ContentLength = byte1.Length;

                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                using (httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        string response1 = new StreamReader(stream).ReadToEnd().Replace("{}", string.Empty);
                        object myClass = Newtonsoft.Json.JsonConvert.DeserializeObject(response1);

                        result = JsonConvert.DeserializeObject<List<KlaviyoProfileRes>>(myClass.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static HttpWebRequest GetWebRequestObject(string endPoint)
        {
            HttpWebRequest request = WebRequest.Create(endPoint) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = false;
            request.Timeout = 15 * 60000;
            return request;
        }

        public void EventAPI(KlaviyoEvent ev, string apiKey)
        {
            KlaviyoGateway gateway = new KlaviyoGateway(apiKey);
            ev.Token = gateway.Token;
            gateway.Track(ev);
        }

        public void PeopleAPI(List<NotRequiredProperty> list, string apiKey)
        {
            KlaviyoGateway gateway = new KlaviyoGateway(apiKey);
            KlaviyoPeople pe = new KlaviyoPeople()
            {
                Token = gateway.Token,
            };
            pe.Properties.NotRequiredProperties = list;
            gateway.Identify(pe);
        }

    }
}
