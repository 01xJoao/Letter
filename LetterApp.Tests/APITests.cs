using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LetterApp.Core.Serialization;
using LetterApp.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.RequestModels;
using Newtonsoft.Json;

namespace LetterApp.Tests
{
    public class APITests
    {
        private HttpClient httpClient;
        private static string baseAddress = "http://www.lettermessenger.com/api/";
        private static string email = "zedo@gmail.com";
        private static string password = "PassNova";
        private static string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9sZXR0ZXJtZXNzZW5nZXIuY29tIiwiaWF0IjoxNTIzODM4NTU0LCJleHAiOjE1MjY0MzA1NTQsI" +
            "mNvbnRleHQiOnsidXNlciI6eyJlbWFpbCI6InBlZHJvQGlwdmMucHQifX19.1v20mHf04n2vE83NoXCSB0st5O4iGJCn0k3fS71xc2k";

        //[Fact]
        //public async Task GetToken()
        //{
        //    httpClient = GetHttpClient(null);
        //    var user = await GetLoginAsync();
        //    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AuthToken);

        //    Assert.NotNull(user.AuthToken);
        //}

        #region user

        ////[Fact]
        //public async Task ChangePassword_Forgoten()
        //{
        //    httpClient = GetHttpClient(null);

        //    var request = new PasswordChangeRequestModel
        //    {
        //        Email = email,
        //        NewPassword = "PassNova",
        //        RequestedCode = "CSWQ5T"
        //    };

        //    var serializedObject = JsonConvert.SerializeObject(request);
        //    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //    var httprequest = await httpClient.PostAsync("users/forgotpassword", content);
        //    var result = await Deserialize<BaseModel>(httprequest);
        //    Assert.Equal(201, result.Code);
        //}

        ////[Fact]
        //public async Task GetUsersFromDivision()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("users/division/1");
        //    var result = Deserialize<List<GetUsersInDivisionModel>>(request);
        //    Debug.WriteLine(result.Result[0].FirstName);
        //}

        ////[Fact]
        //public async Task VerifyDivisionCode()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("users/division/verify/FGHRT2");
        //    var result = Deserialize<DivisionModel>(request);
        //    Debug.WriteLine(result.Result.Name);
        //}

        ////[Fact]
        //public async Task SetUserDivision()
        //{
        //    httpClient = GetHttpClient(token);

        //    var request = new UserDivisionRequestModel
        //    {
        //        DivisionId = 1,
        //        UserId = 15,
        //        PositionId = 1,
        //        IsAdmin = false
        //    };

        //    var serializedObject = JsonConvert.SerializeObject(request);
        //    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //    var httprequest = await httpClient.PostAsync("users/setuserdivision", content);
        //    var result = await Deserialize<BaseModel>(httprequest);
        //    Assert.Equal(202, result.Code);
        //}

        ////[Fact]
        //public async Task UpdateUserPosition()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("users/update/position/2");
        //    var result = Deserialize<BaseModel>(request);
        //    Assert.Equal(203, result.Result.Code);
        //}

        ////[Fact]
        //public async Task UpdateUserPhone()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("users/update/cellphone/12345");
        //    var result = Deserialize<BaseModel>(request);
        //    Assert.Equal(204, result.Result.Code);
        //}

        ////[Fact]
        //public async Task UpdateUserShowPhone()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("users/update/showcellphone/1");
        //    var result = Deserialize<BaseModel>(request);
        //    Assert.Equal(204, result.Result.Code);
        //}

        ////[Fact]
        //public async Task UpdateUserDescription()
        //{
        //    httpClient = GetHttpClient(token);

        //    var request = new CommunUpdateRequestModel
        //    {
        //        Description = "Teste Description"
        //    };

        //    var serializedObject = JsonConvert.SerializeObject(request);
        //    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //    var httprequest = await httpClient.PostAsync("users/update/description", content);
        //    var result = await Deserialize<BaseModel>(httprequest);
        //    Assert.Equal(204, result.Code);
        //}

        ////[Fact]
        //public async Task UpdateUserPicture()
        //{
        //    httpClient = GetHttpClient(token);

        //    var request = new CommunUpdateRequestModel
        //    {
        //        Picture = "iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg=="
        //    };

        //    var serializedObject = JsonConvert.SerializeObject(request);
        //    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //    var httprequest = await httpClient.PostAsync("users/update/picture", content);
        //    var result = await Deserialize<BaseModel>(httprequest);
        //    Assert.Equal(204, result.Code);
        //}

        ////[Fact]
        //public async Task UpdateUserPassword()
        //{
        //    httpClient = GetHttpClient(token);

        //    var request = new CommunUpdateRequestModel
        //    {
        //        OldPassword = "123456789",
        //        NewPassword = "PassNova"
        //    };

        //    var serializedObject = JsonConvert.SerializeObject(request);
        //    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //    var httprequest = await httpClient.PostAsync("users/update/password", content);
        //    var result = await Deserialize<BaseModel>(httprequest);
        //    Assert.Equal(204, result.Code);
        //}

        //[Fact]
        //public async Task UserLeaveDivision()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("users/division/leave/2");
        //    var result = Deserialize<BaseModel>(request);
        //    Assert.Equal(206, result.Result.Code);
        //}

        ////[Fact]
        //public async Task DesactivateUser()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("users/deactivate");
        //    var result = Deserialize<BaseModel>(request);
        //    Assert.Equal(205, result.Result.Code);
        //}

        #endregion

        #region usercheck

        ////[Fact]
        //public async Task CheckTokens()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("usercheck/tokens");
        //    var result = Deserialize<CheckTokensModel>(request);
        //    Assert.Null(result.Result.AuthToken);
        //}

        ////[Fact]
        //public async Task CheckUser()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("usercheck/user");
        //    var result = Deserialize<CheckUserModel>(request);
        //    Assert.NotNull(result);
        //}

        #endregion

        #region registration

        //[Fact]
        //public async Task UserRegistration()
        //{
        //    httpClient = GetHttpClient(null);

        //    var modelobj = new UserRegistrationRequestModel
        //    {
        //        Email = "unit@test.com",
        //        Phone = "911",
        //        FirstName = "Jota",
        //        LastName = "Pal",
        //        Password = "LOLQRIR"
        //    };

        //    var serializedObject = JsonConvert.SerializeObject(modelobj);
        //    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //    var httprequest = await httpClient.PostAsync("registration/add", content);
        //    var result = await Deserialize<TokensModel>(httprequest);
        //    Assert.Equal(110, result.Code);
        //}

        //[Fact]
        //public async Task UserActivationCode()
        //{
        //    httpClient = GetHttpClient(null);

        //    var modelobj = new ActivationCodeRequestModel
        //    {
        //        Email = "unit@test.com",
        //        ActivationCode = "YJ5N7F"
        //    };

        //    var serializedObject = JsonConvert.SerializeObject(modelobj);
        //    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //    var httprequest = await httpClient.PostAsync("registration/activate", content);
        //    var result = await Deserialize<BaseModel>(httprequest);
        //    Assert.Equal(200, result.Code);
        //}

        //[Fact]
        //public async Task ResendCode()
        //{
        //    httpClient = GetHttpClient(null);
        //    var request = await httpClient.GetAsync("registration/resendcode/zedo@gmail.com/true");
        //    var result = await Deserialize<BaseModel>(request);
        //    Assert.Equal(200, result.Code);
        //}

        #endregion

        #region profiles

        ////[Fact]
        //public async Task GetUserProfile()
        //{
        //    httpClient = GetHttpClient(token);

        //    //string dateTime = default(DateTime).ToString("yyyy-MM-dd HH:mm:ss");
        //    string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //    var modelobj = new UserProfileRequestModel
        //    {
        //        UserID = 1,
        //        LastUpdate = dateTime
        //    };

        //    var serializedObject = JsonConvert.SerializeObject(modelobj);
        //    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //    var httprequest = await httpClient.PostAsync("profiles/user", content);
        //    var result = await Deserialize<UserProfileModel>(httprequest);
        //    Assert.Equal(200, result.Code);
        //}

        ////[Fact]
        //public async Task GetDivisionProfile()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("profiles/division/1");
        //    var result = await Deserialize<DivisionProfileModel>(request);
        //    Assert.Equal("EF3AD1", result.AccessCode);
        //}

        //[Fact]
        //public async Task GetOrganizationProfile()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("profiles/organization/1");
        //    var result = await Deserialize<OrganizationModel>(request);
        //    Assert.Equal("IPVC", result.Name);
        //}

        #endregion

        #region organization

        ////[Fact]
        //public async Task VerifyOrganizationName()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("organization/verifyname/IPVC");
        //    var result = await Deserialize<OrganizationAccessModel>(request);
        //    Assert.False(result.RequiresAccessCode);
        //}

        ////[Fact]
        //public async Task VerifyOrganizationCode()
        //{
        //    httpClient = GetHttpClient(token);

        //        var modelobj = new OrganizationRequestModel
        //        {
        //            Email = "geral@ipvc.pt",
        //            AccessCode = "test_access15"
        //        };

        //        var serializedObject = JsonConvert.SerializeObject(modelobj);
        //        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

        //    var request = await httpClient.PostAsync("organization/verifycode", content);
        //    var result = await Deserialize<List<DivisionModel>>(request);
        //    Assert.Equal("ESTG", result[0].Name);
        //}

        //[Fact]
        //public async Task GetPositions()
        //{
        //    httpClient = GetHttpClient(token);
        //    var request = await httpClient.GetAsync("organization/getpositions/1");
        //    var result = await Deserialize<List<PositionModel>>(request);
        //    Assert.Equal("Professor", result[0].Name);
        //}

        #endregion

        #region Private Methods

        private HttpClient GetHttpClient(string accessToken = null)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.Timeout = TimeSpan.FromMinutes(5);

            if (!string.IsNullOrEmpty(accessToken))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            return client;
        }

        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
                return default(T);

            var json = await httpResponseMessage.Content.ReadAsStringAsync();
            var jsonConverter = new JsonConvert<T>();
            var deserializedData = JsonConvert.DeserializeObject<T>(json, jsonConverter);
            return deserializedData;
        }

        private async Task<UserModel> GetLoginAsync()
        {
            var loginRequest = new UserRequestModel
            {
                Email = email,
                Password = password
            };

            var serializedObject = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync("users/login", content);
            return await Deserialize<UserModel>(httpResponseMessage);
        }

        #endregion Private Methods
    }
}