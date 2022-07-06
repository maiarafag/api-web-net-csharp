using System;
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
    public class AlunoController : ControllerBase
    {
        private readonly InstituicaoContext _context;

        public AlunoController(InstituicaoContext context)
        {
            _context = context;
        }

        // GET: api/Aluno
        [HttpGet]
        public async Task<JsonResult> GetAlunos()
        {
          if (_context.Alunos == null)
          {
              return new JsonResult("Não existe alunos cadastrado.");
          }
            List<Aluno> aluno = await _context.Alunos.ToListAsync();

            return new JsonResult(new
            {
                AlunosAtivos = aluno.FindAll(e => e.Turma.Ativo == true),
            });
        }

        // GET: api/Aluno/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aluno>> GetAluno(int id)
        {
          if (_context.Alunos == null)
          {
              return NotFound();
          }
            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            return aluno;
        }

        // PUT: api/Aluno/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAluno(int id, Aluno aluno)
        {
            if (id != aluno.Id)
            {
                return BadRequest();
            }

            var turma = await _context.Turma.FindAsync(aluno.Turma_Id);

           // _context.Entry(aluno).State = EntityState.Modified;

            try
            {
                if (TurmaExists(aluno.Turma_Id) && turma.Ativo)
                {
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Não foi possível mudar o aluno para esta turma.");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlunoExists(id))
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

        // POST: api/Aluno
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Aluno>> PostAluno(Aluno aluno)
        {
          if (_context.Alunos == null)
          {
              return Problem("Entity set 'InstituicaoContext.Alunos'  is null.");
          }
            if (aluno.Turma_Id > 0 && TurmaExists(aluno.Turma_Id))
            {

                _context.Alunos.Add(aluno);
                await _context.SaveChangesAsync();

            return CreatedAtAction("GetAluno", new { id = aluno.Id }, aluno);
            }
            else
            {
                throw new Exception("Só é possível cadastrar aluno se a turma for válida.");
            }

        }

        // DELETE: api/Aluno/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluno(int id)
        {
            if (_context.Alunos == null)
            {
                return NotFound();
            }
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlunoExists(int id)
        {
            return (_context.Alunos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool TurmaExists(int id)
        {
            return (_context.Turma?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
