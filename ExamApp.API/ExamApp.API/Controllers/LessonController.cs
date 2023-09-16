using ExamApp.API.Routes;
using ExamApp.Business.Models.Lessons;
using ExamApp.Business.Services.Lessons;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LessonController : ControllerBase
{
    [HttpGet(ApiRoutes.Lesson.GetAll)]
    public async Task<IActionResult> GetAll([FromServices] ILessonService service)
    {
        var data = await service.GetAll();
        return Ok(data);
    } 
    
    [HttpPost(ApiRoutes.Lesson.Create)]
    public async Task<IActionResult> Create([FromBody]SaveLessonRequest request, [FromServices] ILessonService service)
    {
        var data = await service.Create(request);
        return Ok(data);
    }
    
    [HttpGet(ApiRoutes.Lesson.Get)]
    public async Task<IActionResult> Get([FromRoute] int lessonId, [FromServices] ILessonService service)
    {
        var data = await service.GetById(lessonId);
        return Ok(data);
    }
    
    [HttpPut(ApiRoutes.Lesson.Update)]
    public async Task<IActionResult> Update([FromRoute] int lessonId, [FromBody] UpdateLessonRequest request, [FromServices] ILessonService service)
    {
        var data = await service.Update(lessonId, request);
        return Ok(data);
    }
    
    [HttpDelete(ApiRoutes.Lesson.Delete)]
    public async Task<IActionResult> Delete([FromRoute] int lessonId, [FromServices] ILessonService service)
    {
        await service.Delete(lessonId);
        return Ok();
    }
    
}