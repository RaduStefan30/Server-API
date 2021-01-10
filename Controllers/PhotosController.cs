using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using Microsoft.ProjectOxford.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace StudentApp.API.Controllers
{
    
    

    [Authorize]
    [Route("users/{userid}/photos/")]

    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IStudentRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IStudentRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
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

        [HttpGet("/{id}",Name ="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);


            if (await _repo.SaveAll()) {

                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }

            return BadRequest("Error while trying to update the photo...");
        }

        [HttpPost("link")]
        public async Task<IActionResult> AddPhotoForUserByLink(int userId, [FromBody]string url)
        {   
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var uploadParams = new ImageUploadParams(){
                File=new FileDescription(url)
            };
            var uploadResult=_cloudinary.Upload(uploadParams);

            var photo=userFromRepo.Photos.Where(u => u.UserId == userId).FirstOrDefault(p => p.IsMain);
            var link=uploadResult.Url.ToString();
            
            // IFaceServiceClient faceClient = CreateFaceClient();


            // var faces = await faceClient.DetectAsync(link,
            //     returnFaceAttributes: faceAttr);

            // static IFaceServiceClient CreateFaceClient() => new FaceServiceClient("173b2a5d649c4db3b104f909f8258f2a",
            // "https://raduapi.cognitiveservices.azure.com/face/v1.0");
            // foreach (var face in faces)
            // {
            //  return Ok(face.FaceLandmarks.);
            // }

            IFaceClient client = new FaceClient(new ApiKeyServiceClientCredentials("173b2a5d649c4db3b104f909f8258f2a"))
                     { Endpoint = "https://raduapi.cognitiveservices.azure.com" };
           
            // var faceAttr = new[] { client.Face..Gender, FromBodyAttribute.Age };
            

        
            var faces1 = await client.Face.DetectWithUrlAsync(link);
            Guid sourceFaceId1 = faces1[0].FaceId.Value;
            var faces2 = await client.Face.DetectWithUrlAsync(photo.Url);
            Guid sourceFaceId2 = faces2[0].FaceId.Value;

            
            VerifyResult verifyResult1 = await client.Face.VerifyFaceToFaceAsync(sourceFaceId1, sourceFaceId2);
            // foreach(var face in faces1)
            // {
            //     return Ok(face);
            // }
            if(verifyResult1.IsIdentical==true)
                return Ok("Welcome");
            else
                return BadRequest("The faces don't match. Please try again.");
        }
        

        [HttpPost("{id}/set")]
        public async Task<IActionResult> SetPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var user = await _repo.GetUser(userId);
            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already your photo!");

            var currentPhoto = await _repo.GetSetPhoto(userId);

            currentPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Failed to change the photo.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var user = await _repo.GetUser(userId);
            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo!");

            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                    _repo.Delete(photoFromRepo);
            }
            else _repo.Delete(photoFromRepo);

            //_repo.Delete(user);
            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Unable to delete the photo");
        }
    }
}
