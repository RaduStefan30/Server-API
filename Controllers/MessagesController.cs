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
    [ServiceFilter(typeof(UserActivity))]
    [Authorize]
    [Route("users/{userId}/[controller]",  Name = "Message")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;
        public MessagesController(IStudentRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId, 
            [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var messagesFromRepo = await _repository.GetMessages(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, 
                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);
            
            return Ok(messages);
        }

        [HttpGet("conversation/{receiverId}")]
        public async Task<IActionResult> GetConversation(int userId, int receiverId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesFromRepo=await _repository.GetConversation(userId, receiverId);

            var conversation = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(conversation);
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repository.GetMessage(id);

            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageCreationDto messageCreationDto)
        {
            var sender = await _repository.GetUser(userId);

            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageCreationDto.SenderId = userId;
            
            var receiver = await _repository.GetUser(messageCreationDto.ReceiverId);

            if(receiver==null)
                return BadRequest("No user found!");
            
            var message = _mapper.Map<Message>(messageCreationDto);
            
            _repository.Add(message);


            if(await _repository.SaveAll()){

                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
            
                return CreatedAtRoute("Message",new {id=message.Id}, messageToReturn);
            }

            throw new Exception("Message creation failed...");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();  

            var messageFromRepo = await _repository.GetMessage(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.ReceiverId == userId)
                messageFromRepo.ReceiverDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.ReceiverDeleted)
                _repository.Delete(messageFromRepo);
            
            if (await _repository.SaveAll())
                return NoContent();

            throw new Exception("Error deleting the message");
        }
    }
}