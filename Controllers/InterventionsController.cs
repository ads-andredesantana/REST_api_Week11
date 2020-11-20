using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using intervention_management.Models;

namespace Intervention_management.Controllers
{
    [Route("api/interventions")]
    [ApiController]
    public class InterventionsController : ControllerBase
    {
        private readonly Rocket_app_developmentContext _context;
        public InterventionsController(Rocket_app_developmentContext context)
        {
            _context = context;
        }


        // GET api/interventions/all : to get the full intervention list
        [HttpGet("all")]
        public ActionResult<List<Interventions>> GetAll()
        {
            return _context.Interventions.ToList();
        }


        // GET api/interventions/pending : to get the intervention list with the "Pending" status and NO Starting Time
        [HttpGet("pending")]
        public ActionResult<List<Interventions>> GetPending()
        {
            var list = _context.Interventions.ToList();
            if (list == null)
            {
                return NotFound("Not Found");
            }

            List<Interventions> list_pending = new List<Interventions>();

            foreach (var i in list)
            {

                if ((i.status == "Pending") && (i.InterventionStartTime == null))
                {
                    list_pending.Add(i);
                }
            }
            return list_pending;
        }


        // GET api/interventions/inprogresslist : to get the intervention list with the "In Progress" status
        [HttpGet("inprogresslist")]
        public ActionResult<List<Interventions>> GetInProgress()
        {
            var list = _context.Interventions.ToList();
            if (list == null)
            {
                return NotFound("Not Found");
            }

            List<Interventions> list_inprogress = new List<Interventions>();

            foreach (var i in list)
            {

                if ((i.status == "In Progress"))
                {
                    list_inprogress.Add(i);
                }
            }
            return list_inprogress;
        }


        // GET api/interventions/5 : to get the status of one particular intervention id
        [HttpGet("{id}", Name = "GetInterventions")]
        public ActionResult GetById(string status, long id)
        {
            var item = _context.Interventions.Find(id);
            if (item == null)
            {
                return NotFound("Not Found");
            }
            var json = new JObject();
            json["status"] = item.status;
            return Content(json.ToString(), "application/json");
        }


        // PUT api/interventions/inprogress/id : to modify a specified intervention status from "Pending" to "In Progress"
        [HttpPut("inprogress/{id}")]
        public string UpdateInProgress(long id)
        {
            var intrv = _context.Interventions.Find(id);
            if (intrv == null)
            {
                return "Please enter an existing intervention id";
            }
            if (intrv.status != "Pending")
            {
                return "Please choose a Pending intervention";
            }
            else
            {
                intrv.status = "In Progress";

                string InterventionStartTime = DateTime.Now.ToString("yyyy/MM/dd H:mm:ss");
                intrv.InterventionStartTime = InterventionStartTime;

                _context.Interventions.Update(intrv);
                _context.SaveChanges();
                return "The intervention #" + intrv.Id + " status has been successufully changed to In Progress at " + intrv.InterventionStartTime;
            }
        }


        // PUT api/interventions/completed/id : to update the status once the intervention is done
        [HttpPut("completed/{id}")]
        public string UpdateCompleted(long id)
        {
            var intrv = _context.Interventions.Find(id);
            if (intrv == null)
            {
                return "Please enter an existing intervention id";
            }
            if (intrv.status != "In Progress")
            {
                return "Please choose an In Progress intervention";
            }
            else
            {
                intrv.status = "Completed";

                string InterventionEndTime = DateTime.Now.ToString("yyyy/MM/dd H:mm:ss");
                intrv.InterventionEndTime = InterventionEndTime;

                _context.Interventions.Update(intrv);
                _context.SaveChanges();
                return "The intervention #" + intrv.Id + " status has been successufully changed to Completed at " + intrv.InterventionEndTime;
            }
        }
    }
}