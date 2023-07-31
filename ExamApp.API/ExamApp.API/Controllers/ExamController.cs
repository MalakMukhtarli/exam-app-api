using ExamApp.API.Models.Exams;
using ExamApp.API.Models.PupilExams;
using ExamApp.API.Repository;
using ExamApp.API.Routes;
using ExamApp.API.Services.Exams;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ExamController : ControllerBase
{
    [HttpGet(ApiRoutes.Exam.GetAll)]
    public async Task<IActionResult> GetAll([FromServices] IExamService service)
    {
        var data = await service.GetAll();
        return Ok(data);
    }
    
    [HttpPost(ApiRoutes.Exam.Create)]
    public async Task<IActionResult> Create([FromBody] SaveExamRequest request, [FromServices] IExamService service)
    {
        var data = await service.Create(request);
        return Ok(data);
    }
    
    [HttpGet(ApiRoutes.Exam.Get)]
    public async Task<IActionResult> Get([FromRoute] int examId, [FromServices] IExamService service)
    {
        var data = await service.GetById(examId);
        return Ok(data);
    }
    
    [HttpPut(ApiRoutes.Exam.Update)]
    public async Task<IActionResult> Update([FromRoute] int examId, [FromBody] UpdateExamRequest request, [FromServices] IExamService service)
    {
        var data = await service.Update(examId, request);
        return Ok(data);
    }
    
    [HttpPut(ApiRoutes.Exam.UpdatePupilExam)]
    public async Task<IActionResult> UpdatePupilExam([FromBody] UpdatePupilExamRequest request, [FromServices] IExamService service)
    {
        var data = await service.UpdatePupilExam(request);
        return Ok(data);
    }
    
    [HttpDelete(ApiRoutes.Exam.Delete)]
    public async Task<IActionResult> Delete([FromRoute]int examId, [FromServices] IExamService service)
    {
        await service.Delete(examId);
        return Ok();
    }
}