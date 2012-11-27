﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using WebAPI.Cors;
using zvs.Entities;
using zvs.Processor;
using zvs.Processor.Logging;

namespace WebAPI.Controllers.v2
{
    [Documentation("v2/Scenes", 2.1, @"All scenes.

    Change scene name: PATCH /Scenes/{scene.id}
    {
      ""Name"": ""Movie Mode Bright"",
    }

    Run scene: PATCH /Scenes/{scene.id}
    {
      ""isRunning"": true
    }

")]
    public class ScenesController : zvsEntityController<Scene>
    {
        public ScenesController(WebAPIPlugin webAPIPlugin) : base(webAPIPlugin) { }        

        protected override DbSet DBSet
        {
            get { return db.Scenes; }
        }

        protected override IQueryable<Scene> BaseQueryable
        {
            get
            {
                //We need to hide the scenes the user chooses from all CRUD operations.
                List<Scene> scenes = new List<Scene>();
                foreach (Scene s in DBSet)
                {
                    bool show = true;
                    bool.TryParse(ScenePropertyValue.GetPropertyValue(db, s, "WebAPI_SHOW_SCENE"), out show);
                    if (show) scenes.Add(s);
                }
                return scenes.AsQueryable();
            }
        }

        [EnableCors]
        [HttpGet]
        [DTOQueryable]
        public new IQueryable<Scene> Get()
        {
            return base.Get();
        }

        [EnableCors]
        [HttpGet]
        public new HttpResponseMessage GetById(int id)
        {
            return base.GetById(id);
        }

        [EnableCors]
        [HttpPost]
        public new HttpResponseMessage Add(Scene tEntityPost)
        {
            return base.Add(tEntityPost);
        }

        [EnableCors]
        [HttpPatch]
        [HttpPut]
        public new HttpResponseMessage Update(int id, Delta<Scene> scene)
        {
            DenyUnauthorized();

            //if isRunning = true is sent, lets start the scene.
            Scene s = BaseQueryable.Where(o => o.Id == id).SingleOrDefault();
            if (s == null)            
                return Request.CreateResponse(ResponseStatus.Error, HttpStatusCode.NotFound, "Resource not found");

            if (scene != null)
            {
                object isRunning = false;
                scene.TryGetPropertyValue("isRunning", out isRunning);
                
                if ((bool)isRunning)
                {
                    BuiltinCommand cmd = db.BuiltinCommands.FirstOrDefault(c => c.UniqueIdentifier == "RUN_SCENE");
                    if (cmd != null)
                    {
                        CommandProcessor cp = new CommandProcessor(this.WebAPIPlugin.Core);
                        cp.RunBuiltinCommand(db, cmd, s.Id.ToString());
                        return Request.CreateResponse(ResponseStatus.Success, HttpStatusCode.OK, "Scene started. No other changes to the scene made.");
                    }
                }
            }

            //update standard properties such as scene.Name
            return base.Update(id, scene);
        }

        [EnableCors]
        [HttpDelete]
        public new HttpResponseMessage Remove(int id)
        {
            return base.Remove(id);
        }

        [EnableCors]
        [HttpGet]
        [DTOQueryable]
        public new IQueryable<object> GetNestedCollection(int parentId, string nestedCollectionName)
        {
            return base.GetNestedCollection(parentId, nestedCollectionName);
        }


       

      

        //    [AcceptVerbs("POST")]
        //    public object Post(int id)
        //    {
        //        base.Log(log, "id=", id);

        //        using (zvsContext context = new zvsContext())
        //        {
        //            Scene scene = context.Scenes.FirstOrDefault(s => s.Id == id);

        //            if (scene != null)
        //            {
        //                return Post(scene.Name);
        //            }
        //        }
        //        return new { success = false, reason = "Scene not found." };
        //    }
        //    [AcceptVerbs("POST")]
        //    public object Post(string name)
        //    {
        //        base.Log(log, "name=", name);

        //        using (zvsContext context = new zvsContext())
        //        {
        //            Scene scene = context.Scenes.FirstOrDefault(s => s.Name == name);

        //            if (scene != null)
        //            {
        //                BuiltinCommand cmd = context.BuiltinCommands.FirstOrDefault(c => c.UniqueIdentifier == "RUN_SCENE");
        //                if (cmd != null)
        //                {
        //                    CommandProcessor cp = new CommandProcessor(Core);
        //                    cp.RunBuiltinCommand(context, cmd, scene.Id.ToString());
        //                }
        //                return new { success = true, desc = "Scene Started." };
        //            }
        //        }

        //        return new { success = false, reason = "Scene not found." };
        //    }
        
    }

}