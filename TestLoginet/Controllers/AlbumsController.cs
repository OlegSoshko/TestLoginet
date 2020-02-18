using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestLoginet.Models;

namespace TestLoginet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private IHttpClientFactory ClientFactory { get; }
        public AlbumsController(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
        }

        // GET /albums.js
        // GET /albums.json
        // GET /albums.xml
        [HttpGet("/albums.{format}"), FormatFilter]
        public async Task<ActionResult<IEnumerable<Album>>> GetAllUser()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "albums");
            var client = ClientFactory.CreateClient("jsonplaceholder");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                IEnumerable<Album> users = await JsonSerializer.DeserializeAsync
                        <IEnumerable<Album>>(responseStream);
                return Ok(users);
            }
            return NotFound();
        }

        // GET /albums/2.js
        // GET /albums/2.json
        // GET /albums/2.xml
        [HttpGet("{id}.{format}"), FormatFilter]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"albums/{id}");
            var client = ClientFactory.CreateClient("jsonplaceholder");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                Album album = await JsonSerializer.DeserializeAsync<Album>(responseStream);
                return Ok(album);
            }
            return NotFound();
        }
    }
}