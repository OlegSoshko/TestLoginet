using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using TestLoginet.Models;

namespace TestLoginet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [FormatFilter]
    public class UsersController : ControllerBase
    {
        private IHttpClientFactory ClientFactory { get; }
        public UsersController(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
        }

        // GET /users.js
        // GET /users.json
        // GET /users.xml
        [HttpGet("/users{format}"), FormatFilter]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUser()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "users");
            var client = ClientFactory.CreateClient("jsonplaceholder");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                IEnumerable<User> users = await JsonSerializer.DeserializeAsync
                        <IEnumerable<User>>(responseStream);
                return Ok(users);
            }
            return NotFound();
        }

        // GET /users/3.js
        // GET /users/3.json
        // GET /users/3.xml
        [HttpGet("{id}.{format}"), FormatFilter]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"users/{id}");
            var client = ClientFactory.CreateClient("jsonplaceholder");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                User user = await JsonSerializer.DeserializeAsync<User>(responseStream);
                return Ok(user);
            }
            return NotFound();
        }

        // GET /users/5/albums.js
        // GET /users/5/albums.json
        // GET /users/5/albums.xml
        [HttpGet("{id}/albums.{format}"), FormatFilter]
        public async Task<ActionResult<IEnumerable<Album>>> GetAllUserAlbums(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"users/{id}/albums");
            var client = ClientFactory.CreateClient("jsonplaceholder");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                IEnumerable<Album> userAlbums = await JsonSerializer.DeserializeAsync<IEnumerable<Album>>(responseStream);
                return Ok(userAlbums);
            }
            return NotFound();
        }
    }
}