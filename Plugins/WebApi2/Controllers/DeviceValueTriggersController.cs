﻿using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using zvs.DataModel;

namespace zvsWebapi2Plugin.Controllers
{
    [ODataRoutePrefix("DeviceValueTriggers")]
    [Authorize(Roles = "All Access")]
    public class DeviceValueTriggersController : WebApi2PuginController
    {
        public DeviceValueTriggersController(WebApi2Plugin webApi2Plugin) : base(webApi2Plugin) { }

        [ODataRoute]
        [EnableQuery(PageSize = 50)]
        public IQueryable<DeviceValueTrigger> Get()
        {
            return Context.DeviceValueTriggers;
        }

        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
            base.Dispose(disposing);
        }

        [EnableQuery]
        public SingleResult<DeviceValueTrigger> Get([FromODataUri] long key)
        {
            var result = Get().Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<DeviceValueTrigger> entityDelta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await Context.DeviceValueTriggers.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            entityDelta.Patch(entity);
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return Updated(entity);
        }
    }



}