using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using Clock.Services;
using System;

namespace Clock.Networking
{
    internal class NetworkingService : IService
    {
        public const string CorsAnywhereUrl = "https://corsproxy.io/?";
        public const string TargetUrl = "https://yandex.com/time/sync.json";

        public IEnumerator SendRequestTimeData(Action<DateTime> onSuccess, Action<ErrorDataModel> onError = null)
        {
#if UNITY_WEBGL
            yield return SendRequestTimeDataWeb(onSuccess, onError);
#endif

#if UNITY_EDITOR
            yield return SendRequestTimeDataEditor(onSuccess, onError);
#endif

        }


#if UNITY_WEBGL
        public IEnumerator SendRequestTimeDataWeb(Action<DateTime> onSuccess, Action<ErrorDataModel> onError = null)
        {
            string fullUrl = $"{CorsAnywhereUrl}{TargetUrl}";

            using (UnityWebRequest request = UnityWebRequest.Get(fullUrl))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string response = request.downloadHandler.text;
                    DateTimeRequensModel dateModel = JsonConvert.DeserializeObject<DateTimeRequensModel>(response);

                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(dateModel.time);
                    DateTime dateTime = dateTimeOffset.DateTime;

                    onSuccess.Invoke(dateTime);
                }
                else
                {
                    ErrorDataModel errorData = new ErrorDataModel()
                    {
                        Error = request.error,
                        RensponceCode = (int)request.responseCode
                    };
                    onError?.Invoke(errorData);
                }
            }
        }
#endif

#if UNITY_EDITOR
        public IEnumerator SendRequestTimeDataEditor(Action<DateTime> onSuccess, Action<ErrorDataModel> onError = null)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(TargetUrl))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string response = request.downloadHandler.text;
                    DateTimeRequensModel dateModel = JsonConvert.DeserializeObject<DateTimeRequensModel>(response);

                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(dateModel.time);
                    DateTime dateTime = dateTimeOffset.DateTime;

                    onSuccess.Invoke(dateTime);
                }
                else
                {
                    ErrorDataModel errorData = new ErrorDataModel()
                    {
                        Error = request.error,
                        RensponceCode = (int)request.responseCode
                    };
                    onError.Invoke(errorData);
                }
            }
        }
#endif
    }
}
