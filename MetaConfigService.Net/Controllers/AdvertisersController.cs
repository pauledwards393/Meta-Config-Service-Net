using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using MetaConfigService.Net.Services;

namespace MetaConfigService.Net.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AdvertisersController : ApiController
    {
        private readonly IAdvertisersService _advertisersService;

        public AdvertisersController(IAdvertisersService advertisersService)
        {
            _advertisersService = advertisersService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Index()
        {
            var advertisers = await _advertisersService.GetAdvertisers();

            return Ok(advertisers.Count());
        }

        [HttpGet]
        public async Task<IHttpActionResult> Index(String search)
        {
            var advertisers = await _advertisersService.GetAdvertisers();

            var filteredAdvertisers = advertisers
                .Where(
                    x =>
                        x["Brands"].AsBsonArray.Any(
                            y =>
                                y["Inventories"].AsBsonArray.Any(
                                    z =>
                                        z["Name"].AsString.ToLower().Contains(search)))
                )
                .Select(x => new
                    {
                        id = x["_id"].AsObjectId.ToString(),
                        name = x["Name"].AsString
                    }
                )
                .OrderBy(x => x.name);

            return Ok(filteredAdvertisers);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Filter(Int32 n)
        {
            var advertisers = await _advertisersService.GetAdvertisers();

            var filteredAdvertisers = advertisers
                .Where((x, i) => i % n == 0 && x["Brands"].AsBsonArray.Any())
                .SelectMany(
                    x =>
                        x["Brands"].AsBsonArray.Select(
                            y => new {advertiser = x["Name"].AsString, brand = y["Name"].AsString}))
                .OrderBy(x => x.advertiser)
                .ThenBy(x => x.brand);

            return Ok(filteredAdvertisers);
        }
    }
}
