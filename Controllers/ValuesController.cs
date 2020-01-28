using System.Net.NetworkInformation;
using System.Net;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestApiApp.Models;
using TestApiApp.Helpers;
using TestApiApp.Services;

namespace TestApiApp.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IAuthenticateService _authService;
        private readonly TokenManagement _tokenManagement;
        private TestApiAppContext _context;
        // private LoginRequest _requestLogin;
        private AppSettings _settings;
        public ValuesController(TestApiAppContext context, IAuthenticateService authService, IOptions<TokenManagement> tokenManagement)
        {
            _context = context;
            // _requestLogin = login;
            _authService = authService;
            _tokenManagement = tokenManagement.Value;
        }

        [AllowAnonymous]
        [HttpPost, Route("Login")]
        public IActionResult LoginAccount(string username, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            string token = string.Empty;
            if (_authService.IsAuthenticated(username, password, out token) != null) {
                return Ok(token);
            }

            return BadRequest("invalid response...");
        }
        
        [AllowAnonymous]
        // [Authorize(Roles = "User")]
        [HttpGet, Route("get")]
        public async Task<IActionResult> GetPlatform()
        {
            string url = "http://test-demo.aem-enersol.com/api/PlatformWell/GetPlatformWellActual";
            string token = AppSettings.GeneratorToken();
            // var info = _authService.IsAuthenticated("user@aemenersol.com", "Test@123", out token);

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token));
            client.DefaultRequestHeaders.Add("User-Agent", "d-fens HttpClient");

            HttpResponseMessage response = await client.GetAsync(url);
            return Ok(response);
           

           
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", "Bearer " + token);

            
            // if (response.IsSuccessStatusCode) 
            // {
                // return Ok(StatusCodes.Status200OK);
            // }
            // else
            // {
            
            // }
        }

        [AllowAnonymous]
        [HttpGet, Route("jwttoken")]
        public IActionResult GetToken()
        {
            string val = TestApiApp.Helpers.AppSettings.TokenGenerator("secret", "user@aemenersol.com");
            return Ok(val);
        }

        [AllowAnonymous]
        [HttpGet, Route("jwttest")]
        public IActionResult GetJwtToken() 
        {
            string val = TestApiApp.Helpers.AppSettings.GeneratorToken();
            return Ok(val);
        }
        
        // insert platform
        [HttpPost, Route("InsertPlatform")]
        public async Task<IActionResult> InsertPlatformData(string uniqueName, double latitude, double longitude, DateTime? createdAt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var item_exist = await _context.PlatformSet.Where(a => a.uniqueName.StartsWith(uniqueName)).FirstOrDefaultAsync();

            if (item_exist == null)
            {
                await _context.PlatformSet.AddAsync(new Platform {
                    uniqueName = uniqueName,
                    latitude = latitude,
                    longitude = longitude,
                    createdAt = createdAt
                });
                
                await _context.SaveChangesAsync();
                return Ok(StatusCodes.Status200OK);
            }
            else
            {
                return Ok("already exist in db...");
            }
        }

        // insert well
        [HttpPost, Route("InsertWell")]
        public async Task<IActionResult> InsertWellData(int platformId, string uniqueName, double latitude, double longitude, DateTime? createdAt)
        {
            if (!ModelState.IsValid) 
            {
                return NotFound();
            }

            var item_exist = await _context.WellSet.Where(a => a.uniqueName.Equals(uniqueName)).FirstOrDefaultAsync();
            if (item_exist == null)
            {
                await _context.WellSet.AddAsync(new Well {
                    platformId = platformId,
                    uniqueName = uniqueName,
                    latitude = latitude,
                    longitude = longitude,
                    createdAt = createdAt
                });
                await _context.SaveChangesAsync();
                return Ok(StatusCodes.Status200OK);
            }
            else
            {
                return Ok("already exist in db...");
            }
        }
        
        // update platform
        [HttpPut, Route("UpdatePlateform/{id}")]
        public async Task<IActionResult> UpdatePlatformData(int id, string uniqueName, double latitude, double longitude, DateTime? updatedAt)
        {
            var item = await _context.PlatformSet.Where(a => a.id == id).FirstOrDefaultAsync();
            if (item != null)
            {
                item.uniqueName = uniqueName;
                item.latitude = latitude;
                item.longitude = longitude;
                item.updatedAt = updatedAt;

                await _context.SaveChangesAsync();
                return Ok("save successfully....");
            }
            else 
            { 
                return Ok("following data not exist...");
            }
        }

        // update well
        [HttpPut, Route("UpdateWell/{id}")]
        public async Task<IActionResult> UpdateWellData(int id, int platformId, string uniqueName, double latitude, double longitude, DateTime? updatedAt)
        {
            if (id == 0) 
            {
                return NotFound();
            }

            var result = await _context.WellSet.Where(a => a.id == id).FirstOrDefaultAsync();
            if (result != null) 
            {
                result.platformId = platformId;
                result.uniqueName = uniqueName;
                result.latitude = latitude;
                result.longitude = longitude;
                result.updatedAt = updatedAt;

                await _context.SaveChangesAsync();
                return Ok(StatusCodes.Status200OK);
            }
            else
            {
                return Ok("following data not exist...");
            }            
        } 

        [HttpGet, Route("GetPlatformWellDummy")]
        public async Task<IActionResult> PlatformWellDetails()
        {
            var result =  await _context.PlatformSet.Select(a => new Platform {
                id = a.id,
                uniqueName = a.uniqueName,
                latitude = a.latitude,
                longitude = a.longitude,
                createdAt = a.createdAt,
                updatedAt = a.createdAt,
                well = _context.WellSet.Where(b => b.platformId == a.id).ToList()
            }).ToListAsync();

            return Ok(result);
        }

    }
}
