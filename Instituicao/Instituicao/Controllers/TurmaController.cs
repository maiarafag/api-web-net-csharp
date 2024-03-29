﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Instituicao.Context;
using Instituicao.Models;

namespace Instituicao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurmaController : ControllerBase
    {
        private readonly InstituicaoContext _context;

        public TurmaController(InstituicaoContext context)
        {
            _context = context;
        }

        // GET: api/Turma
        [HttpGet]
        public async Task<JsonResult> GetTurma()
        {
          if (_context.Turma == null)
          {
              return new JsonResult("Não há turma cadastrada.");
          }
            List<Turma> turma = await _context.Turma.ToListAsync();

            return new JsonResult(new
            {
                TurmasAtivas = turma.FindAll(e => e.Ativo == true),
            });
        }

        // GET: api/Turma/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Turma>> GetTurma(int id)
        {
          if (_context.Turma == null)
          {
              return NotFound();
          }
            var turma = await _context.Turma.FindAsync(id);

            if (turma == null)
            {
                return NotFound();
            }

            return turma;
        }

        // PUT: api/Turma/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTurma(int id, Turma turma)
        {
            if (id != turma.Id)
            {
                return BadRequest();
            }

            _context.Entry(turma).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurmaExists(id))
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

        // POST: api/Turma
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Turma>> PostTurma(Turma turma)
        {
          if (_context.Turma == null)
          {
              return Problem("Entity set 'InstituicaoContext.Turma'  is null.");
          }
            _context.Turma.Add(turma);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTurma", new { id = turma.Id }, turma);
        }

        // DELETE: api/Turma/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurma(int id)
        {
            if (_context.Turma == null)
            {
                return NotFound();
            }
            var turma = await _context.Turma.FindAsync(id);
            if (turma == null)
            {
                return NotFound();
            }
            if (!turma.Alunos.Any())
            {
                _context.Turma.Remove(turma);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                throw new Exception("Não é possível deletar uma turma que tenha alunos.");
            }

        }

        private bool TurmaExists(int id)
        {
            return (_context.Turma?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
