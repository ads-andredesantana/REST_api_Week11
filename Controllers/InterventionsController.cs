using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using intervention_management.Models;

namespace Intervention_management.Controllers
{
    [Produces("application/json")]
    [Route("api/interventions")]
    [ApiController]
    public class InterventionsController : ControllerBase
    {
        private readonly Rocket_app_developmentContext _context;
        public InterventionsController(Rocket_app_developmentContext context)
        {
            _context = context;
        }

        // GET: api/interventions: List of all interventions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Interventions>>> GetInterventions()
        {
            return await _context.interventions.ToListAsync();
        }

        // GET: api/interventions/5
        // GET: Returns all fields of all Service Request records that do not have a start date and are in "Pending" status.
        // 1) Getting all existing interventions
        [HttpGet("{id}")]
        public async Task<ActionResult<Interventions>> GetInterventionStatus(long id, String Status)
        {
            var intervention = await _context.interventions.FindAsync(id);

            if (intervention == null)
            {
                return NotFound("Not Found");
            }
            return intervention;
        }

        // GET: api/interventions/pending - Return the intervention list with the "Pending" status without starting date
        [HttpGet("pending")]
        public async Task<ActionResult<List<Interventions>>> GetInterventionsList()
        {

          var list =  await _context.interventions.ToListAsync();

               if (list == null)
            {
                return NotFound();
            }
   
        List<Interventions> interventionList = new List<Interventions>();

        foreach (var intervention in list){

            if (intervention.status == "Pending" && intervention.start_date ==  null){
         
            interventionList.Add(intervention);

            }
        }
             return interventionList;
            }

       //Put the status of the the requested Intervention to "InProgress" and add a starting date and time (Timestamp).
        [HttpPut("{id}/inprogress")]
        public async Task<ActionResult<Interventions>> UpdateIntervention([FromRoute] long id)
        {
            var receivedIntervention = await this._context.interventions.FindAsync(id);
            if (receivedIntervention == null)
            {
                return NotFound();
            }
            else
            {
                receivedIntervention.status = "Pending";
                receivedIntervention.status = "In progress";
                receivedIntervention.start_date = System.DateTime.Now;
            }
            this._context.interventions.Update(receivedIntervention);
            await this._context.SaveChangesAsync();
            return Content("Intenvention status changed - ID: " + receivedIntervention.id +
            " New Status: " + receivedIntervention.status + ". Starting date: "
             + receivedIntervention.start_date + ".");
        }

        //PUT the status of the requested Intervention to "Completed" and add an ending date and time (Timestamp).
        [HttpPut("{id}/completed")]
        public async Task<ActionResult<Interventions>> InterventionCompleted([FromRoute] long id)
        {
            var receivedIntervention = await this._context.interventions.FindAsync(id);
            if (receivedIntervention == null)
            {
                return NotFound();
            }
            else
            {
                receivedIntervention.result = "Successful";
                receivedIntervention.status = "Completed";
                receivedIntervention.end_date = System.DateTime.Now;
            }
            this._context.interventions.Update(receivedIntervention);
            await this._context.SaveChangesAsync();
            return Content("Intenvention status changed - ID: " + receivedIntervention.id +
            " New Status: " + receivedIntervention.status + ". End date: "
             + receivedIntervention.end_date + ".");
        }
    }
}