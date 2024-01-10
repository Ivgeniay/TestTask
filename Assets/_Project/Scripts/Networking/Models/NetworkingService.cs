using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using Clock.Services;
using System;

namespace Clock.Networking
{
    internal class NetworkingService : IService
    {
        public const string URL = "https://yandex.com/time/sync.json";

        public IEnumerator SendRequestTimeData(Action<DateTime> onSuccess, Action<ErrorDataModel> onError = null)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(URL))
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
    }
}
