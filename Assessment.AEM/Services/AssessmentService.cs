using Assessment.AEM.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace Assessment.AEM.Services
{
    public class AssessmentService
    {
        private readonly DatabaseContext _dbContext;
        public AssessmentService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public async Task<string> SavePlatformActualData(string loginusername, string loginpassword)
        {
            LoginCredential login = new LoginCredential()
            {
                username = loginusername,
                password = loginpassword,
            };

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage httpResponse = await httpClient.PostAsJsonAsync("http://test-demo.aemenersol.com/api/Account/Login", login);  //.PostAsync(url, httpContent); //user id apa????????
            string resp = await httpResponse.Content.ReadAsStringAsync();
            string apiresponse = JsonConvert.DeserializeObject<string>(resp);

            return apiresponse;

        }
        //username = user@aemenersol.com & password = Test@123
        public async Task<string> SavePlatform(string loginusername, string loginpassword)
        {
            try
            {
                var token = await SavePlatformActualData(loginusername, loginpassword);
                if (token != null)
                {
                    //http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellDummy
                    var url = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage httpResponse = await httpClient.GetAsync(url);  //.PostAsync(url, httpContent);
                    string resp = await httpResponse.Content.ReadAsStringAsync();
                    List<Platform> platforms = JsonConvert.DeserializeObject<List<Platform>>(resp);

                    foreach (var platform in platforms)
                    {
                        var platform_exist = await _dbContext.Platform.FindAsync(platform.id);

                        if (platform_exist != null)
                        {
                            platform_exist.uniqueName = platform.uniqueName;
                            platform_exist.longitude = platform.longitude;
                            platform_exist.latitude = platform.latitude;
                            platform_exist.createdAt = platform.createdAt;
                            platform_exist.updatedAt = platform.updatedAt;

                            await _dbContext.SaveChangesAsync();

                            foreach (var well in platform.Well)
                            {

                                var well_exist = await _dbContext.Well.FindAsync(well.id);
                                if (well_exist != null)
                                {
                                    well_exist.uniqueName = well.uniqueName;
                                    well_exist.longitude = well.longitude;
                                    well_exist.latitude = well.latitude;
                                    well_exist.createdAt = well.createdAt;
                                    well_exist.updatedAt = well.updatedAt;

                                    await _dbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    var wellItem = new Well()
                                    {
                                        uniqueName = well.uniqueName,
                                        longitude = well.longitude,
                                        latitude = well.latitude,
                                        createdAt = well.createdAt,
                                        updatedAt = well.updatedAt,
                                    };
                                    _dbContext.Well.Add(wellItem);
                                    await _dbContext.SaveChangesAsync();
                                }

                            }
                        }
                        else
                        {
                            var platformItem = new Platform()
                            {
                                //id = platform.id,
                                uniqueName = platform.uniqueName,
                                longitude = platform.longitude,
                                latitude = platform.latitude,
                                createdAt = platform.createdAt,
                                updatedAt = platform.updatedAt,
                                Well = new List<Well>()
                            };

                            foreach (var well in platform.Well)
                            {
                                platformItem.Well.Add(new Well()
                                {
                                    uniqueName = well.uniqueName,
                                    longitude = well.longitude,
                                    latitude = well.latitude,
                                    createdAt = well.createdAt,
                                    updatedAt = well.updatedAt,
                                });
                            }
                            _dbContext.Platform.Add(platformItem);
                            await _dbContext.SaveChangesAsync();
                        }


                    }

                    return "Success";
                }
                else
                    return "Wrong Credentials";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
            

        }
    }
}
