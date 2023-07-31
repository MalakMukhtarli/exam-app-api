using ExamApp.API.Models.Teachers;
using ExamApp.API.Routes;
using ExamApp.API.Services.Teachers;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TeacherController : ControllerBase
{
    [HttpGet(ApiRoutes.Teacher.GetAll)]
    public async Task<IActionResult> GetAll([FromServices] ITeacherService service)
    {
        var data = await service.GetAll();
        return Ok(data);
    }

    [HttpPost(ApiRoutes.Teacher.Create)]
    public async Task<IActionResult> Create([FromBody] SaveTeacherRequest request,
        [FromServices] ITeacherService service)
    {
        var data = await service.Create(request);
        return Ok(data);
    }

    [HttpPost(ApiRoutes.Teacher.CreateLessonGrade)]
    public async Task<IActionResult> CreateLessonGradeTeacher([FromRoute] int teacherId,
        [FromBody] List<SaveLessonGradeTeacherRequest> requests, [FromServices] ITeacherService service)
    {
        await service.CreateLessonGradeTeacher(teacherId, requests);
        return Ok();
    }

    [HttpGet(ApiRoutes.Teacher.Get)]
    public async Task<IActionResult> Get([FromRoute] int teacherId, [FromServices] ITeacherService service)
    {
        var data = await service.GetById(teacherId);
        return Ok(data);
    }

    [HttpPut(ApiRoutes.Teacher.Update)]
    public async Task<IActionResult> Update([FromRoute] int teacherId, [FromBody] List<UpdateTeacherRequest> requests,
        [FromServices] ITeacherService service)
    {
        var data = await service.Update(teacherId, requests);
        return Ok(data);
    }

    [HttpDelete(ApiRoutes.Teacher.Delete)]
    public async Task<IActionResult> Delete([FromRoute] int teacherId, [FromServices] ITeacherService service)
    {
        await service.Delete(teacherId);
        return Ok();
    }
}