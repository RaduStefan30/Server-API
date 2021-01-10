using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentApp.API.Data;
using StudentApp.API.Dtos;
using StudentApp.API.Helpers;
using StudentApp.API.Models;

namespace StudentApp.API.Controllers

{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;
        public CoursesController(IStudentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetCourse")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var courseFromRepo = await _repository.GetCourse(id);

            if (courseFromRepo == null)
                return NotFound();

            return Ok(courseFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages( 
            [FromQuery]CoursesParams coursesParams)
        {
            var coursesFromRepo = await _repository.GetCourses(coursesParams);

            var courses = _mapper.Map<IEnumerable<CourseToReturnDto>>(coursesFromRepo);

            Response.AddPagination(coursesFromRepo.CurrentPage, coursesFromRepo.PageSize, 
                coursesFromRepo.TotalCount, coursesFromRepo.TotalPages);
            
            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseToCreateDto courseToCreateDto)
        {
        //     if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //         return Unauthorized();


            var course = _mapper.Map<Course>(courseToCreateDto);

            _repository.Add(course);

            var courseToReturn = _mapper.Map<CourseToCreateDto>(course);

            if (await _repository.SaveAll())
                 return CreatedAtRoute("GetCourse", new { id = course.Id }, courseToReturn);

            throw new Exception("Failed while creating the note");
        }
    }
}