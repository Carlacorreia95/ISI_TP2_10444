using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISI_TP2_10444_SmartHealth_Data;
using Microsoft.AspNetCore.Authorization;

namespace ISI_TP2_10444_SmartHealth_MedicalService_Controllers
{
    [Route("api/[controller]")] // Define rout as "api/patients"
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class PatientsController : ControllerBase
    {
        private readonly SmartHealthContext _context; //Dependency Injection: The system delivers the 'SmartHealthContext'
        public PatientsController(SmartHealthContext context)
        {
            _context = context;
        }
        // GET: api/patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients() // Returns the list of all patients.
        {
            return await _context.Patients.AsNoTracking().ToListAsync();
        }
        // GET: api/patients/{id}

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return patient;
        }

        // POST: api/patients
        // Cria um novo paciente
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
        {
            //patient.PatientId = Guid.NewGuid();// New ID to grant it is unic

            _context.Patients.Add(patient);            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                //if (_context.Patients.Any(e => e.PatientId == patient.PatientId)) //Verifica se já existe um ID igual
                //{
                //    return Conflict(); //409 Conflict
                //}
                //else
                //{
                //    throw; // Re-throw the exception if it's not a conflict
                //}
            }

                return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientId }, patient); //CreatedAtAction: Return code 201(Created) and add header with "Location"
            
        }

        // PUT: api/patients/{id}
        // Updates an existing patient
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, Patient patient)
        {
           
            if (id != patient.PatientId)
            {
                return BadRequest("The ID in the URL does not match the ID in the body.");
            }

            
            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            
            return NoContent();
        }

        // Helper method to check if a patient exists (used in UpdatePatient)
        private bool PatientExists(Guid id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }

        //DELETE: api/patients/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}