using Microsoft.AspNetCore.Mvc;
using HotellAPI.Data;
using SharedModels;
using Microsoft.EntityFrameworkCore;

namespace HotellAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly HotellContext _context;

        public RoomsController(HotellContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            return Ok(await _context.Rooms.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            return room == null ? NotFound() : Ok(room);
        }

        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom([FromBody] Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoom), new { id = room.RoomId }, room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room room)
        {
            if (id != room.RoomId) return BadRequest();
            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Rooms.AnyAsync(r => r.RoomId == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly HotellContext _context;

        public ReservationsController(HotellContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetReservations()
        {
            return Ok(await _context.Reservations.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            return reservation == null ? NotFound() : Ok(reservation);
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.ReservationId }, reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            if (id != reservation.ReservationId) return BadRequest();
            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Reservations.AnyAsync(r => r.ReservationId == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly HotellContext _context;

        public TasksController(HotellContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SharedModels.Task>>> GetTasks()
        {
            return Ok(await _context.Tasks.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SharedModels.Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<SharedModels.Task>> CreateTask([FromBody] SharedModels.Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] SharedModels.Task task)
        {
            if (id != task.Id) return BadRequest();
            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Tasks.AnyAsync(t => t.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }

}


