using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PaymentGatewayService.Api.ViewModels;
using PaymentGatewayService.Api.ViewModels.Authentication;

namespace PaymentGatewayService.Api.Client
{
    public class PaymentServiceClient : IPaymentServiceClient
    {
        private readonly PaymentServiceClientParameters _parameters;

        public PaymentServiceClient(PaymentServiceClientParameters parameters)
        {
            _parameters = parameters;
        }

        public async Task<string> Login(UserLoginRequest request)
        {
            var url = $"api/v{_version}/datasets/{dataSetId}/files";
            return PostAsync<FileReferenceViewModel>(url, viewModel, properties);
        }

        public async Task<PaymentViewModel> CreatePayment(CreatePaymentRequest request)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_parameters.BaseUrl}/payment";
                client.BaseAddress = new Uri(url);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("value", postData)
                });

                var result = await client.PostAsync("/api/Membership/exists", content);
                var resultContent = await result.Content.ReadAsStringAsync();
            }
        }

        public async Task<PaymentViewModel> GetPayment(string paymentId)
        {
            throw new System.NotImplementedException();
        }

        private async Task<ResponseObject<T>> PostAsync<T>(
            string url,
            object content)
        {
            if (string.IsNullOrWhiteSpace(properties?.AccessToken))
                await this.EnsureTokenIsSet();

            string baseUrl = this._parameters.DetermineBaseUrl(properties, url);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(baseUrl);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + (properties?.AccessToken ?? this.AccessToken.Token));
            httpRequestMessage.Content = (HttpContent) new JsonContent(content);
            HttpRequestMessage request = httpRequestMessage;
            ResponseObject<T> responseObject1;

            ResponseObject<T> responseObject2 = await this.ConvertResponse<T>(await this.HttpClient.SendAsync(request));
            if (responseObject2.IsError)
                this._logger.LogError("Error: " + responseObject2.Error.Code + " ocurred calling URL: " + url);
            responseObject1 = responseObject2;

            return responseObject1;
        }
    }
}