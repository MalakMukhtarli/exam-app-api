using ExamApp.API.Models.Grades;
using ExamApp.API.Routes;
using ExamApp.API.Services.Grades;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GradeController : ControllerBase
{
    [HttpGet(ApiRoutes.Grade.GetAll)]
    public async Task<IActionResult> GetAll([FromServices] IGradeService service)
    {
        var data = await service.GetAll();
        return Ok(data);
    } 
    
    [HttpPost(ApiRoutes.Grade.Create)]
    public async Task<IActionResult> Create([FromRoute] byte grade, [FromServices] IGradeService service)
    {
        var data = await service.Create(grade);
        return Ok(data);
    } 
    
    [HttpGet(ApiRoutes.Grade.Get)]
    public async Task<IActionResult> Get([FromRoute] int gradeId, [FromServices] IGradeService service)
    {
        var data = await service.GetById(gradeId);
        return Ok(data);
    } 
    
    [HttpPut(ApiRoutes.Grade.Update)]
    public async Task<IActionResult> Update([FromRoute] int gradeId, [FromBody] UpdateGradeRequest request, [FromServices] IGradeService service)
    {
        var data = await service.Update(gradeId, request);
        return Ok(data);
    } 
    
    [HttpDelete(ApiRoutes.Grade.Delete)]
    public async Task<IActionResult> Delete([FromRoute] int gradeId, [FromServices] IGradeService service)
    {
        await service.Delete(gradeId);
        return Ok();
    } 

}