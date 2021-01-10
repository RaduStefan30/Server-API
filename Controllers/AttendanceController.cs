using AutoMapper;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StudentApp.API.Data;
using StudentApp.API.Dtos;
using StudentApp.API.Helpers;
using StudentApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentApp.API.Controllers
{
    [Route("users/{userId}/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IStudentRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public AttendanceController(IStudentRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendance(int userId, [FromQuery]AttendanceParams attendanceParams)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            attendanceParams.UserId = userId;

            var attendanceFromRepo = await _repo.GetAttendance(attendanceParams);

            var attendance = _mapper.Map<IEnumerable<AttendanceCreationDto>>(attendanceFromRepo);

            Response.AddPagination(attendanceFromRepo.CurrentPage, attendanceFromRepo.PageSize,
                attendanceFromRepo.TotalCount, attendanceFromRepo.TotalPages);

            return Ok(attendance);
        }

        [HttpPost]
        public async Task<IActionResult> TakeAttendance(int userId, AttendanceCreationDto attendanceCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            attendanceCreationDto.UserId = userId;

            var attendance = _mapper.Map<Attendance>(attendanceCreationDto);

            _repo.Add(attendance);

            var attendanceToReturn = _mapper.Map<AttendanceCreationDto>(attendance);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Failed while creating the note");
        }
    }
}
